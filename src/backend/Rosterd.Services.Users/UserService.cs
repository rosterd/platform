using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Data.TableStorage;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Domain.Models.Users;
using Rosterd.Services.Mappers;
using Rosterd.Services.Users.Interfaces;

namespace Rosterd.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IAzureTableStorage _azureTableStorage;

        public UserService(IAzureTableStorage azureTableStorage) => _azureTableStorage = azureTableStorage;

        public async Task<UserPreferencesModel> GetUserPreferences(string userEmail)
        {
            var rosterdAppUser = await _azureTableStorage.GetAsync<RosterdAppUser>(RosterdAppUser.TableName, RosterdAppUser.UsersPartitionKey, userEmail);

            //We don't have the user in our db, so default the preferences which is true for every thing
            if (rosterdAppUser == null)
                return UserMapper.ToNew();

            return rosterdAppUser?.ToDomainModel();
        }

        public async Task UpdateUserPreferences(UserPreferencesModel userPreferencesModel)
        {
            var rosterdAppUser = userPreferencesModel.ToDataModel();

            await _azureTableStorage.AddOrUpdateAsync(RosterdAppUser.TableName, rosterdAppUser);
        }
    }
}
