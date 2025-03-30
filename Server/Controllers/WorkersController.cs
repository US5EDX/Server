using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Models.Models;
using Server.Services.Dtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly WorkersService _workersService;

        public WorkersController(WorkersService workersService)
        {
            _workersService = workersService;
        }

        [Authorize(Roles = "1")]
        [HttpGet("getCount")]
        public async Task<IActionResult> GetCount([Range(1, uint.MaxValue - 1)] uint? facultyFilter)
        {
            return Ok(await _workersService.GetCount(facultyFilter));
        }

        [Authorize(Roles = "1")]
        [HttpGet("getWorkers")]
        public async Task<IActionResult> GetWorkers([Required][Range(1, int.MaxValue - 1)] int pageNumber,
            [Required][Range(1, 1000)] int pageSize,
            [Range(1, uint.MaxValue - 1)] uint? facultyFilter)
        {
            return Ok(await _workersService.GetWorkers(pageNumber, pageSize, facultyFilter));
        }

        [Authorize(Roles = "2")]
        [HttpGet("getByFacultyAndFullName")]
        public async Task<IActionResult> GetByFacultyAndFullName(
            [Required][Range(1, uint.MaxValue - 1)] uint facultyId,
            [Required][Length(3, 150)] string fullName)
        {
            return Ok(await _workersService.GetByFacultyAndFullName(facultyId, fullName));
        }

        [Authorize(Roles = "1,2")]
        [HttpPost("addWorker")]
        public async Task<IActionResult> AddWorker([FromBody] UserFullInfoDto worker)
        {
            bool isParsed = int.TryParse(User.FindFirst(ClaimTypes.Role)?.Value, out int requestUserRole);

            if (!isParsed || worker.Role == requestUserRole)
                return BadRequest("Неможливо виконати дію");

            return StatusCode(StatusCodes.Status201Created, await _workersService.AddWorker(worker));
        }

        [Authorize(Roles = "1,2")]
        [HttpPut("updateWorker")]
        public async Task<IActionResult> UpdateWorker([FromBody] UserFullInfoDto worker)
        {
            if (worker.Id is null)
                return BadRequest("Невалідні вхідні дані");

            bool isParsed = int.TryParse(User.FindFirst(ClaimTypes.Role)?.Value, out int requestUserRole);

            if (!isParsed || worker.Role <= requestUserRole)
                return BadRequest("Неможливо виконати дію");

            var updatedWorker = await _workersService.UpdateWorker(worker);

            if (updatedWorker is null)
                return NotFound("Вказаного користувача не знайдено");

            return Ok(updatedWorker);
        }

        [Authorize(Roles = "1,2")]
        [HttpDelete("deleteWorker/{workerId}")]
        public async Task<IActionResult> DeleteWorker(
            [BindRequired]
            [Length(26,26)]
            string workerId)
        {
            bool isParsed = int.TryParse(User.FindFirst(ClaimTypes.Role)?.Value, out int requestUserRole);

            if (!isParsed)
                return BadRequest("Неможливо виконати дію");

            var isFacultyDeleted = await _workersService.DeleteWorker(workerId, requestUserRole);

            if (isFacultyDeleted is null)
                return BadRequest("Неможливо видалити, оскільки до співробітника є прив'язані дані");

            if (isFacultyDeleted == false)
                return NotFound("Вказаного співобітника не знайдено або недостатній рівень доступу");

            return Ok();
        }
    }
}
