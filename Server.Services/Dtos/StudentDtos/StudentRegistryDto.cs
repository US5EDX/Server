using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos.StudentDtos;

public class StudentRegistryDto
{
    [Length(26, 26)]
    public string? StudentId { get; set; }

    [Required]
    [EmailAddress]
    [Length(1, 255)]
    public string Email { get; set; } = null!;

    [Required]
    [Length(1, 150)]
    public string FullName { get; set; } = null!;

    [Required]
    [Range(1, uint.MaxValue - 1)]
    public uint Group { get; set; }

    [Required]
    [Range(1, uint.MaxValue - 1)]
    public uint Faculty { get; set; }

    [Required]
    public bool Headman { get; set; }
}
