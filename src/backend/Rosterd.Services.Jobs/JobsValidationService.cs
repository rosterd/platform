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
    public class JobsValidationService : IJobsValidationService
    {
        private readonly IRosterdDbContext _context;
        private readonly ISearchIndexProvider _searchIndexProvider;
        private readonly IBelongsToValidator _belongsToValidator;

        public JobsValidationService(IRosterdDbContext context, ISearchIndexProvider searchIndexProvider, IBelongsToValidator belongsToValidator)
        {
            _context = context;
            _searchIndexProvider = searchIndexProvider;
            _belongsToValidator = belongsToValidator;
        }

        public async Task<(bool isJobValid, IEnumerable<string> errorMessages)> IsJobStillValidToAccept(long jobId)
        {
            var errorMessages = new List<string>();
            var job = await _context.Jobs.FindAsync(jobId);
            var staffJobs = await _context.JobStaffs.AsNoTracking().FirstOrDefaultAsync(s => s.JobId == jobId);

            //1. Check if the job is still time valid
            if(job.JobStartDateTimeUtc < DateTime.UtcNow)
                errorMessages.Add("Job is already scheduled to start, can not be accepted");

            //2. check if it hasn't been accepted by another person already
            if(staffJobs != null)
                errorMessages.Add("This job has already been accepted");

            return (errorMessages.IsNullOrEmpty(), errorMessages);
        }

        public async Task<(bool isJobValid, IEnumerable<string> errorMessages)> IsJobStillValidToCancelForStaff(long jobId, long staff)
        {
            var errorMessages = new List<string>();
            var job = await _context.Jobs.FindAsync(jobId);

            //1. Validate grace period
            if(job.NoGracePeriod is true)
                errorMessages.Add("This job can not be cancelled once it has been accepted.");

            if (job.GracePeriodToCancelMinutes != null && job.JobStartDateTimeUtc.AddMinutes(-job.GracePeriodToCancelMinutes.Value) < DateTime.UtcNow)
                errorMessages.Add("You have past the grace time to cancel this job.");

            return (errorMessages.IsNullOrEmpty(), errorMessages);
        }
    }
}
