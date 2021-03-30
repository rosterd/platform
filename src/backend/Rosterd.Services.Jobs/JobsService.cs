using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
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
            var pagedList = await PagingHelper<Data.SqlServer.Models.Job>.ToPagingHelper(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<JobModel>(domainModels, domainModels.Count, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        public async Task<JobModel> GetJob(long jobId)
        {
            var job = await _context.Jobs.Include(s => s.Facility).FirstOrDefaultAsync(s => s.JobId == jobId);
            return job?.ToDomainModel();
        }

        public async Task CreateJob(JobModel joblModel)
        {
            var jobToCreate = joblModel.ToNewJob();

            await _context.Jobs.AddAsync(jobToCreate);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveJob(long jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateJob(JobModel jobModel)
        {
            var jobModelToUpdate = jobModel.ToDataModel();

            _context.Jobs.Update(jobModelToUpdate);
            await _context.SaveChangesAsync();
        }
    }
}
