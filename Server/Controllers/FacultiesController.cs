using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FacultiesController(FacultiesService facultiesService) : ControllerBase
{
    [Authorize]
    [HttpGet("getFaculties")]
    public async Task<IActionResult> GetFaculties() => Ok(await facultiesService.GetAll());

    [Authorize(Roles = "1")]
    [HttpPost("addFaculty")]
    public async Task<IActionResult> AddFaculty([FromBody][BindRequired][Length(1, 100)] string facultyName) =>
        StatusCode(StatusCodes.Status201Created, await facultiesService.AddFaculty(facultyName));

    [Authorize(Roles = "1")]
    [HttpPut("updateFaculty")]
    public async Task<IActionResult> UpdateFaculty([FromBody] FacultyDto faculty) =>
        Ok(await facultiesService.UpdateOrThrow(faculty));

    [Authorize(Roles = "1")]
    [HttpDelete("deleteFaculty/{facultyId}")]
    public async Task<IActionResult> DeleteFaculty([BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId)
    {
        await facultiesService.DeleteOrThrow(facultyId);
        return Ok();
    }
}
