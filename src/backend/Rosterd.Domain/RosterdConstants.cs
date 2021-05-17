using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosterd.Domain
{
    public static class RosterdConstants
    {
        /// <summary>
        /// A collection of constants used through out the app
        /// </summary>
        public static class ApplicationKeys
        {
            /// <summary>
            /// Api that needs to be presented for every anonymous call (ie: all API's that don't require authentication)
            /// </summary>
            public const string AnonymousApiKey = "3c752809-4776-4b8a-b7b3-ff8e704793ac";
        }

        /// <summary>
        ///  A collection of all the keys used to cache data
        /// </summary>
        public static class CacheKeys
        {
            /// <summary>
            ///     Key that holds all the tenants for a given auth0id
            /// </summary>
            public static string TenantToAuth0Id(string auth0Id) => $"Tenant_{auth0Id}";

            /// <summary>
            ///     Key that holds all the staff for a given auth0id
            /// </summary>
            public static string StaffToAuth0Id(string auth0Id) => $"Staff_{auth0Id}";

            /// <summary>
            ///     Key that holds all the TenantIdToAuth0Id's
            /// </summary>
            public static string TenantIdToAuth0Id(string auth0Id) => $"TenantId_{auth0Id}";
        }

        public static class Events
        {
            public static string Version1 => "v1";

            public const string StaffCreatedOrUpdatedEvent = "{0}.Rosterd.Staff.CreatedOrUpdated";
            public const string StaffDeletedEvent = "{0}.Rosterd.Staff.Deleted";

            public const string NewJobCreatedEvent = "{0}.Rosterd.Job.Created";
            public const string JobDeletedEvent = "{0}.Rosterd.Job.Deleted";

            public const string NewPushNotificationCreatedEvent = "{0}.Rosterd.NewPushNotification";
        }

        public static class Search
        {
            public static string StaffIndex => "staff-index";
            public static string JobsIndex => "jobs-index";
        }
    }
}
