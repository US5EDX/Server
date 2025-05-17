using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Models.CustomExceptions;
using Server.Services.Dtos.GroupDtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController(GroupsService groupsService) : ControllerBase
{
    [Authorize]
    [HttpGet("getGroupById/{groupId}")]
    public async Task<IActionResult> GetGroupById([BindRequired][Range(1, uint.MaxValue - 1)] uint groupId) =>
        Ok(await groupsService.GetByIdOrThrow(groupId));

    [Authorize(Roles = "2,3")]
    [HttpGet("getByFacultyId")]
    public async Task<IActionResult> GetGroups([BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            throw new BadRequestException("Неможливо виконати дію");

        var requestUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(await groupsService.GetByFacultyId(facultyId, requestUserRole == "3" ? userId : null));
    }

    [Authorize(Roles = "2")]
    [HttpPost("addGroup")]
    public async Task<IActionResult> AddGroup([FromBody] GroupRegistryDto group) =>
        StatusCode(StatusCodes.Status201Created, await groupsService.AddGroup(group));

    [Authorize(Roles = "2")]
    [HttpPut("updateGroup")]
    public async Task<IActionResult> UpdateGroup([FromBody] GroupRegistryDto group) => Ok(await groupsService.UpdateOrThrow(group));

    [Authorize(Roles = "2")]
    [HttpDelete("deleteGroup/{groupId}")]
    public async Task<IActionResult> DeleteGroup([BindRequired][Range(1, uint.MaxValue - 1)] uint groupId)
    {
        await groupsService.DeleteOrThrow(groupId);
        return Ok();
    }

    [Authorize(Roles = "2")]
    [HttpDelete("deleteGraduated/{facultyId}")]
    public async Task<IActionResult> DeleteGraduated([BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId)
    {
        await groupsService.DeleteGraduatedOrThrow(facultyId);
        return Ok();
    }
}
