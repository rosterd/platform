using Flurl;
using Flurl.Http;


namespace Rosterd.ComponentTests.Helpers
{
    public class ApiHelper
    {


        public IFlurlRequest GetApiRequest(string endpoint)
        {
            return ApiConstants.ADMIN_BASE_URL.AppendPathSegment(endpoint)
                .WithHeader("Content-Type", "application/json");
        }
    }
}
