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

        public RosterdAppUser(){}

        public RosterdAppUser(string userAuth0Id)
        {
            PartitionKey = UsersPartitionKey;
            RowKey = userAuth0Id;
            Auth0Id = userAuth0Id; //this just makes things a wee bit easier to read and understand in the tools etc.
        }

        public string GetTableName() => TableName;

        public string DeviceId { get; set; }

        public string Auth0Id { get; set; }

        public string IdmUserName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public bool MondayAvailable { get; set; }

        public bool TuesdayAvailable { get; set; }

        public bool WednesdayAvailable { get; set; }

        public bool ThursdayAvailable { get; set; }

        public bool FridayAvailable { get; set; }

        public bool SaturdayAvailable { get; set; }

        public bool SundayAvailable { get; set; }

        public bool DayShiftOk { get; set; }

        public bool NightShiftOk { get; set; }

        public bool TurnAllNotificationsOff { get; set; }
    }
}
