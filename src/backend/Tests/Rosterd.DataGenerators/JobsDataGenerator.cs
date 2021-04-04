using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Domain.Enums;
using Xunit;

namespace Rosterd.DataGenerators
{
    public class JobsDataGenerator
    {
        [Fact]
        public async Task CleanExistingJobsDataAndBringItToLatest()
        {
            var options = new DbContextOptionsBuilder<RosterdDbContext>().UseSqlServer(connectionString: "Server=tcp:sqls-rosterd-prod.database.windows.net,1433;Initial Catalog=rosterd;Persist Security Info=False;User ID=rosterd-admin-user;Password=K66pth1sS@f5;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            var context = new RosterdDbContext(options.Options);

            var count = 0;
            var jobs = await context.Jobs.ToListAsync();

            foreach (var job in jobs)
            {
                if(job.JobStatusId != (int) JobStatus.Published)
                    continue;

                count++;

                if(job.JobStartDateTimeUtc <= DateTime.UtcNow)
                    job.JobStartDateTimeUtc = DateTime.UtcNow.AddHours(RandomNumber(1, 10));

                job.JobEndDateTimeUtc = job.JobStartDateTimeUtc.AddHours(RandomNumber(1, 24));
                job.JobPostedDateTimeUtc = job.JobStartDateTimeUtc.AddHours(-RandomNumber(1, 10));
                job.GracePeriodToCancelMinutes = RandomNumber(0, 100);
                job.NoGracePeriod = job.GracePeriodToCancelMinutes == 0 ? true : false;
                job.JobStatusId = (int)JobStatus.Published;
                job.JobsStatusName = JobStatus.Published.ToString();

                if (count == 100)
                {
                    await context.SaveChangesAsync();
                    count = 0;
                }
            }

            await context.SaveChangesAsync();
        }

        private static int RandomNumber(int min, int max)
        {
            Random random = new Random(); return random.Next(min, max);

        }
    }
}
