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
    public class RecordsController : ControllerBase
    {
        private readonly RecordsService _recordsService;

        public RecordsController(RecordsService recordsService)
        {
            _recordsService = recordsService;
        }

        [Authorize(Roles = "2,3")]
        [HttpGet("getSignedStudents")]
        public async Task<IActionResult> GetCount([BindRequired][Range(1, uint.MaxValue - 1)] uint disciplineId,
            [BindRequired][Range(1, 2)] byte semester)
        {
            return Ok(await _recordsService.GetSignedStudents(disciplineId, semester));
        }

        [Authorize(Roles = "2,3")]
        [HttpGet("getByStudentIdAndGroupId")]
        public async Task<IActionResult> GetStudentYearsRecords([BindRequired][Length(26, 26)] string studentId,
            [BindRequired][Range(1, uint.MaxValue - 1)] uint groupId)
        {
            return Ok(await _recordsService.GetByStudentIdAndGroupId(studentId, groupId));
        }

        [Authorize(Roles = "2")]
        [HttpGet("getStudentYearRecords")]
        public async Task<IActionResult> GetStudentYearRecords([BindRequired][Length(26, 26)] string studentId,
            [BindRequired][Range(2020, 2155)] short year)
        {
            return Ok(await _recordsService.GetRecordsByStudentIdAndYear(studentId, year));
        }

        [Authorize(Roles = "4")]
        [HttpGet("getWithDisciplineShortInfo")]
        public async Task<IActionResult> GetWithDisciplineShortInfo(
            [BindRequired][Range(2020, 2155)] short year)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (studentId is null)
                return BadRequest("Невалідний Id");

            return Ok(await _recordsService.GetWithDisciplineShort(studentId, year));
        }

        [Authorize(Roles = "2")]
        [HttpPost("addRecord")]
        public async Task<IActionResult> AddRecord([FromBody] RecordRegistryDto record)
        {
            if (record.RecordId is not null)
                return BadRequest("Невалідні дані");

            return StatusCode(StatusCodes.Status201Created, await _recordsService.AddRecord(record));
        }

        [Authorize(Roles = "4")]
        [HttpPost("registerRecord")]
        public async Task<IActionResult> RegisterRecord([FromBody] RecordRegistryWithoutStudent record)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (studentId is null)
                return BadRequest("Невалідний Id");

            var result = await _recordsService.RegisterRecord(record, studentId);

            return Ok(result);
        }

        [Authorize(Roles = "2")]
        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] RecordRegistryDto record)
        {
            if (record.RecordId is null)
                return BadRequest("Невалідні дані");

            var updatedDiscipline = await _recordsService.UpdateRecord(record);

            if (updatedDiscipline is null)
                return NotFound("Вказаний запис не знайдена");

            return Ok(updatedDiscipline);
        }

        [Authorize(Roles = "2")]
        [HttpDelete("deleteRecord/{recordId}")]
        public async Task<IActionResult> DeleteDiscipline(
            [BindRequired]
            [Range(1, uint.MaxValue - 1)]
            uint recordId)
        {
            var isDisciplineDeleted = await _recordsService.DeleteRecord(recordId);

            if (isDisciplineDeleted is null)
                return BadRequest("Неможливо видалити, оскільки запис затверджено");

            if (isDisciplineDeleted == false)
                return NotFound("Вказаний запис не знайдено");

            return Ok();
        }

        [Authorize(Roles = "2")]
        [HttpPut("updateRecordStatus/{recordId}")]
        public async Task<IActionResult> UpdateStatus(
            [BindRequired][Range(1, uint.MaxValue - 1)] uint recordId)
        {
            bool isSuccess = await _recordsService.UpdateStatus(recordId);

            if (isSuccess)
                return Ok();

            return NotFound("Запис не знайдено");
        }
    }
}
