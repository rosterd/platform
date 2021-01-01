using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rosterd.Admin.Api.Infrastructure.ServiceRegistrations
{
    public static class RegisterAuthentication
    {
        public static void RegisterAuthenticationDependencies(this IServiceCollection services, IConfiguration config) =>
            services.AddAuthentication(authenticationOptions =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.Authority = config["Auth0:Authority"];
                jwtBearerOptions.Audience = config["Auth0:Audience"];

                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        // Grab the raw value of the token, and store it as a claim so we can retrieve it again later in the request pipeline
                        if (context.SecurityToken is JwtSecurityToken token && context.Principal.Identity is ClaimsIdentity identity)
                            identity.AddClaim(new Claim("access_token", token.RawData));

                        return Task.CompletedTask;
                    }
                };
            });
    }
}
