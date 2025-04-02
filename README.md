# audit-headers-poc
Proof of Concept that captures specific header information and injects it into a component. Open API/Swagger documentation is added to all endpoints for those headers

## Reading Headers
Add the following line to the `Program.cs` file to allow the HttpContext to be injected into components

```C#
builder.Services.AddHttpContextAccessor();
```

Read the headers in a service like the following:

```C#
public interface IAuditHeaderService
{
    public AuditHeaders AuditHeaders { get; }
}

public class AuditHeaderService : IAuditHeaderService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public AuditHeaderService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public AuditHeaders AuditHeaders
    {
        get
        {
            var context = httpContextAccessor.HttpContext;
            var customerIdStr = context!.Request.Headers["customer-id"];
            Guid? customerId = Guid.TryParse(customerIdStr, out var id)
                ? id 
                : null;
            return new AuditHeaders(customerId);
        }
    }
}
```

Inject the captured data into a component:

```C#
public interface IAuditHeaderLoggerService
{
    void Log(string message);
}

public class AuditHeaderLoggerService : IAuditHeaderLoggerService
{
    private readonly IAuditHeaderService auditHeaderService;

    public AuditHeaderLoggerService(IAuditHeaderService auditHeaderService)
    {
        this.auditHeaderService = auditHeaderService;
    }

    public void Log(string message)
    {
        Debug.WriteLine($"{auditHeaderService.AuditHeaders.CustomerId}: {message}");
    }
}
```

Register both classes as a **Scoped** service in `Program.cs` to ensure the correct data is injected:

```C#
builder.Services.AddScoped<IAuditHeaderService, AuditHeaderService>();
builder.Services.AddScoped<IAuditHeaderLoggerService, AuditHeaderLoggerService>();
```

## Adding the headers to the `swagger.json` output

Create a document filter:

```C#
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
```

And regster it in the `Program.cs file`:

```C#
builder.Services.AddSwaggerGen(options => {
    options.DocumentFilter<AddAuditHeadersDocumetFilter>();
});
```