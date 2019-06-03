using DetegoServer.Helpers.Attributes;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DetegoServer.Helpers
{
    public class AuthFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }
            context.ApiDescription.TryGetMethodInfo(out var mi);

            var authorizeAttributes = context.ApiDescription
            .ControllerAttributes()
            .Union(context.ApiDescription.ActionAttributes())
            .OfType<RequireTokenAttribute>();
            var allowAnonymousAttributes = mi.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>();

            if (!authorizeAttributes.Any() || authorizeAttributes.Any() && allowAnonymousAttributes.Any())
            {
                return;
            }

            if (operation.Security == null)
            {
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
            }
            Dictionary<string, IEnumerable<string>> d = new Dictionary<string, IEnumerable<string>>
            {
                { "JWT Token", Enumerable.Empty<string>() }
            };
            operation.Security.Add(d);
        }
    }
}
