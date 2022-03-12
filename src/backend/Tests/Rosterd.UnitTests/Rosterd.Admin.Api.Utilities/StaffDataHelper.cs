using System.Collections.Generic;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Utilities
{
    public class StaffDataHelper
    {
        public static void ArrangeStaffTestData(IRosterdDbContext context)
        {
            StaffSkillsDataHelper.ArrangeStaffSkillTestData(context);
            var staff1 = new Staff
            {
                StaffId = 1,
                FirstName = "One",
                LastName = "Test",
                JobTitle = "Tester",
                Email = "one.test@gmail.com",
                IsActive = true,
                OrganizationId = 1,
                StaffSkills = new List<StaffSkill> {context.StaffSkills.Find(1L)}
            };
            var staff2 = new Staff
            {
                StaffId = 2,
                FirstName = "Two",
                LastName = "Test",
                JobTitle = "Tester",
                Email = "two.test@gmail.com",
                IsActive = true,
                OrganizationId = 1,
                StaffSkills = new List<StaffSkill> {context.StaffSkills.Find(2L)}
            };
            var staff3 = new Staff
            {
                StaffId = 3,
                FirstName = "Three",
                LastName = "Test",
                JobTitle = "Tester",
                Email = "three.test@gmail.com",
                IsActive = true,
                OrganizationId = 1,
                StaffSkills = new List<StaffSkill> {context.StaffSkills.Find(3L)}
            };
            var staff4 = new Staff
            {
                StaffId = 4,
                FirstName = "Four",
                LastName = "Test",
                JobTitle = "Tester",
                Email = "four.test@gmail.com",
                IsActive = true,
                OrganizationId = 1,
                StaffSkills = new List<StaffSkill> {context.StaffSkills.Find(4L)}
            };
            var staff5 = new Staff
            {
                StaffId = 5,
                FirstName = "Five",
                LastName = "Test",
                JobTitle = "Tester",
                Email = "five.test@gmail.com",
                IsActive = true,
                OrganizationId = 1,
                StaffSkills = new List<StaffSkill> {context.StaffSkills.Find(5L)}
            };
            var staff6 = new Staff
            {
                StaffId = 6,
                FirstName = "Six",
                LastName = "Test",
                JobTitle = "Tester",
                Email = "six.test@gmail.com",
                OrganizationId = 1,
                IsActive = false,
                StaffSkills = new List<StaffSkill> {context.StaffSkills.Find(6L)}
            };

            context.Staff.Add(staff1);
            context.Staff.Add(staff2);
            context.Staff.Add(staff3);
            context.Staff.Add(staff4);
            context.Staff.Add(staff5);
            context.Staff.Add(staff6);

            context.SaveChanges();
        }
    }
}
