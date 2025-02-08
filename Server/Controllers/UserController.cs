using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Dtos;
using Server.Services.Services;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpPost("updatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePassword)
        {
            if (updatePassword == null ||
                string.IsNullOrEmpty(updatePassword.OldPassword) ||
                string.IsNullOrEmpty(updatePassword.NewPassword))
                return BadRequest("Невалідні вхідні дані");

            if (updatePassword.OldPassword == updatePassword.NewPassword)
                return BadRequest("Новий пароль не може бути таким самим, як старий");

            if (!Regex.IsMatch(updatePassword.NewPassword,
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?!.*(.)\1)[a-zA-Z\d!""#$%&'()*+,\-./:;<=>?\@[\\\]^_{|}~]{8,}$"))
                return BadRequest("Новий пароль не задовльняє умови");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null || new Guid(userId).ToByteArray() == updatePassword.UserId)
                return BadRequest("Неспівпадіння даних запиту");

            var res = await _userService.UpdatePassword(updatePassword);

            if (res is null)
                return NotFound("Користувача не знайдено");

            if (res == false)
                return BadRequest("Невірний старий пароль");

            return Ok();
        }
    }
}
