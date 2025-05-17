using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos.UserDtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(UsersService userService) : ControllerBase
{
    [Authorize]
    [HttpPut("updatePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePassword)
    {
        await userService.UpdatePasswordOrThrow(updatePassword, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return Ok();
    }

    [Authorize(Roles = "1,2")]
    [HttpPut("resetPassword/{userId}")]
    public async Task<IActionResult> ResetPassword([BindRequired][Length(26, 26)] string userId)
    {
        await userService.ResetPasswordOrThrow(userId, User.FindFirst(ClaimTypes.Role)?.Value);
        return Ok();
    }
}
