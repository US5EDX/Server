using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Services.Dtos.RecordDtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecordsController(RecordsService recordsService) : ControllerBase
{
    [Authorize(Roles = "2,3")]
    [HttpGet("getSignedStudents")]
    public async Task<IActionResult> GetSignedStudents(
        [BindRequired][Range(1, uint.MaxValue - 1)] uint disciplineId,
        [BindRequired][Range(1, 2)] Semesters semester) =>
        Ok(await recordsService.GetSignedStudents(disciplineId, semester));

    [Authorize(Roles = "2,3")]
    [HttpGet("getByStudentIdAndGroupId")]
    public async Task<IActionResult> GetStudentYearsRecords(
        [BindRequired][Length(26, 26)] string studentId,
        [BindRequired][Range(1, uint.MaxValue - 1)] uint groupId) =>
        Ok(await recordsService.GetByStudentAndGroupIdOrThrow(studentId, groupId));

    [Authorize(Roles = "2,3")]
    [HttpGet("getStudentYearRecords")]
    public async Task<IActionResult> GetStudentYearRecords(
        [BindRequired][Length(26, 26)] string studentId,
        [BindRequired][Range(2020, 2155)] short year) =>
        Ok(await recordsService.GetRecordsByStudentIdAndYear(studentId, year));

    [Authorize(Roles = "4")]
    [HttpGet("getWithDisciplineShortInfo")]
    public async Task<IActionResult> GetWithDisciplineShortInfo([BindRequired][Range(2020, 2155)] short year)
    {
        var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(await recordsService.GetWithDisciplineShort(studentId, year));
    }

    [Authorize(Roles = "4")]
    [HttpGet("getMadeChoices")]
    public async Task<IActionResult> GetMadeChoices() =>
        Ok(await recordsService.GetMadeChoices(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

    [Authorize(Roles = "2")]
    [HttpPost("addRecord")]
    public async Task<IActionResult> AddRecord([FromBody] RecordRegistryDto record) =>
        StatusCode(StatusCodes.Status201Created, await recordsService.AddRecord(record));

    [Authorize(Roles = "4")]
    [HttpPost("registerRecord")]
    public async Task<IActionResult> RegisterRecord([FromBody] RecordRegistryWithoutStudent record)
    {
        var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(await recordsService.RegisterRecordOrThrow(record, studentId));
    }

    [Authorize(Roles = "2")]
    [HttpPut("updateRecord")]
    public async Task<IActionResult> UpdateRecord([FromBody] RecordRegistryDto record) =>
        Ok(await recordsService.UpdateOrThrow(record));

    [Authorize(Roles = "2")]
    [HttpDelete("deleteRecord/{recordId}")]
    public async Task<IActionResult> DeleteDiscipline([BindRequired][Range(1, uint.MaxValue - 1)] uint recordId)
    {
        await recordsService.DeleteOrThrow(recordId);
        return Ok();
    }


    [HttpPut("updateRecordStatus/{recordId}")]
    public async Task<IActionResult> UpdateStatus(
        [BindRequired][Range(1, uint.MaxValue - 1)] uint recordId,
        [FromBody][BindRequired][Range(0, 2)] RecordStatus status)
    {
        var requestUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (requestUserRole == "3" && status < RecordStatus.UnderSuspicious)
            throw new ForbidException("У доступі відмовлено");

        await recordsService.UpdateStatusOrThrow(recordId, status);
        return Ok();
    }
}
