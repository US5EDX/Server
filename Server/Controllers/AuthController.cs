using Microsoft.AspNetCore.Mvc;
using Server.Services.Dtos;
using Server.Services.Services;

namespace Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _userService;

        public AuthController(AuthService userService)
        {
            _userService = userService;
        }

        [HttpPost("autologin")]
        public async Task<IActionResult> AutoLogin([FromBody] string refreshToken)
        {
            if (refreshToken == null || refreshToken == "")
                return Unauthorized("Refresh token empty");

            var user = await _userService.AutoLogin(refreshToken);

            if (user == null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _userService.Login(login);
            if (user == null)
                return Unauthorized("Invalid email or password");

            return Ok(user);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Resfresh([FromBody] string refreshToken)
        {
            if (refreshToken == null || refreshToken == "")
                return Unauthorized("Refresh token empty");

            var tokens = await _userService.Refresh(refreshToken);

            if (tokens == null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(tokens);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            if (refreshToken == null || refreshToken == "")
                return Unauthorized("Refresh token empty");

            await _userService.Logout(refreshToken);

            return Ok("Logged out successfully");
        }
    }
}
