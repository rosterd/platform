using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rosterd.Web.Infra.Filters.Swagger
{
    public class LowercaseDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var keys = new List<string>(swaggerDoc.Paths.Keys);
            foreach (var key in keys)
            {
                swaggerDoc.Paths.Add(key.ToLower(), swaggerDoc.Paths[key]);
                swaggerDoc.Paths.Remove(key);
            }
        }
    }
}
