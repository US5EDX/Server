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
    public class SpecialtiesController : ControllerBase
    {
        private readonly SpecialtiesService _specialtiesService;

        public SpecialtiesController(SpecialtiesService specialtiesService)
        {
            _specialtiesService = specialtiesService;
        }

        [Authorize]
        [HttpGet("getSpecialties/{facultyId}")]
        public async Task<IActionResult> GetSpecailties(
            [BindRequired]
            [Range(1, uint.MaxValue - 1)]
            uint facultyId)
        {
            return Ok(await _specialtiesService.GetByFacultyId(facultyId));
        }

        [Authorize(Roles = "2")]
        [HttpPost("addSpecialty")]
        public async Task<IActionResult> AddSpecialty([FromBody] SpecialtyDto specialty)
        {
            if (specialty.FacultyId < 1 || specialty.FacultyId > (uint.MaxValue - 1))
                return BadRequest("Невалідні вхідні дані");

            return StatusCode(StatusCodes.Status201Created, await _specialtiesService.AddSpecialty(specialty));
        }

        [Authorize(Roles = "2")]
        [HttpPut("updateSpecialty")]
        public async Task<IActionResult> UpdateSpecialty([FromBody] SpecialtyDto specialty)
        {
            if (specialty.SpecialtyId < 1 || specialty.SpecialtyId > (uint.MaxValue - 1))
                return BadRequest("Невалідні вхідні дані");

            var updatedSpecialty = await _specialtiesService.UpdateSpecialty(specialty);

            if (updatedSpecialty is null)
                return NotFound("Вказана спеціальність не знайдена");

            return Ok(updatedSpecialty);
        }

        [Authorize(Roles = "2")]
        [HttpDelete("deleteSpecialty/{specialtyId}")]
        public async Task<IActionResult> DeleteSpecialty(
            [BindRequired]
            [Range(1, uint.MaxValue - 1)]
            uint specialtyId)
        {
            var isSpecialtyDeleted = await _specialtiesService.DeleteSpecialty(specialtyId);

            if (isSpecialtyDeleted is null)
                return BadRequest("Неможливо видалити, оскільки до спеціальності є прив'язані дані");

            if (isSpecialtyDeleted == false)
                return NotFound("Вказана спеціальність не знайдена");

            return Ok();
        }
    }
}
