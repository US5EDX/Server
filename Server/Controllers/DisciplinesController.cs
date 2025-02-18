using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;

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

        [Authorize]
        [HttpPut("getDisciplineById/{disciplineId}")]
        public async Task<IActionResult> GetById(
            [BindRequired][Range(1, uint.MaxValue - 1)] uint disciplineId)
        {
            var discipline = await _disciplinesService.GetById(disciplineId);

            if (discipline is null)
                return NotFound("Дисципліну не знайдено");

            return Ok(discipline);
        }

        [Authorize(Roles = "2,3")]
        [HttpPost("addDiscipline")]
        public async Task<IActionResult> AddDiscipline([FromBody] DisciplineFullInfoDto discipline)
        {
            return StatusCode(StatusCodes.Status201Created, await _disciplinesService.AddDiscipline(discipline));
        }

        [Authorize(Roles = "2,3")]
        [HttpPut("updateDiscipline")]
        public async Task<IActionResult> UpdateDiscipline([FromBody] DisciplineFullInfoDto discipline)
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
