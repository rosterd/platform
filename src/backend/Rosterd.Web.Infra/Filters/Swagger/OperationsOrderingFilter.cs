using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rosterd.Web.Infra.Filters.Swagger
{
    public class OperationsOrderingFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
        {
            //var paths = new Dictionary<KeyValuePair<string, OpenApiPathItem>, int>();
            //foreach (var path in openApiDoc.Paths)
            //{
            //    if (!(context.ApiDescriptions.FirstOrDefault(x => x.RelativePath.Replace("/", string.Empty)
            //            .Equals(path.Key.Replace("/", string.Empty), StringComparison.OrdinalIgnoreCase))?
            //        .ActionDescriptor?.EndpointMetadata?.FirstOrDefault(x => x is OperationOrderAttribute) is OperationOrderAttribute orderAttribute))
            //    {
            //        continue;
            //    }

            //    var order = orderAttribute.Order;
            //    paths.Add(path, order);
            //}

            //if (paths.Count == 0)
            //    return;

            //var orderedPaths = paths.OrderBy(x => x.Value).ToList();
            //openApiDoc.Paths.Clear();
            //orderedPaths.ForEach(x => openApiDoc.Paths.Add(x.Key.Key, x.Key.Value));
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OperationOrderAttribute : Attribute
    {
        public int Order { get; }

        public OperationOrderAttribute(int order) => Order = order;
    }
}
