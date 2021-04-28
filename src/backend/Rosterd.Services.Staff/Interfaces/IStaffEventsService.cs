using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Services.Staff.Interfaces
{
    public interface IStaffEventsService
    {
        /// <summary>
        /// Generates a new staff created or updated event and sends the event to event grid
        /// </summary>
        /// <param name="eventGridClient"></param>
        /// <param name="topicHostName"></param>
        /// <param name="environmentThisEventIsBeingGenerateFrom"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task GenerateStaffCreatedOrUpdatedEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom, long staffId);

        /// <summary>
        /// Generates a new staff deleted event and send the event to the event grid
        /// </summary>
        /// <param name="eventGridClient"></param>
        /// <param name="topicHostName"></param>
        /// <param name="environmentThisEventIsBeingGenerateFrom"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task GenerateStaffDeletedEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom, long staffId);
    }
}
