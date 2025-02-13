using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly UsersService _userService;

        public UsersController(UsersService userService)
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

            if (userId is null || userId != updatePassword.UserId)
                return BadRequest("Неспівпадіння даних запиту");

            var isSuccess = await _userService.UpdatePassword(updatePassword);

            if (isSuccess is null)
                return NotFound("Користувача не знайдено");

            if (isSuccess is false)
                return BadRequest("Невірний старий пароль");

            return Ok();
        }

        [Authorize(Roles = "1,2")]
        [HttpPut("resetPassword/{userId}")]
        public async Task<IActionResult> ResetPassword(
            [BindRequired]
            [Length(26,26)]
            string userId)
        {
            var requestUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var isSuccess = await _userService.ResetPassword(userId, requestUserRole);

            if (isSuccess is null)
                return BadRequest("Неможливо виконати дію");

            if (isSuccess == false)
                return NotFound("Користувача не знайдено");

            return Ok();
        }
    }
}
