using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Admin.Api.Requests.Staff
{
    public class AddUpdateStaffRequest
    {
        public StaffModel StaffToAddOrUpdate { get; set; }
    }
}
