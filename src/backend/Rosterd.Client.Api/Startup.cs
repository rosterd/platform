using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Rosterd.Client.Api.Infrastructure.Extensions;
using Rosterd.Client.Api.Infrastructure.Middleware;
using Rosterd.Domain;
using Rosterd.Web.Infra;
using Rosterd.Web.Infra.Extensions;
using Rosterd.Web.Infra.Middleware;

namespace Rosterd.Client.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        /// <summary>
        ///     Gets the hosting environment.
        /// </summary>
        /// <value>The hosting environment.</value>
        public IWebHostEnvironment HostingEnvironment { get; }

        /// <summary>
        ///     Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (HostingEnvironment.IsDevelopment())
                IdentityModelEventSource.ShowPII = true;

            services
                .AddCustomAuthenticationWithJwtBearer(Configuration) //Add auth and JWT as the first thing (This always needs to be the first thing to configure)
                .AddApplicationInsightsTelemetry()
                .AddAppAndDatabaseDependencies(Configuration, HostingEnvironment)
                .AddCustomSwagger("Rosterd Client Api", "v1")
                .AddApiVersioning(o =>
                {
                    o.ApiVersionReader = new UrlSegmentApiVersionReader();
                    o.AssumeDefaultVersionWhenUnspecified = true;
                })
                .AddCustomCaching()
                .AddCorsWithAllowAll()
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = HostingEnvironment.IsDevelopment();
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                })
                .AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = true;
                    fv.ValidatorOptions.CascadeMode = CascadeMode.Stop;

                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                    fv.RegisterValidatorsFromAssemblyContaining<BaseValidator>();
                });

            //Enable the default 400 Bad Request error handling
            services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = false);

            //Register all custom middleware
            services.AddScoped<SwaggerAuthenticationMiddleware>();
            services.AddScoped<ExceptionHandlerMiddleware>();
            services.AddScoped<UserContextBuilderMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //    app.UseCustomExceptionMiddleware();
            //}

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseCustomExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowAll");

            //Adds authentication middleware to the pipeline so authentication will be performed automatically on each request to host
            //Adds authorization middleware to the pipeline to make sure the Api endpoint cannot be accessed by anonymous clients
            app.UseAuthentication();
            app.UseAuthorization();

            //Custom Middleware
            app.UseMiddleware<UserContextBuilderMiddleware>(); //Sets up the user context with all the details for the logged in user

            //Enable Swagger and SwaggerUI (for swagger we have our own basic auth so its not available to everyone)
            app.UseSwaggerAuthenticationCheck();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rosterd.Client.Api v1"));

            app.UseEndpoints(endpoints => endpoints.MapControllers().RequireAuthorization());
        }
    }
}
