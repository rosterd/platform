using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Domain.Enums;

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

        }

        public static class Messaging
        {
            public static string JobQueueName = "job-queue";
            public static string StaffQueueName = "staff-queue";

            public const string StaffCreatedOrUpdatedMessage = "Staff.CreatedOrUpdated";
            public const string StaffDeletedMessage = "Staff.Deleted";

            public const string NewJobCreatedMessage = "Job.Created";
            public const string JobCancelledMessage = "Job.Cancelled";
            public const string JobStatusChangedMessage = "Job.StatusChanged";

            public const string NewPushNotificationCreatedMessage = "PushNotification.New";
        }

        public static class Search
        {
            public static string StaffIndex => "staff-index";
            public static string JobsIndex => "jobs-index";
        }

        public static class ErrorMessages
        {
            public const string GenericError = "An error occured while performing this operation, please try again at a later time.";
        }

        public static class RosterdRoleNames
        {
            public const string FacilityAdmin = "FacilityAdmin";
            public const string RosterdAdmin = "RosterdAdmin";
            public const string Staff = "Staff";
            public const string OrganizationAdmin = "OrganizationAdmin";
        }

        public static class AccessTokenFields
        {
            public const string Auth0OrganizationId = "org_id";
            public const string Auth0UserId = "sub";
            public const string AccessToken = "access_token";
            public const string Roles = "https://rosterd.com/roles";
        }

        public static class Users
        {
            public const string UserRemovedFromAuth0Text = "UserRemovedFromAuth0Text";
        }

        public static class Email
        {
            public const string AdminFromEmailAddress = "amdin@rosterd.com";
            public const string AdminFromEmailName = "Rosterd";

            public const string AppDownloadWelcomeUrl = "www.rosterd.com/welcome";

            public const string WelcomeEmailSubject = "Welcome To Rosterd";
            public const string WelcomeEmailTemplateId = "d-a18ecbcc7f364341a0d7bf1a5e6fc170";
        }
    }
}
