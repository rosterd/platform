
namespace Rosterd.Client.Api.IntegrationTests.Helpers
{
    public static class ApiConstants
    {
        //admin apis
        public static string JOBS_ENDPOINT = "/api/v1/jobs";

        //client apis
        public static string MY_CURRENT_JOBS = "/api/v1/jobs/my/current";
        public static string MY_CANCELLED_JOBS = "/api/v1/jobs/my/history/cancelled";
        public static string MY_COMPLETED_JOBS = "/api/v1/jobs/my/history/cancelled";
        public static string CANCEL_MY_ACCEPTED_JOBS = "/api/v1/Jobs/{0}/cancellations";
        public static string ACCEPT_MY_CURRENT_JOBS = "/api/v1/Jobs/{0}/confirmation";
        public static string MY_RELEVANT_JOBS = "/api/v1/jobs/my/relevant";
        public static string MY_PREFERENCES = "/api/v1/Preferences/my";

    }
}
