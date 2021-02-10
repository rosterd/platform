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
        DbSet<Job> Job { get; set; }
        DbSet<JobStatus> JobStatus { get; set; }
        DbSet<JobStatusChange> JobStatusChange { get; set; }
        DbSet<JobSkill> JobSkill { get; set; }
        DbSet<Organization> Organization { get; set; }
        DbSet<Skill> Skill { get; set; }
        DbSet<Staff> Staff { get; set; }
        DbSet<StaffFacility> StaffFacility { get; set; }
        DbSet<StaffSkill> StaffSkill { get; set; }
        DbSet<Tenant> Tenant { get; set; }


        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
