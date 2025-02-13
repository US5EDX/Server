using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Services.Dtos;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoldingsController : ControllerBase
    {
        private readonly HoldingsService _holdingsService;

        public HoldingsController(HoldingsService holdingsService)
        {
            _holdingsService = holdingsService;
        }

        [Authorize]
        [HttpGet("getHoldings")]
        public async Task<IActionResult> GetHoldings()
        {
            return Ok(await _holdingsService.GetAll());
        }

        [Authorize(Roles = "1")]
        [HttpPost("addHolding")]
        public async Task<IActionResult> AddHolding([FromBody] HoldingDto holding)
        {
            return StatusCode(StatusCodes.Status201Created, await _holdingsService.AddHolding(holding));
        }

        [Authorize(Roles = "1")]
        [HttpPut("updateHolding")]
        public async Task<IActionResult> UpdateHolding([FromBody] HoldingDto holding)
        {
            var updatedHolding = await _holdingsService.UpdateHolding(holding);

            if (updatedHolding is null)
                return NotFound("Вказаний навчальний рік не знайдено");

            return Ok(updatedHolding);
        }

        [Authorize(Roles = "1")]
        [HttpDelete("deleteHolding/{eduYear}")]
        public async Task<IActionResult> DeleteHolding(
            [BindRequired]
            [Range(1901, 2155)]
            short eduYear)
        {
            var isHoldingDeleted = await _holdingsService.DeleteHolding(eduYear);

            if (isHoldingDeleted is null)
                return BadRequest("Неможливо видалити, оскільки до навчального є прив'язані дані");

            if (isHoldingDeleted == false)
                return NotFound("Вказаний навчальний рік не знайдено");

            return Ok();
        }
    }
}
