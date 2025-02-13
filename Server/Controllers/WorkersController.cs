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

        [Authorize(Roles = "1")]
        [HttpPost("addWorker")]
        public async Task<IActionResult> AddHolding([FromBody] WorkerFullInfoDto worker)
        {
            return StatusCode(StatusCodes.Status201Created, await _workersService.AddWorker(worker));
        }

        [Authorize(Roles = "1")]
        [HttpPut("updateWorker")]
        public async Task<IActionResult> UpdateHolding([FromBody] WorkerFullInfoDto worker)
        {
            if (worker.Id is null)
                BadRequest("Невалідні вхідні дані");

            var updatedWorker = await _workersService.UpdateWorker(worker);

            if (updatedWorker is null)
                return NotFound("Вказаного користувача не знайдено");

            return Ok(updatedWorker);
        }

        [Authorize(Roles = "1")]
        [HttpDelete("deleteWorker/{workerId}")]
        public async Task<IActionResult> DeleteWorker(
            [BindRequired]
            [Length(26,26)]
            string workerId)
        {
            var isFacultyDeleted = await _workersService.DeleteWorker(workerId);

            if (isFacultyDeleted is null)
                return BadRequest("Неможливо видалити, оскільки до співробітника є прив'язані дані");

            if (isFacultyDeleted == false)
                return NotFound("Вказаного співобітника не знайдено");

            return Ok();
        }
    }
}
