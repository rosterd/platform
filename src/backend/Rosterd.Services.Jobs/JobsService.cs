using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Mappers;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;

namespace Rosterd.Services.Jobs
{
    public class JobsService : IJobsService
    {
        private readonly IRosterdDbContext _context;
        private readonly ISearchIndexProvider _searchIndexProvider;

        public JobsService(IRosterdDbContext context, ISearchIndexProvider searchIndexProvider)
        {
            _context = context;
            _searchIndexProvider = searchIndexProvider;
        }

        public async Task<Domain.Models.PagedList<JobModel>> GetAllJobs(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Jobs.Include(s => s.Facility);
            var pagedList = await PagingList<Data.SqlServer.Models.Job>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new Domain.Models.PagedList<JobModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        public async Task<JobModel> GetJob(long jobId)
        {
            var job = await _context.Jobs.Include(s => s.Facility).FirstOrDefaultAsync(s => s.JobId == jobId);
            return job?.ToDomainModel();
        }

        public async Task<long> CreateJob(JobModel jobModel)
        {
            var jobToCreate = jobModel.ToNewJob();

            //New job specific properties
            jobToCreate.JobStatusId = (int) JobStatus.Published;
            jobToCreate.JobsStatusName = JobStatus.Published.ToString();
            jobToCreate.JobPostedDateTimeUtc = jobToCreate.LastJobStatusChangeDateTimeUtc = DateTime.UtcNow;

            var jobCreated = await _context.Jobs.AddAsync(jobToCreate);
            await _context.SaveChangesAsync();

            return jobCreated.Entity.JobId;
        }

        public async Task RemoveJob(long jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job != null)
            {
                job.JobStatusId = (int) JobStatus.Cancelled;
                job.JobsStatusName = JobStatus.Cancelled.ToString();
                job.LastJobStatusChangeDateTimeUtc = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<JobModel>> GetRelevantJobsForStaff(long staffId, PagingQueryStringParameters pagingParameters)
        {
            var staffSearchClient = _searchIndexProvider.GetSearchClient(RosterdConstants.Search.StaffIndex);
            var jobsSearchClient = _searchIndexProvider.GetSearchClient(RosterdConstants.Search.JobsIndex);

            //First go and fetch the staff and get the list of skills for that staff
            var staff = await staffSearchClient.GetDocumentAsync<StaffSearchModel>(staffId.ToString());

            //We cant find the staff, may be the staff member was deleted etc, so return no jobs
            if (staff == null)
                return PagedList<JobModel>.EmptyPagedList();

            //Go to the jobs index and get all available jobs that match those skills
            var parameters =
                new SearchOptions()
                {
                    SearchMode = SearchMode.Any,
                    IncludeTotalCount = true,

                    //At least one skill that the staff has must match whats in the job
                    Filter = $"search.in(SkillsSpaceSeperatedString, '{staff.Value.SkillsCsvString}', ',')",
                    Size = pagingParameters.PageSize,
                    Skip = ((pagingParameters.PageNumber - 1)  * pagingParameters.PageSize)
                };

            //Search for matching jobs, map and return
            var jobSearchResults = await jobsSearchClient.SearchAsync<JobSearchModel>("*", parameters);
            var totalResultsFound = (jobSearchResults.Value.TotalCount ?? 0).ToInt32();
            var totalPages = (int)Math.Ceiling(totalResultsFound / (double)pagingParameters.PageSize);
            
            return new PagedList<JobModel>(jobSearchResults.Value.ToDomainModels(), totalResultsFound, pagingParameters.PageNumber, pagingParameters.PageSize, totalPages);
        }

        public async Task<PagedList<JobModel>> GetCurrentJobsForStaff(long staffId, PagingQueryStringParameters pagingParameters)
        {
            var staffSearchClient = _searchIndexProvider.GetSearchClient(RosterdConstants.Search.StaffIndex);
            var jobsSearchClient = _searchIndexProvider.GetSearchClient(RosterdConstants.Search.JobsIndex);

            //First go and fetch the staff and get the list of skills for that staff
            var staff = await staffSearchClient.GetDocumentAsync<StaffSearchModel>(staffId.ToString());

            //We cant find the staff, may be the staff member was deleted etc, so return no jobs
            if (staff == null)
                return PagedList<JobModel>.EmptyPagedList();

            //Go to the jobs index and get all available jobs that match those skills
            var parameters =
                new SearchOptions()
                {
                    SearchMode = SearchMode.Any,
                    IncludeTotalCount = true,

                    //At least one skill that the staff has must match whats in the job
                    Filter = $"search.in(SkillsSpaceSeperatedString, '{staff.Value.SkillsCsvString}', ',')",
                    Size = pagingParameters.PageSize,
                    Skip = ((pagingParameters.PageNumber - 1)  * pagingParameters.PageSize)
                };

            //Search for matching jobs, map and return
            var jobSearchResults = await jobsSearchClient.SearchAsync<JobSearchModel>("*", parameters);
            var totalResultsFound = (jobSearchResults.Value.TotalCount ?? 0).ToInt32();
            var totalPages = (int)Math.Ceiling(totalResultsFound / (double)pagingParameters.PageSize);
            
            return new PagedList<JobModel>(jobSearchResults.Value.ToDomainModels(), totalResultsFound, pagingParameters.PageNumber, pagingParameters.PageSize, totalPages);
        }
    }
}
