using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController(AuditLogsService auditLogsService) : ControllerBase
    {
        [Authorize(Roles = "1")]
        [HttpGet("getCount")]
        public async Task<IActionResult> GetCount([FromQuery] AuditFiltersDto auditFiltersDto) =>
        Ok(await auditLogsService.GetCount(auditFiltersDto));

        [Authorize(Roles = "1")]
        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetDisciplines(
            [BindRequired][Range(1, int.MaxValue - 1)] int pageNumber,
            [BindRequired][Range(1, 100)] int pageSize,
            [FromQuery] AuditFiltersDto auditFiltersDto) =>
            Ok(await auditLogsService.Get(pageNumber, pageSize, auditFiltersDto));
    }
}
