using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Dtos;
using Server.Services.Services;

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
        public async Task<IActionResult> AddFaculty([FromBody] string facultyName)
        {
            if (string.IsNullOrEmpty(facultyName))
                return BadRequest("Невалідні вхідні дані");

            return StatusCode(StatusCodes.Status201Created, await _facultiesService.AddFaculty(facultyName));
        }

        [Authorize(Roles = "1")]
        [HttpPut("updateFaculty")]
        public async Task<IActionResult> UpdateFaculty([FromBody] FacultyDto faculty)
        {
            if (faculty is null)
                return BadRequest("Невалідні вхідні дані");

            var updatedFaculty = await _facultiesService.UpdateFaculty(faculty);

            if (updatedFaculty is null)
                return NotFound("Вказаний факультет не знайдено");

            return Ok(updatedFaculty);
        }

        [Authorize(Roles = "1")]
        [HttpDelete("deleteFaculty")]
        public async Task<IActionResult> DeleteFaculty(uint? facultyId)
        {
            if (facultyId is null)
                return BadRequest("Невалідні вхідні дані");

            var isFacultyDeleted = await _facultiesService.DeleteFaculty(facultyId.Value);

            if (isFacultyDeleted is null)
                return BadRequest("Неможливо видалити, оскільки до факультету є прив'язані дані");

            if (isFacultyDeleted == false)
                return NotFound("Вказаний факультет не знайдено");

            return Ok();
        }
    }
}
