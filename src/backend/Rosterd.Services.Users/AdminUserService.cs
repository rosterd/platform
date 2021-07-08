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

        public async Task CreateAdminUser(AdminUserModel adminUserModel)
        {
            
        }

        public async Task<AdminUserModel> GetAdminUser(string auth0Id)
        {
            return null;
        }

        public async Task UpdateAdminUser(AdminUserModel adminUserModel)
        {

        }
    }
}
