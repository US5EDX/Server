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
    public class UsersController : ControllerBase
    {
        public readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpPut("updatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePassword)
        {
            if (updatePassword.OldPassword == updatePassword.NewPassword)
                return BadRequest("Новий пароль не може бути таким самим, як старий");

            if (!Regex.IsMatch(updatePassword.NewPassword,
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?!.*(.)\1)[a-zA-Z\d!""#$%&'()*+,\-./:;<=>?\@[\\\]^_{|}~]{8,}$"))
                return BadRequest("Новий пароль не задовльняє умови");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null || new Guid(userId).ToByteArray() == updatePassword.UserId)
                return BadRequest("Неспівпадіння даних запиту");

            var isSuccess = await _userService.UpdatePassword(updatePassword);

            if (isSuccess is null)
                return NotFound("Користувача не знайдено");

            if (isSuccess is false)
                return BadRequest("Невірний старий пароль");

            return Ok();
        }
    }
}
