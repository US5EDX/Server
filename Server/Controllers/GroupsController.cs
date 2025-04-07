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
    public class GroupsController : ControllerBase
    {
        private readonly GroupsService _groupsService;

        public GroupsController(GroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [Authorize]
        [HttpGet("getGroupById/{groupId}")]
        public async Task<IActionResult> GetGroupById(
            [BindRequired][Range(1, uint.MaxValue - 1)] uint groupId)
        {
            return Ok(await _groupsService.GetGroupById(groupId));
        }

        [Authorize(Roles = "2")]
        [HttpGet("getByFacultyId")]
        public async Task<IActionResult> GetGroups(
            [BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId)
        {
            return Ok(await _groupsService.GetByFacultyId(facultyId));
        }

        [Authorize(Roles = "2")]
        [HttpPost("addGroup")]
        public async Task<IActionResult> AddGroup([FromBody] GroupRegistryDto group)
        {
            return StatusCode(StatusCodes.Status201Created, await _groupsService.AddGroup(group));
        }

        [Authorize(Roles = "2")]
        [HttpPut("updateGroup")]
        public async Task<IActionResult> UpdateGroup([FromBody] GroupRegistryDto group)
        {
            if (group.GroupId is null)
                return BadRequest("Невалідні вхідні дані");

            var updatedSpecialty = await _groupsService.UpdateGroup(group);

            if (updatedSpecialty is null)
                return NotFound("Вказана група не знайдена");

            return Ok(updatedSpecialty);
        }

        [Authorize(Roles = "2")]
        [HttpDelete("deleteGroup/{groupId}")]
        public async Task<IActionResult> DeleteGroup(
            [BindRequired]
            [Range(1, uint.MaxValue - 1)]
            uint groupId)
        {
            var isGroupDeleted = await _groupsService.DeleteGroup(groupId);

            if (isGroupDeleted is null)
                return BadRequest("Неможливо видалити, оскільки у групі є студенти або група ще не закінчила навчання");

            if (isGroupDeleted == false)
                return NotFound("Вказана група не знайдена");

            return Ok();
        }
    }
}
