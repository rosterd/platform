using System.Threading.Tasks;
using Rosterd.Domain.Messaging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Services.Staff.Interfaces
{
    public interface IStaffEventsService
    {
        /// <summary>
        /// Generates a new staff created or updated event and sends the event to event grid
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task GenerateStaffCreatedOrUpdatedEvent(long staffId, string auth0OrganizationId);

        /// <summary>
        /// Generates a new staff deleted event and send the event to the event grid
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task GenerateStaffDeletedEvent(long staffId, string auth0OrganizationId);

        /// <summary>
        /// Handles the new staff created or updated event
        /// </summary>
        /// <param name="staffCreatedOrUpdatedMessage"></param>
        /// <returns></returns>
        Task HandleStaffCreatedOrUpdatedEvent(StaffCreatedOrUpdatedMessage staffCreatedOrUpdatedMessage);

        /// <summary>
        /// Handles the staff deleted event
        /// </summary>
        /// <param name="staffDeletedMessage"></param>
        /// <returns></returns>
        Task HandleStaffDeletedEvent(StaffDeletedMessage staffDeletedMessage);

        Task AddAllActiveStaffToSearch();
    }
}
