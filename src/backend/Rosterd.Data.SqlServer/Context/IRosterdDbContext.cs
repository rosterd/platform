using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Rosterd.Data.SqlServer.Models;

namespace Rosterd.Data.SqlServer.Context
{
    public interface IRosterdDbContext
    {
        DbSet<Capability> Capabilities { get; set; }
        DbSet<Facility> Facilities { get; set; }
        DbSet<FacilityCapability> FacilityCapabilities { get; set; }
        DbSet<Job> Jobs { get; set; }
        DbSet<JobSkill> JobSkills { get; set; }
        DbSet<JobStaff> JobStaffs { get; set; }
        DbSet<JobStatusChange> JobStatusChanges { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<Skill> Skills { get; set; }
        DbSet<StaffSkill> StaffSkills { get; set; }
        DbSet<Staff> Staff { get; set; }


        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
