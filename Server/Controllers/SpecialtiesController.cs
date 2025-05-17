using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpecialtiesController(SpecialtiesService specialtiesService) : ControllerBase
{
    [Authorize]
    [HttpGet("getSpecialties/{facultyId}")]
    public async Task<IActionResult> GetSpecailties([BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId) =>
        Ok(await specialtiesService.GetByFacultyId(facultyId));

    [Authorize(Roles = "2")]
    [HttpPost("addSpecialty")]
    public async Task<IActionResult> AddSpecialty([FromBody] SpecialtyDto specialty) =>
        StatusCode(StatusCodes.Status201Created, await specialtiesService.AddSpecialty(specialty));

    [Authorize(Roles = "2")]
    [HttpPut("updateSpecialty")]
    public async Task<IActionResult> UpdateSpecialty([FromBody] SpecialtyDto specialty) =>
        Ok(await specialtiesService.UpdateOrThrow(specialty));

    [Authorize(Roles = "2")]
    [HttpDelete("deleteSpecialty/{specialtyId}")]
    public async Task<IActionResult> DeleteSpecialty([BindRequired][Range(1, uint.MaxValue - 1)] uint specialtyId)
    {
        await specialtiesService.DeleteOrThrow(specialtyId);
        return Ok();
    }
}
