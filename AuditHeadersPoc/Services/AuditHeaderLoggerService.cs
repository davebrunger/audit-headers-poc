using System.Diagnostics;

namespace AuditHeadersPoc.Services
{
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
}
