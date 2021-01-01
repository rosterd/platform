using Microsoft.AspNetCore.Builder;
using Rosterd.Admin.Api.Infrastructure.Middleware;

namespace Rosterd.Admin.Api.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add the swagger authentication middleware responsible for making sure Swagger UI is not open to public
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerAuthenticationCheck(this IApplicationBuilder builder) => builder.UseMiddleware<SwaggerAuthenticationMiddleware>();
    }
}
