using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain;
using Rosterd.Domain.Messaging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Messaging;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Services.Mappers;
using Rosterd.Services.Staff.Interfaces;


namespace Rosterd.Services.Staff
{
    public class StaffEventsService : IStaffEventsService
    {
        private readonly IRosterdDbContext _context;
        private readonly ISearchIndexProvider _searchIndexProvider;
        private readonly IQueueClient<StaffQueueClient> _staffQueueClient;

        public StaffEventsService(IRosterdDbContext context, ISearchIndexProvider searchIndexProvider, IQueueClient<StaffQueueClient> staffQueueClient)
        {
            _context = context;
            _searchIndexProvider = searchIndexProvider;
            _staffQueueClient = staffQueueClient;
        }

        ///<inheritdoc/>
        public async Task GenerateStaffCreatedOrUpdatedEvent(long staffId, string auth0OrganizationId)
        {
            var staffCreatedOrUpdatedMessage = new StaffCreatedOrUpdatedMessage(staffId.ToString(), auth0OrganizationId);
            await _staffQueueClient.SendMessageWithNoExpiry(staffCreatedOrUpdatedMessage);
        }

        ///<inheritdoc/>
        public async Task GenerateStaffDeletedEvent(long staffId, string auth0OrganizationId)
        {
            var staffDeletedMessage = new StaffDeletedMessage(staffId.ToString(), auth0OrganizationId);

            //Send to storage queue
            await _staffQueueClient.SendMessageWithNoExpiry(BinaryData.FromObjectAsJson(staffDeletedMessage));
        }

        ///<inheritdoc/>
        public async Task HandleStaffCreatedOrUpdatedEvent(StaffCreatedOrUpdatedMessage staffCreatedOrUpdatedMessage)
        {
            //Get the latest staff info
            var staffId = staffCreatedOrUpdatedMessage.StaffId.ToLong();
            var staff = await _context.Staff
                .Include(s => s.StaffSkills)
                .FirstAsync(s => s.StaffId == staffId);

            //Convert to search model and store in search
            var staffSearchModel = staff.ToSearchModel();
            await _searchIndexProvider.AddOrUpdateDocumentsToIndex(RosterdConstants.Search.StaffIndex, new List<StaffSearchModel> { staffSearchModel });
        }

        ///<inheritdoc/>
        public async Task HandleStaffDeletedEvent(StaffDeletedMessage staffDeletedMessage)
        {
            var staffId = staffDeletedMessage.StaffId;
            await _searchIndexProvider.DeleteDocumentsFromIndex(RosterdConstants.Search.StaffIndex, StaffSearchModel.Key(), new List<string>() { staffId });
        }
    }
}
