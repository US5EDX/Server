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
    public class FacultiesController : ControllerBase
    {
        private readonly FacultiesService _facultiesService;

        public FacultiesController(FacultiesService facultiesService)
        {
            _facultiesService = facultiesService;
        }

        [Authorize]
        [HttpGet("getFaculties")]
        public async Task<IActionResult> GetFaculties()
        {
            return Ok(await _facultiesService.GetAll());
        }

        [Authorize(Roles = "1")]
        [HttpPost("addFaculty")]
        public async Task<IActionResult> AddFaculty([FromBody]
        [Required]
        [Length(1, 100)]
        string facultyName)
        {
            return StatusCode(StatusCodes.Status201Created, await _facultiesService.AddFaculty(facultyName));
        }

        [Authorize(Roles = "1")]
        [HttpPut("updateFaculty")]
        public async Task<IActionResult> UpdateFaculty([FromBody] FacultyDto faculty)
        {
            var updatedFaculty = await _facultiesService.UpdateFaculty(faculty);

            if (updatedFaculty is null)
                return NotFound("Вказаний факультет не знайдено");

            return Ok(updatedFaculty);
        }

        [Authorize(Roles = "1")]
        [HttpDelete("deleteFaculty/{facultyId}")]
        public async Task<IActionResult> DeleteFaculty(
            [BindRequired]
            [Range(1, uint.MaxValue - 1)]
            uint facultyId)
        {
            var isFacultyDeleted = await _facultiesService.DeleteFaculty(facultyId);

            if (isFacultyDeleted is null)
                return BadRequest("Неможливо видалити, оскільки до факультету є прив'язані дані");

            if (isFacultyDeleted == false)
                return NotFound("Вказаний факультет не знайдено");

            return Ok();
        }
    }
}
