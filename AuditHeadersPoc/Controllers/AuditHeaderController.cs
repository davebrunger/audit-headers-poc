using AuditHeadersPoc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Validations.Rules;

namespace AuditHeadersPoc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditHeaderController : ControllerBase
    {
        private readonly IAuditHeaderLoggerService auditHeaderLoggerService;

        public AuditHeaderController(IAuditHeaderLoggerService auditHeaderLoggerService)
        {
            this.auditHeaderLoggerService = auditHeaderLoggerService;
        }

        [HttpPost("{message}")]
        public void Log(string message)
        {
            auditHeaderLoggerService.Log(message);
        }
    }
}
