using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rosterd.Web.Infra.Extensions;

namespace Rosterd.Web.Infra.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerAuthenticationMiddleware : IMiddleware
    {
        /// <summary>
        ///     The user name that needs to be presented to see the swagger ui
        /// </summary>
        public const string SwaggerAccessUserName = "testuser";

        /// <summary>
        ///     The password that needs to be presented to see the swagger ui
        /// </summary>
        public const string SwaggerAccessPassword = "e562322c-d9fe-486d-8537-5975c3d84ebb";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //If we hit the swagger locally (in development) then don't worry about doing auth
            if (context.Request.IsSwaggerRoute() && !IsLocalRequest(context))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the encoded username and password
                    var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                    // Decode from Base64 to string
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    // Split username and password
                    var username = decodedUsernamePassword.Split(':', 2)[0];
                    var password = decodedUsernamePassword.Split(':', 2)[1];

                    // Check if login is correct
                    if (IsAuthorized(username, password))
                    {
                        await next.Invoke(context);
                        return;
                    }
                }

                // Return authentication type (causes browser to show login dialog)
                context.Response.Headers["WWW-Authenticate"] = "Basic";

                // Return unauthorized
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await next.Invoke(context);
            }
        }

        private bool IsAuthorized(string username, string password) => SwaggerAccessUserName == username && SwaggerAccessPassword == password;

        private bool IsLocalRequest(HttpContext context)
        {
            //Handle running using the Microsoft.AspNetCore.TestHost and the site being run entirely locally in memory without an actual TCP/IP connection
            if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
                return true;

            if (context.Connection.RemoteIpAddress != null && context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
                return true;

            if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
                return true;

            return context.Request.Host.Value.StartsWith("localhost:");
        }
    }
}
