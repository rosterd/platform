using System;
using System.Collections.Generic;
using System.Linq;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Utilities
{
    public class JobsDataHelper
    {
        public static void ArrangeJobsTestData(IRosterdDbContext context)
        {
            FacilitiesDataHelper.ArrangeFacilitiesTestData(context);
            JobStatusDataHelper.ArrangeJobStatusTestData(context);
            JobSkillsDataHelper.ArrangeJobSkillTestData(context);


            // published job
            var job1 = new Job
            {
                JobId = 1,
                JobTitle = "Level 1 HCA",
                Description = "Level 1 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = DateTime.Now.AddDays(2),
                JobEndDateTimeUtc = DateTime.Now.AddDays(2).AddHours(8),
                JobPostedDateTimeUtc = DateTime.Now.AddDays(-5),
                Facility = context.Facilities.Find( 1L),
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                JobStatusId = context.JobStatusChanges.Find(1L).JobStatusId,
                JobsStatusName = context.JobStatusChanges.Find(1L).JobStatusName,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = context.JobSkills.ToArray()
            };

            // accepted job
            var job2 = new Job
            {
                JobId = 2,
                JobTitle = "Level 2 HCA",
                Description = "Level 2 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = DateTime.Now.AddDays(2),
                JobEndDateTimeUtc = DateTime.Now.AddDays(2).AddHours(8),
                JobPostedDateTimeUtc = DateTime.Now.AddDays(-5),
                Facility = context.Facilities.Find( 1L),
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                JobStatusId = context.JobStatusChanges.Find(2L).JobStatusId,
                JobsStatusName = context.JobStatusChanges.Find(2L).JobStatusName,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = context.JobSkills.ToArray()
            };

            var job3 = new Job
            {
                JobId = 3,
                JobTitle = "Level 3 HCA",
                Description = "Level 3 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = DateTime.Now.AddDays(2),
                JobEndDateTimeUtc = DateTime.Now.AddDays(2).AddHours(8),
                JobPostedDateTimeUtc = DateTime.Now.AddDays(-5),
                Facility = context.Facilities.Find( 1L),
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                JobStatusId = context.JobStatusChanges.Find(3L).JobStatusId,
                JobsStatusName = context.JobStatusChanges.Find(3L).JobStatusName,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = context.JobSkills.ToArray()
            };

            var job4 = new Job
            {
                JobId = 4,
                JobTitle = "Level 4 HCA",
                Description = "Level 4 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = DateTime.Now.AddDays(2),
                JobEndDateTimeUtc = DateTime.Now.AddDays(2).AddHours(8),
                JobPostedDateTimeUtc = DateTime.Now.AddDays(-5),
                Facility = context.Facilities.Find( 1L),
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                JobStatusId = context.JobStatusChanges.Find(4L).JobStatusId,
                JobsStatusName = context.JobStatusChanges.Find(4L).JobStatusName,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = context.JobSkills.ToArray()
            };

            var job5 = new Job
            {
                JobId = 5,
                JobTitle = "Level 5 HCA",
                Description = "Level 5 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = DateTime.Now.AddDays(2),
                JobEndDateTimeUtc = DateTime.Now.AddDays(2).AddHours(8),
                JobPostedDateTimeUtc = DateTime.Now.AddDays(-5),
                Facility = context.Facilities.Find( 1L),
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                JobStatusId = context.JobStatusChanges.Find(5L).JobStatusId,
                JobsStatusName = context.JobStatusChanges.Find(5L).JobStatusName,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = context.JobSkills.ToArray()
            };

            var job6 = new Job
            {
                JobId = 6,
                JobTitle = "Level 5 HCA",
                Description = "Level 5 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = DateTime.Now.AddDays(2),
                JobEndDateTimeUtc = DateTime.Now.AddDays(2).AddHours(8),
                JobPostedDateTimeUtc = DateTime.Now.AddDays(-5),
                Facility = context.Facilities.Find( 1L),
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                JobStatusId = context.JobStatusChanges.Find(6L).JobStatusId,
                JobsStatusName = context.JobStatusChanges.Find(6L).JobStatusName,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = context.JobSkills.ToArray()
            };

            var job7 = new Job
            {
                JobId = 7,
                JobTitle = "Level 5 HCA",
                Description = "Level 5 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = DateTime.Now.AddDays(2),
                JobEndDateTimeUtc = DateTime.Now.AddDays(2).AddHours(8),
                JobPostedDateTimeUtc = DateTime.Now.AddDays(-5),
                Facility = context.Facilities.Find( 1L),
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                JobStatusId = context.JobStatusChanges.Find(7L).JobStatusId,
                JobsStatusName = context.JobStatusChanges.Find(7L).JobStatusName,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = context.JobSkills.ToArray()
            };

            //accepted past start time and before endtime job
            var job8 = new Job
            {
                JobId = 8,
                JobTitle = "Level 8 HCA",
                Description = "Level 8 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = DateTime.Now.AddMinutes(-5),
                JobEndDateTimeUtc = DateTime.Now.AddHours(7).AddMinutes(55),
                JobPostedDateTimeUtc = DateTime.Now.AddDays(-5),
                Facility = context.Facilities.Find( 1L),
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                JobStatusId = context.JobStatusChanges.Find(2L).JobStatusId,
                JobsStatusName = context.JobStatusChanges.Find(2L).JobStatusName,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = context.JobSkills.ToArray()
            };

            // in progress past start time job
            var job9 = new Job
            {
                JobId = 9,
                JobTitle = "Level 9 HCA",
                Description = "Level 9 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = DateTime.Now.AddMinutes(-8).AddMinutes(-5),
                JobEndDateTimeUtc = DateTime.Now.AddMinutes(-5),
                JobPostedDateTimeUtc = DateTime.Now.AddDays(-5),
                Facility = context.Facilities.Find( 1L),
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                JobStatusId = context.JobStatusChanges.Find(3L).JobStatusId,
                JobsStatusName = context.JobStatusChanges.Find(3L).JobStatusName,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = context.JobSkills.ToArray()
            };

            // published not accepted
            var job10 = new Job
            {
                JobId = 10,
                JobTitle = "Level 10 HCA",
                Description = "Level 10 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = DateTime.Now.AddHours(-12),
                JobEndDateTimeUtc = DateTime.Now.AddHours(-4),
                JobPostedDateTimeUtc = DateTime.Now.AddDays(-5),
                Facility = context.Facilities.Find( 1L),
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                JobStatusId = context.JobStatusChanges.Find(2L).JobStatusId,
                JobsStatusName = context.JobStatusChanges.Find(2L).JobStatusName,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = context.JobSkills.ToArray()
            };

            context.Jobs.Add(job1);
            // context.Jobs.Add(job2);
            // context.Jobs.Add(job3);
            // context.Jobs.Add(job4);
            // context.Jobs.Add(job5);
            // context.Jobs.Add(job6);
            // context.Jobs.Add(job7);
            // context.Jobs.Add(job8);
            // context.Jobs.Add(job9);
            // context.Jobs.Add(job10);
            context.SaveChanges();
        }
    }
}
