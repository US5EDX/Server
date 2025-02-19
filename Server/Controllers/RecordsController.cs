using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly RecordsService _recordsService;

        public RecordsController(RecordsService recordsService)
        {
            _recordsService = recordsService;
        }

        [Authorize(Roles = "2,3")]
        [HttpGet("getSignedStudents")]
        public async Task<IActionResult> GetCount([BindRequired][Range(1, uint.MaxValue - 1)] uint disciplineId,
            [BindRequired][Range(1, 2)] byte semester)
        {
            return Ok(await _recordsService.GetSignedStudents(disciplineId, semester));
        }
    }
}
