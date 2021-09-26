using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Rosterd.Admin.Api.Infrastructure.ServiceRegistrations
{
    public static class RegisterAuthentication
    {
        public static void RegisterAuthenticationDependencies(this IServiceCollection services, IConfiguration config)
        {
            var domain = $"https://{config["Auth0Settings:Domain"]}/";
            services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    options.Authority = domain;
                    options.Audience = config["Auth0Settings:Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "Roles",
                        RoleClaimType = "https://rosterd.com/roles"
                    };

                    options.Events = new JwtBearerEvents
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

            //In future if want fine grain access, then we need to use scopes like this
            //services.AddAuthorization(options => {
            //    options.AddPolicy("create:facility", policy => policy.Requirements.Add(new HasScopeRequirement("create:facility", domain)));
            //});
        }
    }
}
