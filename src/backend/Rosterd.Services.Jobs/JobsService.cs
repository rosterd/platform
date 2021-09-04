using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.Azure.EventGrid;
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
        public async Task<PagedList<JobModel>> GetAllJobs(PagingQueryStringParameters pagingParameters, string auth0OrganizationId)
        {
            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var query = _context.Jobs.Include(s => s.Facility).Where(s => s.Facility.OrganzationId == organization.OrganizationId);
            var pagedList = await PagingList<Job>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<JobModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        ///<inheritdoc/>
        public async Task<JobModel> GetJob(long jobId, string auth0OrganizationId)
        {
            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var job = await _context.Jobs.Include(s => s.Facility)
                .Where(s => s.Facility.OrganzationId == organization.OrganizationId)
                .FirstOrDefaultAsync(s => s.JobId == jobId);

            return job?.ToDomainModel();
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
            await CreateJobsStatusChangeRecord(jobCreated.Entity.JobId, JobStatus.Published, "New job created", utcNow);
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

                await CreateJobsStatusChangeRecord(jobId, JobStatus.Cancelled, auth0OrganizationId);

                await _context.SaveChangesAsync();
            }
        }

        ///<inheritdoc/>
        public async Task<PagedList<JobModel>> GetRelevantJobsForStaff(long staffId, PagingQueryStringParameters pagingParameters)
        {
            //TODO:Org Check

            var staffSearchClient = _searchIndexProvider.GetSearchClient(RosterdConstants.Search.StaffIndex);
            var jobsSearchClient = _searchIndexProvider.GetSearchClient(RosterdConstants.Search.JobsIndex);

            //First go and fetch the staff and get the list of skills for that staff
            var staff = await staffSearchClient.GetDocumentAsync<StaffSearchModel>(staffId.ToString());

            //We cant find the staff, may be the staff member was deleted etc, so return no jobs
            if (staff == null)
                return PagedList<JobModel>.EmptyPagedList();

            //Go to the jobs index and get all available jobs that match those skills
            var parameters =
                new SearchOptions
                {
                    SearchMode = SearchMode.Any,
                    IncludeTotalCount = true,

                    //At least one skill that the staff has must match whats in the job
                    Filter = $"search.in(SkillsSpaceSeperatedString, '{staff.Value.SkillsCsvString}', ',')",
                    Size = pagingParameters.PageSize,
                    Skip = (pagingParameters.PageNumber - 1) * pagingParameters.PageSize
                };

            //Search for matching jobs, map and return
            var jobSearchResults = await jobsSearchClient.SearchAsync<JobSearchModel>("*", parameters);
            var totalResultsFound = (jobSearchResults.Value.TotalCount ?? 0).ToInt32();
            var totalPages = (int)Math.Ceiling(totalResultsFound / (double)pagingParameters.PageSize);

            return new PagedList<JobModel>(jobSearchResults.Value.ToDomainModels(), totalResultsFound, pagingParameters.PageNumber, pagingParameters.PageSize,
                totalPages);
        }

        ///<inheritdoc/>
        public async Task<PagedList<JobModel>> GetCurrentJobsForStaff(long staffId, PagingQueryStringParameters pagingParameters)
        {
            //TODO:Org Check

            var currentJobsForStaffQuery =
                _context.JobStaffs
                        .Include(s => s.Job)
                        .Where(j => j.StaffId == staffId)
                        .Select(s => s.Job);

            var pagedList = await PagingList<Data.SqlServer.Models.Job>.ToPagingList(currentJobsForStaffQuery, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new Domain.Models.PagedList<JobModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        ///<inheritdoc/>
        public async Task<PagedList<JobModel>> GetJobsForStaff(long staffId, List<JobStatus> jobsStatusesToQueryFor, PagingQueryStringParameters pagingParameters)
        {
            //TODO:Org Check

            var statusList = jobsStatusesToQueryFor.AlwaysList().Select(s => (long)s).AlwaysList();

            var completedJobsForStaffQuery =
                from js in _context.JobStaffs
                join job in _context.Jobs on js.JobId equals job.JobId
                where js.StaffId == staffId &&
                      statusList.Contains(job.JobStatusId)
                select job;

            var pagedList = await PagingList<Data.SqlServer.Models.Job>.ToPagingList(completedJobsForStaffQuery, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new Domain.Models.PagedList<JobModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        ///<inheritdoc/>
        public async Task<PagedList<JobModel>> GetJobsForStaff(long staffId, JobStatus jobsStatusToQueryFor, PagingQueryStringParameters pagingParameters) => await GetJobsForStaff(staffId, new List<JobStatus> {jobsStatusToQueryFor}, pagingParameters);

        ///<inheritdoc/>
        public async Task<bool> AcceptJobForStaff(long jobId, long staffId)
        {
            //TODO:Org Check

            var job = await _context.Jobs.FindAsync(jobId);
            var jobStaff = new JobStaff {JobId = jobId, StaffId = staffId};

            //Assign the job to staff
            await _context.JobStaffs.AddAsync(jobStaff);

            //Record history of this status change
            await CreateJobsStatusChangeRecord(jobId, JobStatus.Accepted, $"Job accepted by staff {staffId}");

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
            //TODO:Org Check

            var job = await _context.Jobs.FindAsync(jobId);
            var jobStaff = await _context.JobStaffs.FirstOrDefaultAsync(s => s.JobId == jobId && s.StaffId == staffId);

            //Remove any job assignment to staff
            if (jobStaff != null)
                _context.JobStaffs.Remove(jobStaff);

            //Record history of this status change
            await CreateJobsStatusChangeRecord(jobId, JobStatus.Published, $"Job rejected by staff {staffId}");

            //Change status in the main job table
            job.JobStatusId = JobStatus.Published.ToInt32();
            job.JobsStatusName = JobStatus.Published.ToString();

            //Do an update to the db (all in one transaction by default)
            await _context.SaveChangesAsync();

            return true;
        }

        ///<inheritdoc/>
        public async Task CreateJobsStatusChangeRecord(long jobId, JobStatus jobStatusChangedTo, string statusChangeReason, DateTime? eventOccurredDateTime = null) =>

            //TODO:Org Check
            await _context.JobStatusChanges.AddAsync(new JobStatusChange
            {
                JobId = jobId,
                JobStatusId = (int)jobStatusChangedTo,
                JobStatusName = jobStatusChangedTo.ToString(),
                JobStatusChangeDateTimeUtc = eventOccurredDateTime ?? DateTime.UtcNow,
                JobStatusChangeReason = statusChangeReason
            });

        ///<inheritdoc/>
        public async Task<IEnumerable<long>> GetAllJobsThatAreExpiredButStatusStillNotSetToExpired()
        {
            //TODO:Org Check

            var publishedStatus = (int) JobStatus.Published;
            var currentDateTime = DateTime.UtcNow;

            var jobsThatNeedToBeExpired =
                await _context.Jobs.Where(s => s.JobStatusId == publishedStatus && currentDateTime >= s.JobStartDateTimeUtc)
                    .Select(s => s.JobId)
                    .ToListAsync();

            return jobsThatNeedToBeExpired;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<long>> GetAllJobsThatArePastEndDateButStatusStillNotSetToFeedback()
        {
            //TODO:Org Check

            var inProgressStatus = (int) JobStatus.InProgress;
            var currentDateTime = DateTime.UtcNow;

            var jobsThatAreFinished =
                await _context.Jobs.Where(s => s.JobStatusId == inProgressStatus && currentDateTime >= s.JobEndDateTimeUtc)
                    .Select(s => s.JobId)
                    .ToListAsync();

            return jobsThatAreFinished;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<long>> GetAllJobsThatAreFinished()
        {
            //TODO:Org Check

            var completedStatus = (int) JobStatus.Completed;
            var noShowStatus = (int)JobStatus.NoShow;

            var jobsThatAreFinished =
                await _context.Jobs.Where(s => s.JobStatusId == completedStatus || s.JobStatusId == noShowStatus)
                    .Select(s => s.JobId)
                    .ToListAsync();

            return jobsThatAreFinished;
        }
    }
}
