using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Rosterd.Web.Infra.Filters.Swagger;

namespace Rosterd.Web.Infra.ServiceRegistrations
{
    public static class RegisterSwagger
    {
        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = AppContext.BaseDirectory;
                var fileName = Assembly.GetEntryAssembly().GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        public static void RegisterSwaggerDependencies(this IServiceCollection services, string apiTitle, string version) => services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = apiTitle, Version = version });
            options.DescribeAllParametersInCamelCase();
            options.DocInclusionPredicate((name, api) => true);
            options.TagActionsBy(api => new[] {api.GroupName});

            try
            {
                options.IncludeXmlComments(XmlCommentsFilePath);
            }
            catch
            {
                //do nothing if the swagger documentation isn't there carry on
            }

            //Operation Filters
            options.OperationFilter<SwaggerAuthorizeCheckOperationFilter>();

            //Document Filters
            options.DocumentFilter<LowercaseDocumentFilter>();
            options.DocumentFilter<SwaggerCustomControllerOrderingFilter>();

            //Security
            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {new OpenApiSecurityScheme {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}}, new string[] { }}
            });
        });
    }
}
