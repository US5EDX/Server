using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos.AuthDtos;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}
