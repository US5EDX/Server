using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Services.AppSettingServices;
using System.Text.Json;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SettingsController(KeyedAppSettingsService settingsService) : ControllerBase
{
    [Authorize(Roles = "1")]
    [HttpGet("{key}")]
    public async Task<IActionResult> GetSetting(string key) => Ok(await settingsService.GetByKey(key));

    [Authorize(Roles = "1")]
    [HttpPut("{key}")]
    public async Task<IActionResult> UpdateSetting(string key, [FromBody] JsonElement body)
    {
        await settingsService.UpdateByKey(key, body);
        return Ok();
    }
}
