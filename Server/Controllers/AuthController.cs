using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos.AuthDtos;
using Server.Services.Services.AuthorizationServices;

namespace Server.Controllers;

[Route("api")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login) =>
        Ok(await authService.LoginOrThrow(login, DateTime.UtcNow));

    [HttpPost("autologin")]
    public async Task<IActionResult> AutoLogin([BindRequired][FromBody] string refreshToken) =>
        Ok(await authService.AutoLoginOrThrow(refreshToken, DateTime.UtcNow));

    [HttpPost("refresh")]
    public async Task<IActionResult> Resfresh([BindRequired][FromBody] string refreshToken) =>
        Ok(await authService.RefreshOrThrow(refreshToken, DateTime.UtcNow));

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([BindRequired][FromBody] string refreshToken)
    {
        await authService.Logout(refreshToken);
        return Ok();
    }
}
