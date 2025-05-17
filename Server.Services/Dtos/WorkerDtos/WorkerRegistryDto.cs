using Server.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos.WorkerDtos;

public class WorkerRegistryDto
{
    [Length(26, 26)]
    public string? WorkerId { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Range(2, 4)]
    public Roles Role { get; set; }

    [Required]
    [Length(1, 150)]
    public string FullName { get; set; } = null!;

    [Required]
    [Range(1, uint.MaxValue - 1)]
    public uint FacultyId { get; set; }

    [Required]
    [Length(1, 255)]
    public string Department { get; set; } = null!;

    [Required]
    [Length(1, 100)]
    public string Position { get; set; } = null!;
}
