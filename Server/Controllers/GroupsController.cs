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
    public class GroupsController : ControllerBase
    {
        private readonly GroupsService _groupsService;

        public GroupsController(GroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [Authorize(Roles = "2")]
        [HttpGet("getGroupsByCodeSearch")]
        public async Task<IActionResult> GetSpecailties(
            [BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
            [BindRequired][Length(1, 30)] string codeFilter)
        {
            return Ok(await _groupsService.GetByFacultyIdAndCodeFilter(facultyId, codeFilter));
        }
    }
}
