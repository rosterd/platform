using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Data.TableStorage;
using Rosterd.Data.TableStorage.Context;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Services.Mappers;
using Rosterd.Services.Users.Interfaces;

namespace Rosterd.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IAzureTableStorage _azureTableStorage;

        public UserService(IAzureTableStorage azureTableStorage) => _azureTableStorage = azureTableStorage;
    }
}
