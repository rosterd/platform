using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;
using Rosterd.Services.Resources.Interfaces;

namespace Rosterd.Services.Resources
{
    public class JobService: IJobService
    {
        private readonly IRosterdDbContext _context;

        public JobService(IRosterdDbContext context) => _context = context;

        public Task<IActionResult> DeleteJob(long jobId) => throw new System.NotImplementedException();
        public Task<JobModel> GetJobById(long jobId) => throw new System.NotImplementedException();

        public async Task<PagedList<JobModel>> GetJobs(PagingQueryStringParameters pagingParameters) 
        {
            var query = _context.Job;
            var pagedList = await PagingHelper<Job>.ToPagingHelper(query, pagingParameters.PageNumber, pagingParameters.PageSize);


            var domainModelJob = pagedList.Select(s => new JobModel {JobId = s.JobId}).ToList();

            return new PagedList<JobModel>(domainModelJob, domainModelJob.Count, pagedList.CurrentPage, pagedList.PageSize,
                pagedList.TotalPages);
        }

        public Task<IActionResult> PostJob(JobModel jobModel) => throw new System.NotImplementedException();
    }
}