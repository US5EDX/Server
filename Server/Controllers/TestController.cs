using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("data")]
        public IActionResult GetProtectedData()
        {
            return Ok(new { Message = "This is a protected endpoint" });
        }

        [Authorize(Roles = "1")] // Доступ тільки для суперадміна
        [HttpGet("admin")]
        public IActionResult GetAdminData()
        {
            return Ok(new { Message = "Super admin only" });
        }
    }
}
