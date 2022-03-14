using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models.JobModels;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Utilities
{
    public class JobStatusDataHelper
    {
        public static void ArrangeJobStatusTestData(IRosterdDbContext context)
        {
            var jobStatus1 = new JobStatusChange {JobStatusId = 1, JobStatusName = "Published"};
            var jobStatus2 = new JobStatusChange {JobStatusId = 2, JobStatusName = "Accepted"};
            var jobStatus3 = new JobStatusChange {JobStatusId = 3, JobStatusName = "In Progress"};
            var jobStatus4 = new JobStatusChange {JobStatusId = 4, JobStatusName = "Feedback Pending"};
            var jobStatus5 = new JobStatusChange {JobStatusId = 5, JobStatusName = "No Show"};
            var jobStatus6 = new JobStatusChange {JobStatusId = 6, JobStatusName = "Completed"};
            var jobStatus7 = new JobStatusChange {JobStatusId = 7, JobStatusName = "Expired"};

            context.JobStatusChanges.Add(jobStatus1);
            context.JobStatusChanges.Add(jobStatus2);
            context.JobStatusChanges.Add(jobStatus3);
            context.JobStatusChanges.Add(jobStatus4);
            context.JobStatusChanges.Add(jobStatus5);
            context.JobStatusChanges.Add(jobStatus6);
            context.JobStatusChanges.Add(jobStatus7);
            context.SaveChanges();
        }
    }
}
