using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Domain.Models.Users;

namespace Rosterd.Services.Users.Interfaces
{
    public interface IUserService
    {
        public Task<UserPreferencesModel> GetUserPreferences(string userEmail);

        public Task UpdateUserPreferences(UserPreferencesModel userPreferencesModel);
    }
}
