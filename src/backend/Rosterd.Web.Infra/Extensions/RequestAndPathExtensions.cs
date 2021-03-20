using Microsoft.AspNetCore.Http;

namespace Rosterd.Web.Infra.Extensions
{
    public static class RequestAndPathExtensions
    {
        public static bool IsSwaggerRoute(this HttpRequest request) => request.Path.HasValue && request.Path.StartsWithSegments("/swagger");

        public static bool IsNotSwaggerRoute(this HttpRequest request) => !request.IsSwaggerRoute();
    }
}
