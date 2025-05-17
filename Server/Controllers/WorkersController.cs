using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos.WorkerDtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkersController(WorkersService workersService) : ControllerBase
{
    [Authorize(Roles = "1")]
    [HttpGet("getCount")]
    public async Task<IActionResult> GetCount([Range(1, uint.MaxValue - 1)] uint? facultyFilter) =>
        Ok(await workersService.GetCount(facultyFilter));

    [Authorize(Roles = "1")]
    [HttpGet("getWorkers")]
    public async Task<IActionResult> GetWorkers(
        [BindRequired][Range(1, int.MaxValue - 1)] int pageNumber,
        [BindRequired][Range(1, 1000)] int pageSize,
        [Range(1, uint.MaxValue - 1)] uint? facultyFilter) =>
        Ok(await workersService.GetWorkers(pageNumber, pageSize, facultyFilter));

    [Authorize(Roles = "2")]
    [HttpGet("getByFacultyAndFullName")]
    public async Task<IActionResult> GetByFacultyAndFullName(
        [BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
        [BindRequired][Length(3, 150)] string fullName) =>
        Ok(await workersService.GetByFacultyAndFullName(facultyId, fullName));

    [Authorize(Roles = "1,2")]
    [HttpPost("addWorker")]
    public async Task<IActionResult> AddWorker([FromBody] WorkerRegistryDto worker) =>
        StatusCode(StatusCodes.Status201Created, await workersService.AddWorker(worker,
            User.FindFirst(ClaimTypes.Role)?.Value));

    [Authorize(Roles = "1,2")]
    [HttpPut("updateWorker")]
    public async Task<IActionResult> UpdateWorker([FromBody] WorkerRegistryDto worker) =>
        Ok(await workersService.UpdateOrThrow(worker, User.FindFirst(ClaimTypes.Role)?.Value));

    [Authorize(Roles = "1,2")]
    [HttpDelete("deleteWorker/{workerId}")]
    public async Task<IActionResult> DeleteWorker([BindRequired][Length(26, 26)] string workerId)
    {
        await workersService.DeleteOrThrow(workerId, User.FindFirst(ClaimTypes.Role)?.Value);
        return Ok();
    }
}
