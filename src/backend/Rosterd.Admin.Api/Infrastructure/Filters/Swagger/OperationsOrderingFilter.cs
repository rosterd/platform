using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rosterd.Admin.Api.Infrastructure.Filters.Swagger
{
    public class OperationsOrderingFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
        {
            Dictionary<KeyValuePair<string, OpenApiPathItem>, int> paths = new Dictionary<KeyValuePair<string, OpenApiPathItem>, int>();
            foreach (var path in openApiDoc.Paths)
            {
                var orderAttribute = context.ApiDescriptions.FirstOrDefault(x => x.RelativePath.Replace("/", string.Empty)
                        .Equals(path.Key.Replace("/", string.Empty), StringComparison.InvariantCultureIgnoreCase))?
                    .ActionDescriptor?.EndpointMetadata?.FirstOrDefault(x => x is OperationOrderAttribute) as OperationOrderAttribute;

                if (orderAttribute == null)
                    throw new ArgumentNullException("there is no order for operation " + path.Key);

                var order = orderAttribute.Order;
                paths.Add(path, order);
            }

            var orderedPaths = paths.OrderBy(x => x.Value).ToList();
            openApiDoc.Paths.Clear();
            orderedPaths.ForEach(x => openApiDoc.Paths.Add(x.Key.Key, x.Key.Value));
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OperationOrderAttribute : Attribute
    {
        public int Order { get; }

        public OperationOrderAttribute(int order) => Order = order;
    }
}
