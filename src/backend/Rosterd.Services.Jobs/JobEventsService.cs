using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Domain.Events;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Mappers;

namespace Rosterd.Services.Jobs
{
    public class JobEventsService : IJobEventsService
    {
        private readonly IRosterdDbContext _context;

        public JobEventsService(IRosterdDbContext context) => _context = context;

        ///<inheritdoc/>
        public async Task GenerateNewJobCreatedEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom, long jobId)
        {
            //Get the latest job info
            var job = await _context.Jobs.Include(s => s.JobSkills).FirstAsync(s => s.JobId == jobId);

            //Translate to domain model and create event
            var jobModel = job.ToDomainModel();
            var jobCreatedEvent = new NewJobCreatedEvent(environmentThisEventIsBeingGenerateFrom, jobModel);

            //Sent the event to event grid
            await eventGridClient.PublishEventsAsync(topicHostName, new List<EventGridEvent> { jobCreatedEvent });
        }

        ///<inheritdoc/>
        public async Task GenerateJobCancelledEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom, long jobId)
        {
            var jobCancelledEvent = new JobCancelledEvent(environmentThisEventIsBeingGenerateFrom, jobId);

            //Sent the event to event grid
            await eventGridClient.PublishEventsAsync(topicHostName, new List<EventGridEvent> { jobCancelledEvent });
        }
    }
}
