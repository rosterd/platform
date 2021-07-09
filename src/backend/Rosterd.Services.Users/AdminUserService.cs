using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Data.TableStorage;
using Rosterd.Data.TableStorage.Context;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Domain.Models.Users;
using Rosterd.Services.Mappers;
using Rosterd.Services.Users.Interfaces;

namespace Rosterd.Services.Users
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAzureTableStorage _azureTableStorage;

        public AdminUserService(IAzureTableStorage azureTableStorage) => _azureTableStorage = azureTableStorage;

        ///<inheritdoc/>
        public async Task CreateAdminUser(AdminUserModel adminUserModel)
        {
            var dataModel = adminUserModel.ToDataModel();
            dataModel.CreatedDateTimeUtc = DateTime.UtcNow;
            dataModel.LastUpdatedDateTimeUtc = DateTime.UtcNow;

            await _azureTableStorage.AddOrUpdateAsync(RosterdAdminUser.TableName, dataModel);
        }

        ///<inheritdoc/>
        public async Task<AdminUserModel> GetAdminUser(string auth0Id)
        {
            var rosterdAppUser = await _azureTableStorage.GetAsync<RosterdAdminUser>(RosterdAppUser.TableName, RosterdAppUser.UsersPartitionKey, auth0Id);
            return rosterdAppUser.ToDomainModel();
        }

        ///<inheritdoc/>
        public async Task UpdateAdminUser(AdminUserModel adminUserModel)
        {
            var dataModel = adminUserModel.ToDataModel();
            dataModel.LastUpdatedDateTimeUtc = DateTime.UtcNow;

            await _azureTableStorage.AddOrUpdateAsync(RosterdAdminUser.TableName, dataModel);
        }
    }
}
