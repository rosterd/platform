using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rosterd.Admin.Api.Infrastructure.Filters.Swagger
{
    public class SwaggerCustomControllerOrderingFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.OrderBy(e => e.Key).ToList();
            swaggerDoc.Paths.Clear();
            paths.ForEach(x =>
            {
                var (key, value) = x;
                swaggerDoc.Paths.Add(key, value);
            });
        }
    }
}
