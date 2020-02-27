using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPChat.Infrastructure
{
    public class SwaggerHeaders : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Required = false,
                Description = "access token",
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = new OpenApiString("Bearer ")
                }
            });
        }
    }
}
