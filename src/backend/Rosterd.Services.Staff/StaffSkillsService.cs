using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Services.Staff.Interfaces;
using Rosterd.Services.Staff.Mappers;

namespace Rosterd.Services.Staff
{
    public class StaffSkillsService : IStaffSkillsService
    {
        private readonly IRosterdDbContext _context;

        public StaffSkillsService(IRosterdDbContext context) => _context = context;
    }
}
