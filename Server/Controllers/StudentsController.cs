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
    public class StudentsController : ControllerBase
    {
        private readonly StudentsService _studentsService;

        public StudentsController(StudentsService studentsService)
        {
            _studentsService = studentsService;
        }

        [Authorize]
        [HttpGet("getWithRecrodsByGroupId/{groupId}")]
        public async Task<IActionResult> GetCount([BindRequired][Range(1, uint.MaxValue - 1)] uint groupId)
        {
            return Ok(await _studentsService.GetWithLastReocorsByGroupId(groupId));
        }

        [Authorize]
        [HttpPost("addStudent")]
        public async Task<IActionResult> AddStudent([FromBody] StudentRegistryDto student)
        {
            return StatusCode(StatusCodes.Status201Created, await _studentsService.AddStudent(student));
        }

        [Authorize]
        [HttpPost("addStudents")]
        public async Task<IActionResult> AddStudents([FromBody] IEnumerable<StudentRegistryDto> students)
        {
            return StatusCode(StatusCodes.Status201Created, await _studentsService.AddStudents(students));
        }

        [Authorize]
        [HttpPut("updateStudent")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentRegistryDto student)
        {
            if (student.StudentId is null)
                return BadRequest("Невалідні вхідні дані");

            var updatedStudent = await _studentsService.UpdateStudent(student);

            if (updatedStudent is null)
                return NotFound("Вказаного студента не знайдено");

            return Ok(updatedStudent);
        }

        [Authorize(Roles = "2,3")]
        [HttpDelete("deleteStudent/{studentId}")]
        public async Task<IActionResult> DeleteStudent(
            [BindRequired]
            [Length(26,26)]
            string studentId)
        {
            bool isParsed = int.TryParse(User.FindFirst(ClaimTypes.Role)?.Value, out int requestUserRole);

            if (!isParsed)
                return BadRequest("Неможливо виконати дію");

            var isStudentDeleted = await _studentsService.DeleteStudent(studentId, requestUserRole);

            if (isStudentDeleted is null)
                return BadRequest("Неможливо видалити, оскільки до студента є прив'язані дані за останні два роки");

            if (isStudentDeleted == false)
                return NotFound("Вказаного студента не знайдено або недостатній рівень доступу");

            return Ok();
        }
    }
}
