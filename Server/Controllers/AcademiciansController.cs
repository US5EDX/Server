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
    public class AcademiciansController : ControllerBase
    {
        private readonly AcademiciansService _academiciansService;

        public AcademiciansController(AcademiciansService academiciansService)
        {
            _academiciansService = academiciansService;
        }

        [Authorize(Roles = "2")]
        [HttpGet("getCount")]
        public async Task<IActionResult> GetCount([BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
            [Range(3, 4)] byte? roleFilter)
        {
            return Ok(await _academiciansService.GetCount(facultyId, roleFilter));
        }

        [Authorize(Roles = "2")]
        [HttpGet("getAcademicians")]
        public async Task<IActionResult> GetAcademicians([Required][Range(1, int.MaxValue - 1)] int pageNumber,
            [Required][Range(1, 1000)] int pageSize,
            [BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
            [Range(3, 4)] byte? roleFilter)
        {
            return Ok(await _academiciansService.GetAcademicians(pageNumber, pageSize, facultyId, roleFilter));
        }
    }
}
