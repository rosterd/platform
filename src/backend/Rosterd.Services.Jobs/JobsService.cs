using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Mappers;

namespace Rosterd.Services.Jobs
{
    public class JobsService : IJobsService
    {
        private readonly IRosterdDbContext _context;

        public JobsService(IRosterdDbContext context) => _context = context;

        public async Task<PagedList<JobModel>> GetAllJobs(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Jobs.Include(s => s.Facility);
            var pagedList = await PagingList<Data.SqlServer.Models.Job>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<JobModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
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
    }
}
