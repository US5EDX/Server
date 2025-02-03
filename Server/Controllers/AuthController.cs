using Microsoft.AspNetCore.Mvc;
using Server.Services.Dtos;
using Server.Services.Services;

namespace Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("autologin")]
        public async Task<IActionResult> AutoLogin([FromBody] string refreshToken)
        {
            if (refreshToken == null || refreshToken == "")
                return Unauthorized("Refresh token empty");

            var user = await _authService.AutoLogin(refreshToken);

            if (user == null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _authService.Login(login);
            if (user == null)
                return Unauthorized("Invalid email or password");

            return Ok(user);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Resfresh([FromBody] string refreshToken)
        {
            if (refreshToken == null || refreshToken == "")
                return Unauthorized("Refresh token empty");

            var tokens = await _authService.Refresh(refreshToken);

            if (tokens == null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(tokens);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            if (refreshToken == null || refreshToken == "")
                return Unauthorized("Refresh token empty");

            await _authService.Logout(refreshToken);

            return Ok("Logged out successfully");
        }
    }
}
