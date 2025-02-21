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
            var valResult = ValidateToken(refreshToken);

            if (valResult is not null)
                return Unauthorized(valResult);

            var user = await _authService.AutoLogin(refreshToken);

            if (user is null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _authService.Login(login);

            if (user is null)
                return Unauthorized("Invalid email or password");

            return Ok(user);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Resfresh([FromBody] string refreshToken)
        {
            var valResult = ValidateToken(refreshToken);

            if (valResult is not null)
                return Unauthorized(valResult);

            var tokens = await _authService.Refresh(refreshToken);

            if (tokens is null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(tokens);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            var valResult = ValidateToken(refreshToken);

            if (valResult is not null)
                return Unauthorized(valResult);

            await _authService.Logout(refreshToken);

            return Ok();
        }

        private string? ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return "Refresh token empty";

            if (token.Length != 32)
                return "Unvalid token";

            return null;
        }
    }
}
