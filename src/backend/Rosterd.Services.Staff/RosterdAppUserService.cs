using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Data.TableStorage.Context;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Domain;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Mappers;
using Rosterd.Services.Staff.Interfaces;


namespace Rosterd.Services.Staff
{
    public class RosterdAppUserService : IRosterdAppUserService
    {
        private readonly IRosterdDbContext _context;
        private readonly IAzureTableStorage _azureTableStorage;
        private readonly IBelongsToValidator _belongsToValidator;
        private readonly ISearchIndexProvider _searchIndexProvider;

        public RosterdAppUserService(IRosterdDbContext context, IAzureTableStorage azureTableStorage, IBelongsToValidator belongsToValidator, ISearchIndexProvider searchIndexProvider)
        {
            _context = context;
            _azureTableStorage = azureTableStorage;
            _belongsToValidator = belongsToValidator;
            _searchIndexProvider = searchIndexProvider;
        }

        public async Task<RosterdAppUser> GetStaffAppUser(string userAuth0Id)
        {
            var rosterdAppUser = await _azureTableStorage.GetAsync<RosterdAppUser>(RosterdAppUser.TableName, RosterdAppUser.UsersPartitionKey, userAuth0Id);
            return rosterdAppUser;
        }

        public async Task CreateOrUpdateStaffAppUser(string userAuth0Id, long staffId, string organizationAuth0Id, long organizationId)
        {
            var staffAppUser = new RosterdAppUser(userAuth0Id)
            {
                OrganizationId = organizationId,
                Auth0OrganizationId = organizationAuth0Id,
                StaffId = staffId
            };

            await _azureTableStorage.AddOrUpdateAsync(RosterdAppUser.TableName, staffAppUser);
        }

        public async Task<StaffAppUserPreferencesModel> GetStaffAppUserPreferences(string userAuth0Id)
        {
            var rosterdAppUser = await _azureTableStorage.GetAsync<RosterdAppUserPreferences>(RosterdAppUserPreferences.TableName, RosterdAppUserPreferences.UsersPartitionKey, userAuth0Id);

            //We don't have the user in our db, so default the preferences which is true for every thing
            return rosterdAppUser == null ? StaffAppUserMapper.ToNew() : rosterdAppUser?.ToDomainModel();
        }

        public async Task UpdateStaffAppUserPreferences(StaffAppUserPreferencesModel staffAppUserPreferencesModel, string userAuth0Id, long staffId)
        {
            var rosterdAppUser = staffAppUserPreferencesModel.ToDataModel(userAuth0Id);

            //Update table storage (where we store the preferences for staff)
            await _azureTableStorage.AddOrUpdateAsync(RosterdAppUserPreferences.TableName, rosterdAppUser);

            //Update search also with the staff preferences
            var staffSearchDocument = await _searchIndexProvider.GetSearchClient(RosterdConstants.Search.StaffIndex).GetDocumentAsync<StaffSearchModel>(staffId.ToString());
            if(staffSearchDocument?.Value == null)
                throw new EntityNotFoundException("You are currently not registered in the Rosterd Platform, please consult you workplace to get your self added."); //Should not happen (can only happen if the index was deleted)

            var staffSearchModelToUpdate = staffSearchDocument.Value;
            staffSearchModelToUpdate.StaffPreferenceCity = staffAppUserPreferencesModel.City;
            staffSearchModelToUpdate.StaffPreferenceIsNightShiftOk = staffSearchModelToUpdate.StaffPreferenceIsNightShiftOk;
            staffSearchModelToUpdate.DeviceId = staffSearchModelToUpdate.DeviceId;

            await _searchIndexProvider.AddOrUpdateDocumentToIndex(RosterdConstants.Search.StaffIndex, staffAppUserPreferencesModel);
        }
    }
}
