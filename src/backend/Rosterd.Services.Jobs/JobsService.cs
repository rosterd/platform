using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Mappers;

namespace Rosterd.Services.Jobs
{
    public class JobsService : IJobsService
    {
        private readonly IRosterdDbContext _context;
        private readonly ISearchIndexProvider _searchIndexProvider;
        private readonly IBelongsToValidator _belongsToValidator;

        public JobsService(IRosterdDbContext context, ISearchIndexProvider searchIndexProvider, IBelongsToValidator belongsToValidator)
        {
            _context = context;
            _searchIndexProvider = searchIndexProvider;
            _belongsToValidator = belongsToValidator;
        }

        ///<inheritdoc/>
        public async Task<PagedList<JobModel>> GetAllJobs(PagingQueryStringParameters pagingParameters, string userContextUsersAuth0OrganizationId, JobStatus? jobStatus = null)
        {
            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(userContextUsersAuth0OrganizationId);
            IQueryable<Job> query = _context.Jobs.AsNoTracking().Include(s => s.Facility);

            if (jobStatus == null)
                query = query.Where(s => s.Facility.OrganzationId == organization.OrganizationId);
            else
            {
                var jobStatusId = (int)jobStatus;
                query = query.Where(s => s.Facility.OrganzationId == organization.OrganizationId && s.JobStatusId == jobStatusId);
            }

            var pagedList = await PagingList<Job>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<JobModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        ///<inheritdoc/>
        public async Task<JobModel> GetJob(long jobId, string auth0OrganizationId)
        {
            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var job = await _context.Jobs
                .Include(s => s.JobSkills)
                .Include(y => y.Facility)
                .Where(s => s.Facility.OrganzationId == organization.OrganizationId)
                .FirstAsync(s => s.JobId == jobId);

            List<Skill> skills = new List<Skill>();
            if (job.JobSkills.IsNotNullOrEmpty())
            {
                var skillIds = job.JobSkills.Select(s => s.SkillId).AlwaysList();
                skills = await _context.Skills.Where(s => skillIds.Contains(s.SkillId)).ToListAsync();
            }

            return job.ToDomainModel(skills);
        }

        ///<inheritdoc/>
        public async Task<JobModel> CreateJob(JobModel jobModel, string auth0OrganizationId)
        {
            await _belongsToValidator.ValidateFacilityBelongsToOrganization(jobModel.Facility.FacilityId, auth0OrganizationId);

            var jobToCreate = jobModel.ToNewJob();
            var utcNow = DateTime.UtcNow;

            //New job specific properties
            jobToCreate.JobStatusId = (int)JobStatus.Published;
            jobToCreate.JobsStatusName = JobStatus.Published.ToString();
            jobToCreate.JobPostedDateTimeUtc = jobToCreate.LastJobStatusChangeDateTimeUtc = utcNow;
            jobToCreate.LastJobStatusChangeDateTimeUtc = utcNow;
            jobToCreate.FacilityId = jobModel.Facility.FacilityId;

            //Add the job to be created and insert the job so we get the job id bad
            var jobCreated = await _context.Jobs.AddAsync(jobToCreate);
            await _context.SaveChangesAsync();

            //Create a status change record for this job (with the job id)
            await CreateJobsStatusChangeRecord(jobCreated.Entity.JobId, null, JobStatus.Published, "New job created", utcNow);
            await _context.SaveChangesAsync();

            return jobCreated.Entity.ToDomainModelWithNoFacilityDetails();
        }

        ///<inheritdoc/>
        public async Task RemoveJob(long jobId, string cancellationReason, string auth0OrganizationId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job != null)
            {
                await _belongsToValidator.ValidateFacilityBelongsToOrganization(job.FacilityId, auth0OrganizationId);

                job.JobStatusId = (int)JobStatus.Cancelled;
                job.JobsStatusName = JobStatus.Cancelled.ToString();
                job.LastJobStatusChangeDateTimeUtc = DateTime.UtcNow;

                await CreateJobsStatusChangeRecord(jobId, null, JobStatus.Cancelled, auth0OrganizationId);

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Query that will be run in azure search
        /// {
	    ///     "search": "Auth0OrganizationId:'org_jHHQBIxLVGXxSfLt' AND JobStatusName:Published AND FacilityCity:Auck* AND SkillsIds:/(24|25)/",
	    ///     "filter": "not IsNightShift and JobStartDateTimeUtc gt 2021-10-11T00:00:00.000Z",
	    ///     "queryType": "full",
	    ///     "searchMode": "all",
	    ///     "count": true
        /// }
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="staffAuth0OrganizationId"></param>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        public async Task<PagedList<JobModel>> GetRelevantJobsForStaff(long staffId, string staffAuth0OrganizationId, PagingQueryStringParameters pagingParameters)
        {
            var staffSearchClient = _searchIndexProvider.GetSearchClient(RosterdConstants.Search.StaffIndex);
            var jobsSearchClient = _searchIndexProvider.GetSearchClient(RosterdConstants.Search.JobsIndex);

            //First go and fetch the staff and get the list of skills for that staff
            var staff = (await staffSearchClient.GetDocumentAsync<StaffSearchModel>(staffId.ToString()))?.Value;

            //We cant find the staff, may be the staff member was deleted etc
            if (staff == null)
                return PagedList<JobModel>.EmptyPagedList();

            //Go to the jobs index and get all available jobs that match those skills
            var parameters =
                new SearchOptions
                {
                    SearchMode = SearchMode.All,  QueryType = SearchQueryType.Full,  IncludeTotalCount = true,

                    //The jobs should still be active
                    Filter = $"JobStartDateTimeUtc gt {DateTimeOffset.UtcNow:O}",
                    Size = pagingParameters.PageSize,
                    Skip = (pagingParameters.PageNumber - 1) * pagingParameters.PageSize
                };

            //Build query
            var staffPreferenceCityQueryElement = staff.StaffPreferenceCity.IsNotNullOrEmpty() ? $" AND FacilityCity:{staff.StaffPreferenceCity.GetTheFirstXCharsOrEmpty(4)}* " : string.Empty;
            var staffSkillsElement = staff.SkillsIds.IsNotNullOrEmpty() ? $" AND SkillsIds:/({staff.SkillsIds.AlwaysList().ToDelimitedString("|")})/ " : string.Empty;
            var query = $"Auth0OrganizationId:'{staffAuth0OrganizationId}' AND JobStatusName:{JobStatus.Published} {staffPreferenceCityQueryElement} {staffSkillsElement}";

            //Search for matching jobs, map and return
            var jobSearchResults = await jobsSearchClient.SearchAsync<JobSearchModel>(query, parameters);
            var totalResultsFound = (jobSearchResults.Value.TotalCount ?? 0).ToInt32();
            var totalPages = (int)Math.Ceiling(totalResultsFound / (double)pagingParameters.PageSize);

            return new PagedList<JobModel>(jobSearchResults.Value.ToDomainModels(), totalResultsFound, pagingParameters.PageNumber, pagingParameters.PageSize, totalPages);
        }

        /// <summary>
        /// Query that will be run in azure search
        /// {
        ///     "search": "Auth0OrganizationId:'org_jHHQBIxLVGXxSfLt' AND StaffPreferenceCity:Auck* AND SkillsIds:/(20|26)/",
        ///     "filter": "StaffPreferenceIsNightShiftOk and IsActive",
        ///     "queryType": "full",
        ///     "searchMode": "all",
        ///     "count": true
        /// }
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<(JobSearchModel job, List<string> staffDeviceIds)> GetRelevantStaffDeviceIdsForJob(string jobId)
        {
            var staffSearchClient = _searchIndexProvider.GetSearchClient(RosterdConstants.Search.StaffIndex);
            var jobsSearchClient = _searchIndexProvider.GetSearchClient(RosterdConstants.Search.JobsIndex);

            //First go and fetch the job and get the list of skills for that job
            var job = (await jobsSearchClient.GetDocumentAsync<JobSearchModel>(jobId))?.Value;

            //We cant find the job, may be the job was deleted etc
            if (job == null)
                return (null, new List<string>());

            //Go to the staff index and get all matching staff for the job
            var jobDayFilter = StaffSearchModel.GetDayOfWeekFilter(job.JobStartDateTimeUtc.UtcDateTime.ToNzstFromUtc().DayOfWeek);
            jobDayFilter = jobDayFilter.IsNullOrEmpty() ? string.Empty : $" and {jobDayFilter}";
            var isNightShiftFilter = job.IsNightShift ? $" and {nameof(StaffSearchModel.StaffPreferenceIsNightShiftOk)}" : string.Empty;
            var parameters =
                new SearchOptions
                {
                    SearchMode = SearchMode.All,  QueryType = SearchQueryType.Full,  IncludeTotalCount = true,

                    //The staff should still be active
                    Filter = $"IsActive {isNightShiftFilter} {jobDayFilter}",
                    Size = 1000
                };

            //-------Build query
            var staffPreferenceCityQueryElement = job.FacilityCity.IsNotNullOrEmpty() ? $" AND StaffPreferenceCity:{job.FacilityCity.GetTheFirstXCharsOrEmpty(4)}* " : string.Empty;
            var staffSkillsElement = job.SkillsIds.IsNotNullOrEmpty() ? $" AND SkillsIds:/({job.SkillsIds.AlwaysList().ToDelimitedString("|")})/ " : string.Empty;
            var query = $"Auth0OrganizationId:'{job.Auth0OrganizationId}' {staffPreferenceCityQueryElement} {staffSkillsElement}";

            //Search for matching jobs, map and return
            var staffSearchResults = await staffSearchClient.SearchAsync<StaffSearchModel>(query, parameters);
            var resultPages = staffSearchResults.Value.GetResults().AsPages();

            var staffDeviceIds = new List<string>();
            foreach (var page in resultPages)
            {
                staffDeviceIds.AddRange(page.Values.Select(s => (s.Document.DeviceId ?? string.Empty).Trim()));
            }

            return (job, staffDeviceIds.Distinct().AlwaysList());
        }

        ///<inheritdoc/>
        public async Task<PagedList<JobModel>> GetCurrentJobsForStaff(long staffId, PagingQueryStringParameters pagingParameters)
        {
            var currentJobsForStaffQuery =
                _context.JobStaffs
                        .Include(s => s.Job)
                            .ThenInclude(s => s.Facility)
                        .Where(j => j.StaffId == staffId)
                        .Select(s => s.Job);

            var pagedList = await PagingList<Data.SqlServer.Models.Job>.ToPagingList(currentJobsForStaffQuery, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new Domain.Models.PagedList<JobModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        ///<inheritdoc/>
        public async Task<PagedList<JobModel>> GetJobsForStaff(long staffId, List<JobStatus> jobsStatusesToQueryFor, PagingQueryStringParameters pagingParameters)
        {
            var statusList = jobsStatusesToQueryFor.AlwaysList().Select(s => (long)s).AlwaysList();

            var jobsForStaffQuery =
                from js in _context.JobStaffs.AsNoTracking()
                join job in _context.Jobs.AsNoTracking() on js.JobId equals job.JobId
                where js.StaffId == staffId && statusList.Contains(job.JobStatusId)
                select job;

            var pagedList = await PagingList<Data.SqlServer.Models.Job>.ToPagingList(jobsForStaffQuery, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new Domain.Models.PagedList<JobModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        ///<inheritdoc/>
        public async Task<PagedList<JobModel>> GetJobsForStaff(long staffId, JobStatus jobsStatusToQueryFor, PagingQueryStringParameters pagingParameters)
        {
            if (jobsStatusToQueryFor == JobStatus.Cancelled)
            {
                var cancelledStatus = (long) JobStatus.Cancelled;
                var cancelledJobsForStaff =
                    from js in _context.JobStatusChanges.AsNoTracking()
                    join job in _context.Jobs.AsNoTracking() on js.JobId equals job.JobId
                    where js.JobStatusId == cancelledStatus && js.StaffId == staffId
                    select job;

                var pagedList = await PagingList<Data.SqlServer.Models.Job>.ToPagingList(cancelledJobsForStaff, pagingParameters.PageNumber, pagingParameters.PageSize);

                var domainModels = pagedList.ToDomainModels();
                return new Domain.Models.PagedList<JobModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
            }

            return await GetJobsForStaff(staffId, new List<JobStatus> { jobsStatusToQueryFor }, pagingParameters);
        }

        ///<inheritdoc/>
        public async Task<bool> AcceptJobForStaff(long jobId, long staffId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            var jobStaff = new JobStaff {JobId = jobId, StaffId = staffId};

            //Assign the job to staff
            await _context.JobStaffs.AddAsync(jobStaff);

            //Record history of this status change
            await CreateJobsStatusChangeRecord(jobId, staffId, JobStatus.Accepted, $"Job accepted by staff {staffId}");

            //Change status in the main job table
            job.JobStatusId = JobStatus.Accepted.ToInt32();
            job.JobsStatusName = JobStatus.Accepted.ToString();

            //Do an update to the db (all in one transaction by default)
            await _context.SaveChangesAsync();

            return true;
        }

        ///<inheritdoc/>
        public async Task<bool> CancelJobForStaff(long jobId, long staffId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            var jobStaff = await _context.JobStaffs.FirstOrDefaultAsync(s => s.JobId == jobId && s.StaffId == staffId);

            //Remove any job assignment to staff
            if (jobStaff != null)
                _context.JobStaffs.Remove(jobStaff);

            //Record history of this status change
            await CreateJobsStatusChangeRecord(jobId, staffId, JobStatus.Cancelled, $"Job rejected by staff {staffId}");
            await CreateJobsStatusChangeRecord(jobId, staffId, JobStatus.Published, $"Job reset back to published due to being cancelled by staff {staffId}");

            //Change status in the main job table
            job.JobStatusId = JobStatus.Published.ToInt32();
            job.JobsStatusName = JobStatus.Published.ToString();

            //Do an update to the db (all in one transaction by default)
            await _context.SaveChangesAsync();

            return true;
        }

        ///<inheritdoc/>
        public async Task CreateJobsStatusChangeRecord(long jobId, long? staffId, JobStatus jobStatusChangedTo, string statusChangeReason, DateTime? eventOccurredDateTime = null) =>

            await _context.JobStatusChanges.AddAsync(new JobStatusChange
            {
                JobId = jobId,
                JobStatusId = (int)jobStatusChangedTo,
                StaffId = staffId,
                JobStatusName = jobStatusChangedTo.ToString(),
                JobStatusChangeDateTimeUtc = eventOccurredDateTime ?? DateTime.UtcNow,
                JobStatusChangeReason = statusChangeReason
            });

        ///<inheritdoc/>
        public async Task MovedAllPublishedStatusJobsPastTimeLimitToExpiredState()
        {
            var publishedStatus = (long) JobStatus.Published;
            var currentDateTimeUtc = DateTime.UtcNow;

            var expiredJobs = await _context.Jobs.Where(s => s.JobStatusId == publishedStatus && s.JobStartDateTimeUtc < currentDateTimeUtc).Take(50).ToListAsync();
            while (expiredJobs.IsNotNullOrEmpty())
            {
                foreach (var expiredJob in expiredJobs)
                {
                    expiredJob.JobStatusId = (long)JobStatus.Expired;

                    //Record history of this status change
                    await CreateJobsStatusChangeRecord(expiredJob.JobId, null, JobStatus.Expired, $"AUTO-MOVE: Job expired, still in published stated after end time has past current time");
                }

                await _context.SaveChangesAsync();
                expiredJobs = await _context.Jobs.Where(s => s.JobStatusId == publishedStatus && s.JobStartDateTimeUtc < currentDateTimeUtc).Take(50).ToListAsync();
            }
        }

        ///<inheritdoc/>
        public async Task MoveAllJobsThatArePastEndDateToFeedbackStatus()
        {
            var inProgressStatus = (long) JobStatus.InProgress;
            var currentDateTimeUtc = DateTime.UtcNow;

            var pendingJobs = await _context.Jobs.Where(s => s.JobStatusId == inProgressStatus && s.JobEndDateTimeUtc < currentDateTimeUtc).Take(50).ToListAsync();
            while (pendingJobs.IsNotNullOrEmpty())
            {
                foreach (var pendingJob in pendingJobs)
                {
                    pendingJob.JobStatusId = (long)JobStatus.FeedbackPending;

                    //Record history of this status change
                    await CreateJobsStatusChangeRecord(pendingJob.JobId, null, JobStatus.Expired, $"AUTO-MOVE: Job set to feedback pending");
                }

                await _context.SaveChangesAsync();
                pendingJobs = await _context.Jobs.Where(s => s.JobStatusId == inProgressStatus && s.JobEndDateTimeUtc < currentDateTimeUtc).Take(50).ToListAsync();
            }
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<long>> GetAllJobsThatAreFinished()
        {
            var completedStatus = (int) JobStatus.Completed;
            var noShowStatus = (int)JobStatus.NoShow;
            var expired = (int)JobStatus.Expired;

            var jobsThatAreFinished =
                await _context.Jobs.AsNoTracking().Where(s => s.JobStatusId == completedStatus || s.JobStatusId == noShowStatus || s.JobStatusId == expired)
                    .Select(s => s.JobId)
                    .ToListAsync();

            return jobsThatAreFinished;
        }
    }
}
