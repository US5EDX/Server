using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HoldingsController(HoldingsService holdingsService) : ControllerBase
{
    [Authorize]
    [HttpGet("getHoldings")]
    public async Task<IActionResult> GetHoldings() => Ok(await holdingsService.GetAll());

    [Authorize(Roles = "2,3")]
    [HttpGet("getLastFive")]
    public async Task<IActionResult> GetLastFive() => Ok(await holdingsService.GetLastFive());

    [Authorize(Roles = "4")]
    [HttpGet("getLastHolding")]
    public async Task<IActionResult> GetLastHolding() => Ok(await holdingsService.GetLast());

    [Authorize(Roles = "1")]
    [HttpPost("addHolding")]
    public async Task<IActionResult> AddHolding([FromBody] HoldingDto holding) =>
        StatusCode(StatusCodes.Status201Created, await holdingsService.AddHolding(holding));

    [Authorize(Roles = "1")]
    [HttpPut("updateHolding")]
    public async Task<IActionResult> UpdateHolding([FromBody] HoldingDto holding) => Ok(await holdingsService.UpdateOrThrow(holding));

    [Authorize(Roles = "1")]
    [HttpDelete("deleteHolding/{eduYear}")]
    public async Task<IActionResult> DeleteHolding([BindRequired][Range(1901, 2155)] short eduYear)
    {
        await holdingsService.DeleteOrThrow(eduYear);
        return Ok();
    }
}
