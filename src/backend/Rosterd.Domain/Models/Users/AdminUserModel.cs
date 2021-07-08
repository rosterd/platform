using System;
using System.Collections.Generic;
using System.Text;

namespace Rosterd.Domain.Models.Users
{
    public class AdminUserModel
    {
        public string Auth0Id { get; set; }

        public List<long> FacilityIds { get; set; }

        public long OrganizationId { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }

        public DateTime LastUpdatedDateTimeUtc { get; set; }
    }
}
