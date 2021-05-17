using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain;
using Rosterd.Domain.Events;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Services.Mappers;
using Rosterd.Services.Staff.Interfaces;


namespace Rosterd.Services.Staff
{
    public class StaffEventsService : IStaffEventsService
    {
        private readonly IRosterdDbContext _context;
        private readonly ISearchIndexProvider _searchIndexProvider;

        public StaffEventsService(IRosterdDbContext context, ISearchIndexProvider searchIndexProvider)
        {
            _context = context;
            _searchIndexProvider = searchIndexProvider;
        }

        ///<inheritdoc/>
        public async Task GenerateStaffCreatedOrUpdatedEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom, long staffId)
        {
            //Get the latest staff info
            var staff = await _context.Staff
                                        .Include(s => s.StaffFacilities)
                                        .Include(s => s.StaffSkills)
                                        .FirstAsync(s => s.StaffId == staffId);

            //Translate to domain model and create event
            var staffSearchModel = staff.ToSearchModel();
            var staffCreatedOrUpdatedEvent = new StaffCreatedOrUpdatedEvent(environmentThisEventIsBeingGenerateFrom, staffSearchModel);

            //Sent the event to event grid
            await eventGridClient.PublishEventsAsync(topicHostName, new List<EventGridEvent> {staffCreatedOrUpdatedEvent});
        }

        ///<inheritdoc/>
        public async Task GenerateStaffDeletedEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom, long staffId)
        {
            var staffDeletedEvent = new StaffDeletedEvent(environmentThisEventIsBeingGenerateFrom, staffId);

            //Sent the event to event grid
            await eventGridClient.PublishEventsAsync(topicHostName, new List<EventGridEvent> {staffDeletedEvent});
        }

        ///<inheritdoc/>
        public async Task HandleStaffCreatedOrUpdatedEvent(EventGridEvent staffCreatedOrUpdatedEvent)
        {
            var staffModel = staffCreatedOrUpdatedEvent.Data as StaffModel;
            await _searchIndexProvider.AddOrUpdateDocumentsToIndex(RosterdConstants.Search.StaffIndex, new List<StaffModel> {staffModel});
        }

        ///<inheritdoc/>
        public async Task HandleStaffDeletedEvent(EventGridEvent staffDeletedEvent)
        {
            var staffId = staffDeletedEvent.Data as string;
            await _searchIndexProvider.DeleteDocumentsFromIndex(RosterdConstants.Search.StaffIndex, StaffSearchModel.Key(), new List<string>() {staffId});
        }
    }
}
