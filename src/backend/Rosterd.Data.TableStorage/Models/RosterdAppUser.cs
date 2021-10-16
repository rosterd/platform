using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Rosterd.Data.TableStorage.Models
{
    public class RosterdAppUser : TableEntity
    {
        public const string UsersPartitionKey = "userAuth0Id";
        public const string TableName = "RosterdAppUserTable";

        public RosterdAppUser() { }

        public RosterdAppUser(string userAuth0Id)
        {
            PartitionKey = UsersPartitionKey;
            RowKey = userAuth0Id;
            Auth0Id = userAuth0Id;
        }

        /// <summary>
        /// The rosterd staffIf for this app user (The primary key for the staff table)
        /// </summary>
        public long StaffId { get; set; }

        /// <summary>
        /// The rosterd organization id for this appuser (The Rosterd organization this staff belongs too)
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// The rosterd auth0 organization id for this appuser (The Rosterd organization's Auth0Id this staff belongs too)
        /// </summary>
        public string Auth0OrganizationId { get; set; }

        public string GetTableName() => TableName;

        public string Auth0Id { get; set; }
    }
}
