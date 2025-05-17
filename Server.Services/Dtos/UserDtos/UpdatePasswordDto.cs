using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Server.Services.Dtos.UserDtos;

public partial class UpdatePasswordDto : IValidatableObject
{
    [Required]
    [MinLength(1)]
    public string OldPassword { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string NewPassword { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (OldPassword == NewPassword)
        {
            yield return new ValidationResult(
                "Новий пароль не може бути таким самим, як старий",
                [nameof(NewPassword)]);
        }

        if (!PasswordValidationRegex().IsMatch(NewPassword))
        {
            yield return new ValidationResult(
                "Новий пароль не задовльняє умови",
                [nameof(NewPassword)]);
        }
    }

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?!.*(.)\1)[a-zA-Z\d!""#$%&'()*+,\-./:;<=>?\@[\\\]^_{|}~]{8,}$")]
    private static partial Regex PasswordValidationRegex();
}
