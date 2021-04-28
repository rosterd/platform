using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;

namespace Rosterd.Services.Jobs.Interfaces
{
    public interface IJobEventsService
    {
        /// <summary>
        /// Generates a new job created event and sends the event to event grid
        /// </summary>
        /// <param name="eventGridClient"></param>
        /// <param name="topicHostName"></param>
        /// <param name="environmentThisEventIsBeingGenerateFrom"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task GenerateNewJobCreatedEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom, long jobId);

        /// <summary>
        /// Generates a new job cancelled event and send the event to the event grid
        /// </summary>
        /// <param name="eventGridClient"></param>
        /// <param name="topicHostName"></param>
        /// <param name="environmentThisEventIsBeingGenerateFrom"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task GenerateJobCancelledEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom, long jobId);
    }
}
