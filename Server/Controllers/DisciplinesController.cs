using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinesController : ControllerBase
    {
        private readonly DisciplinesService _disciplinesService;

        public DisciplinesController(DisciplinesService disciplinesService)
        {
            _disciplinesService = disciplinesService;
        }

        [Authorize(Roles = "2,3")]
        [HttpGet("getCount")]
        public async Task<IActionResult> GetCount([BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
            [BindRequired][Range(2020, 2155)] short holdingFilter,
            [Range(1, 2)] byte? catalogFilter)
        {
            return Ok(await _disciplinesService.GetCount(facultyId, holdingFilter, catalogFilter));
        }

        [Authorize(Roles = "2,3")]
        [HttpGet("getDisciplines")]
        public async Task<IActionResult> GetDisciplines([Required][Range(1, int.MaxValue - 1)] int pageNumber,
            [Required][Range(1, 100)] int pageSize,
            [BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
            [BindRequired][Range(2020, 2155)] short holdingFilter,
            [Range(1, 2)] byte? catalogFilter)
        {
            return Ok(await _disciplinesService.GetDisciplines(pageNumber, pageSize, facultyId, holdingFilter, catalogFilter));
        }

        [Authorize(Roles = "4")]
        [HttpGet("getCountForStudent")]
        public async Task<IActionResult> GetCountForStudent(
            [BindRequired][Range(1, 3)] byte eduLevel,
            [BindRequired][Range(2020, 2155)] short holding,
            [BindRequired][Range(1, 4)] byte courseFilter,
            [BindRequired][Range(1, 2)] byte catalogFilter,
            [BindRequired][Range(0, 2)] byte semesterFilter,
            [Range(1, uint.MaxValue - 1)] uint? facultyFilter)
        {
            return Ok(await _disciplinesService.GetCountForStudent(eduLevel, holding, catalogFilter, courseFilter, semesterFilter, facultyFilter));
        }

        [Authorize(Roles = "4")]
        [HttpGet("getDisciplinesForStudent/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetDisciplinesForStudent(
            [BindRequired][FromRoute][Range(1, int.MaxValue - 1)] int pageNumber,
            [BindRequired][FromRoute][Range(1, 100)] int pageSize,
            [BindRequired][Range(1, 3)] byte eduLevel,
            [BindRequired][Range(2020, 2155)] short holding,
            [BindRequired][Range(1, 4)] byte courseFilter,
            [BindRequired][Range(1, 2)] byte catalogFilter,
            [BindRequired][Range(0, 2)] byte semesterFilter,
            [Range(1, uint.MaxValue - 1)] uint? facultyFilter)
        {
            return Ok(await _disciplinesService.GetDisciplinesForStudent(pageNumber, pageSize, eduLevel, holding,
                catalogFilter, courseFilter, semesterFilter, facultyFilter));
        }

        [Authorize]
        [HttpGet("getDisciplineById/{disciplineId}")]
        public async Task<IActionResult> GetById(
            [BindRequired][Range(1, uint.MaxValue - 1)] uint disciplineId)
        {
            var discipline = await _disciplinesService.GetById(disciplineId);

            if (discipline is null)
                return NotFound("Дисципліну не знайдено");

            return Ok(discipline);
        }

        [Authorize(Roles = "2")]
        [HttpGet("getShortInfo")]
        public async Task<IActionResult> GetShortInfo([BindRequired][Range(2020, 2155)] short holding,
            [BindRequired][Range(1, 3)] byte eduLevel,
            [BindRequired][Range(1, 2)] byte semester,
            [BindRequired][Length(1, 50)] string code)
        {
            return Ok(await _disciplinesService.GetByCodeSearchYearEduLevelSemester(code, holding, eduLevel, semester));
        }

        [Authorize(Roles = "4")]
        [HttpGet("getOptionsInfo")]
        public async Task<IActionResult> GetOptionsInfo([BindRequired][Range(2020, 2155)] short holding,
            [BindRequired][Range(1, 3)] byte eduLevel,
            [BindRequired][Range(1, 4)] byte course,
            [BindRequired][Range(1, 2)] byte semester,
            [BindRequired][Length(1, 50)] string code)
        {
            return Ok(await _disciplinesService.GetOptionsByCodeSearch(code, holding, eduLevel, course, semester));
        }

        [Authorize(Roles = "2,3")]
        [HttpGet("getTresholds")]
        public async Task<IActionResult> GetTresholds()
        {
            return Ok(_disciplinesService.GetThresholds());
        }

        [Authorize(Roles = "2")]
        [HttpGet("getDisciplinesPrintInfo")]
        public async Task<IActionResult> GetDisciplinesPrintInfo([BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
            [BindRequired][Range(1, 2)] byte catalogType,
            [BindRequired][Range(2020, 2155)] short eduYear,
            [BindRequired][Range(1, 2)] byte semester)
        {
            return Ok(await _disciplinesService.GetDisciplinesPrintInfo(facultyId, catalogType, eduYear, semester));
        }

        [Authorize(Roles = "2,3")]
        [HttpPost("addDiscipline")]
        public async Task<IActionResult> AddDiscipline([FromBody] DisciplineRegistryDto discipline)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                return BadRequest("Неможливо виконати дію");

            return StatusCode(StatusCodes.Status201Created, await _disciplinesService.AddDiscipline(discipline, userId));
        }

        [Authorize(Roles = "2,3")]
        [HttpPut("updateDiscipline")]
        public async Task<IActionResult> UpdateDiscipline([FromBody] DisciplineRegistryDto discipline)
        {
            if (discipline.DisciplineId == 0)
                return BadRequest("Невалідні дані");

            var updatedDiscipline = await _disciplinesService.UpdateDiscipline(discipline);

            if (updatedDiscipline is null)
                return NotFound("Вказана дисциипліна не знайдена");

            return Ok(updatedDiscipline);
        }

        [Authorize(Roles = "2")]
        [HttpDelete("deleteDiscipline/{disciplineId}")]
        public async Task<IActionResult> DeleteDiscipline(
            [BindRequired]
            [Range(1, uint.MaxValue - 1)]
            uint disciplineId)
        {
            var isDisciplineDeleted = await _disciplinesService.DeleteDiscipline(disciplineId);

            if (isDisciplineDeleted is null)
                return BadRequest("Неможливо видалити, оскільки дисципліна активна");

            if (isDisciplineDeleted == false)
                return NotFound("Вказана дисципліна не знайдена");

            return Ok();
        }

        [Authorize(Roles = "2")]
        [HttpPut("updateDisciplineStatus/{disciplineId}")]
        public async Task<IActionResult> UpdateStatus(
            [BindRequired][Range(1, uint.MaxValue - 1)] uint disciplineId)
        {
            bool isSuccess = await _disciplinesService.UpdateStatus(disciplineId);

            if (isSuccess)
                return Ok();

            return NotFound("Дисципліну не знайдено");
        }
    }
}
