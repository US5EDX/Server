using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Models.Enums;
using Server.Services.Dtos.DisciplineDtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DisciplinesController(DisciplinesService disciplinesService) : ControllerBase
{
    [Authorize(Roles = "2,3")]
    [HttpGet("getCount")]
    public async Task<IActionResult> GetCount(
        [BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
        [BindRequired][Range(2020, 2155)] short holdingFilter,
        [Range(1, 2)] CatalogTypes? catalogFilter,
        [Range(1, 2)] Semesters? semesterFilter,
        [Length(26, 26)] string? lecturerFilter) =>
        Ok(await disciplinesService.GetCount(facultyId, holdingFilter, catalogFilter, semesterFilter, lecturerFilter));

    [Authorize(Roles = "2,3")]
    [HttpGet("getDisciplines")]
    public async Task<IActionResult> GetDisciplines(
        [BindRequired][Range(1, int.MaxValue - 1)] int pageNumber,
        [BindRequired][Range(1, 100)] int pageSize,
        [BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
        [BindRequired][Range(2020, 2155)] short holdingFilter,
        [Range(1, 2)] CatalogTypes? catalogFilter,
        [Range(1, 2)] Semesters? semesterFilter,
        [Length(26, 26)] string? lecturerFilter) =>
        Ok(await disciplinesService.GetDisciplines(pageNumber, pageSize, facultyId,
            holdingFilter, catalogFilter, semesterFilter, lecturerFilter));

    [Authorize(Roles = "4")]
    [HttpGet("getCountForStudent")]
    public async Task<IActionResult> GetCountForStudent(
        [BindRequired][Range(1, 3)] EduLevels eduLevel,
        [BindRequired][Range(2020, 2155)] short holding,
        [BindRequired][Range(1, 4)] byte courseFilter,
        [BindRequired][Range(1, 2)] CatalogTypes catalogFilter,
        [BindRequired][Range(0, 2)] Semesters semesterFilter,
        [Range(1, uint.MaxValue - 1)] uint? facultyFilter) =>
        Ok(await disciplinesService.GetCountForStudent(eduLevel, holding, catalogFilter, courseFilter, semesterFilter, facultyFilter));

    [Authorize(Roles = "4")]
    [HttpGet("getDisciplinesForStudent/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetDisciplinesForStudent(
        [BindRequired][FromRoute][Range(1, int.MaxValue - 1)] int pageNumber,
        [BindRequired][FromRoute][Range(1, 100)] int pageSize,
        [BindRequired][Range(1, 3)] EduLevels eduLevel,
        [BindRequired][Range(2020, 2155)] short holding,
        [BindRequired][Range(1, 4)] byte courseFilter,
        [BindRequired][Range(1, 2)] CatalogTypes catalogFilter,
        [BindRequired][Range(0, 2)] Semesters semesterFilter,
        [Range(1, uint.MaxValue - 1)] uint? facultyFilter) =>
        Ok(await disciplinesService.GetDisciplinesForStudent(pageNumber, pageSize, eduLevel, holding,
            catalogFilter, courseFilter, semesterFilter, facultyFilter));

    [Authorize]
    [HttpGet("getDisciplineById/{disciplineId}")]
    public async Task<IActionResult> GetById([BindRequired][Range(1, uint.MaxValue - 1)] uint disciplineId) =>
        Ok(await disciplinesService.GetByIdOrThrow(disciplineId));

    [Authorize(Roles = "2")]
    [HttpGet("getShortInfo")]
    public async Task<IActionResult> GetShortInfo(
        [BindRequired][Range(2020, 2155)] short holding,
        [BindRequired][Range(1, 3)] EduLevels eduLevel,
        [BindRequired][Range(1, 2)] Semesters semester,
        [BindRequired][Length(1, 50)] string code) =>
        Ok(await disciplinesService.GetByCodeSearchYearEduLevelSemester(code, holding, eduLevel, semester));

    [Authorize(Roles = "4")]
    [HttpGet("getOptionsInfo")]
    public async Task<IActionResult> GetOptionsInfo(
        [BindRequired][Range(2020, 2155)] short holding,
        [BindRequired][Range(1, 3)] EduLevels eduLevel,
        [BindRequired][Range(1, 4)] byte course,
        [BindRequired][Range(1, 2)] Semesters semester,
        [BindRequired][Length(1, 50)] string code) =>
        Ok(await disciplinesService.GetOptionsByCodeSearch(code, holding, eduLevel, course, semester));

    [Authorize(Roles = "2,3")]
    [HttpGet("getTresholds")]
    public async Task<IActionResult> GetTresholds() => Ok(await Task.Run(disciplinesService.GetThresholds));

    [Authorize(Roles = "2")]
    [HttpGet("getDisciplinesPrintInfo")]
    public async Task<IActionResult> GetDisciplinesPrintInfo(
        [BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
        [BindRequired][Range(1, 2)] CatalogTypes catalogType,
        [BindRequired][Range(2020, 2155)] short eduYear,
        [BindRequired][Range(1, 2)] Semesters semester) =>
        Ok(await disciplinesService.GetDisciplinesPrintInfo(facultyId, catalogType, eduYear, semester));

    [Authorize(Roles = "2,3")]
    [HttpPost("addDiscipline")]
    public async Task<IActionResult> AddDiscipline([FromBody] DisciplineRegistryDto discipline)
    {
        var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return StatusCode(StatusCodes.Status201Created, await disciplinesService.AddDisciplineOrThrow(discipline, creatorId));
    }

    [Authorize(Roles = "2,3")]
    [HttpPut("updateDiscipline")]
    public async Task<IActionResult> UpdateDiscipline([FromBody] DisciplineRegistryDto discipline) =>
        Ok(await disciplinesService.UpdateOrThrow(discipline));

    [Authorize(Roles = "2")]
    [HttpDelete("deleteDiscipline/{disciplineId}")]
    public async Task<IActionResult> DeleteDiscipline([BindRequired][Range(1, uint.MaxValue - 1)] uint disciplineId)
    {
        await disciplinesService.DeleteOrThrow(disciplineId);
        return Ok();
    }

    [Authorize(Roles = "2")]
    [HttpPut("updateDisciplineStatus/{disciplineId}")]
    public async Task<IActionResult> UpdateStatus([BindRequired][Range(1, uint.MaxValue - 1)] uint disciplineId)
    {
        await disciplinesService.UpdateStatusOrThrow(disciplineId);
        return Ok();
    }
}
