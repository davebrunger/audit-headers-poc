using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuditHeadersPoc.OpenApi;

public class AddAuditHeadersDocumetFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach(var path in swaggerDoc.Paths.Values)
        {
            path.Parameters.Insert(0, new OpenApiParameter 
            { 
                Name = "customer-id", 
                Description = "Customer ID", 
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "uuid"
                },
                Example = new OpenApiString("d31e6785-bce7-4e3d-8471-156fdcd7d1ef") 
            });
        }
    }
}
