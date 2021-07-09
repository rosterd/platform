using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Rosterd.Data.TableStorage.Models
{
    public class RosterdAdminUser : TableEntity
    {
        public const string AdminUsersPartitionKey = "auth0Id";
        public const string TableName = "RosterdAdminUserTable";

        public RosterdAdminUser(){}

        public RosterdAdminUser(string auth0Id)
        {
            PartitionKey = AdminUsersPartitionKey;
            RowKey = auth0Id;
        }

        public string GetTableName() => TableName;

        public string Auth0Id { get; set; }

        public string FacilityIdsCsvString { get; set; }

        public long OrganizationId { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }

        public DateTime LastUpdatedDateTimeUtc { get; set; }
    }
}
