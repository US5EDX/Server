using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos.StudentDtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(StudentsService studentsService) : ControllerBase
{
    [Authorize]
    [HttpGet("getWithRecrodsByGroupId/{groupId}")]
    public async Task<IActionResult> GetWithLastReocorsByGroupId([BindRequired][Range(1, uint.MaxValue - 1)] uint groupId) =>
        Ok(await studentsService.GetWithLastReocorsByGroupId(groupId));

    [Authorize(Roles = "2,3")]
    [HttpGet("getWithAllRecrodsByGroupId/{groupId}")]
    public async Task<IActionResult> GetWithAllRecordsByGroupId([BindRequired][Range(1, uint.MaxValue - 1)] uint groupId) =>
        Ok(await studentsService.GetWithAllRecordsByGroupId(groupId));

    [Authorize]
    [HttpPost("addStudent")]
    public async Task<IActionResult> AddStudent([FromBody] StudentRegistryDto student) =>
        StatusCode(StatusCodes.Status201Created, await studentsService.AddStudent(student));

    [Authorize]
    [HttpPost("addStudents")]
    public async Task<IActionResult> AddStudents([FromBody] List<StudentRegistryDto> students) =>
        StatusCode(StatusCodes.Status201Created, await studentsService.AddStudents(students));

    [Authorize]
    [HttpPut("updateStudent")]
    public async Task<IActionResult> UpdateStudent([FromBody] StudentRegistryDto student) =>
        Ok(await studentsService.UpdateOrThrow(student));

    [Authorize(Roles = "2,3")]
    [HttpDelete("deleteStudent/{studentId}")]
    public async Task<IActionResult> DeleteStudent([BindRequired][Length(26, 26)] string studentId)
    {
        await studentsService.DeleteOrThrow(studentId, User.FindFirst(ClaimTypes.Role)?.Value);
        return Ok();
    }
}
