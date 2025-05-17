using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Models.Enums;
using Server.Services.Services;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AcademiciansController(AcademiciansService academiciansService) : ControllerBase
{
    [Authorize(Roles = "2")]
    [HttpGet("getCount")]
    public async Task<IActionResult> GetCount(
        [BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
        [Range(3, 4)] Roles? roleFilter) =>
        Ok(await academiciansService.GetCount(facultyId, roleFilter));

    [Authorize(Roles = "2")]
    [HttpGet("getAcademicians")]
    public async Task<IActionResult> GetAcademicians(
        [BindRequired][Range(1, int.MaxValue - 1)] int pageNumber,
        [BindRequired][Range(1, 1000)] int pageSize,
        [BindRequired][Range(1, uint.MaxValue - 1)] uint facultyId,
        [Range(3, 4)] Roles? roleFilter) =>
        Ok(await academiciansService.GetAcademicians(pageNumber, pageSize, facultyId, roleFilter));
}
