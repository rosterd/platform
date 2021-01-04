using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Rosterd.Data.SqlServer.Models;

namespace Rosterd.Data.SqlServer.Context
{
    public interface IRosterdDbContext
    {
        DbSet<Capability> Capability { get; set; }
        DbSet<Facility> Facility { get; set; }
        DbSet<FacilityCapability> FacilityCapability { get; set; }
        DbSet<Organization> Organization { get; set; }
        DbSet<Resource> Resource { get; set; }

        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
