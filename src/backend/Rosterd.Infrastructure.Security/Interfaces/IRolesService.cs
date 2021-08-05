using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.Roles;

namespace Rosterd.Infrastructure.Security.Interfaces
{
    public interface IRolesService
    {
        Task<List<RosterdRole>> GetAllRoles();

        Task<RosterdRole> GetRole(string roleId);

        Task<RosterdRole> GetRole(RosterdRoleEnum rosterdRoleEnum);

        Task<RosterdRole> AddRole(RosterdRole roleToAdd);

        Task DeleteRole(string roleId);
    }
}
