using AuditHeadersPoc.Models;

namespace AuditHeadersPoc.Services;

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
