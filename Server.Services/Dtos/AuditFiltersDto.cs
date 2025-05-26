using Server.Services.Validations;
using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos;

public class AuditFiltersDto : IValidatableObject
{
    [Required]
    [RegularExpression("Added|Modified|Deleted", ErrorMessage = "Invalid actionType.")]
    public string ActionType { get; set; } = null!;

    [Required]
    [DateRange("2020-01-01", "2100-12-31")]
    public DateTime LeftBorder { get; set; }

    [Required]
    [DateRange("2020-01-01", "2100-12-31")]
    public DateTime RightBorder { get; set; }

    [Required]
    public bool HasDescription { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (LeftBorder > RightBorder)
            yield return new ValidationResult(
                "Left border can't be greater then right one.",
                [nameof(LeftBorder), nameof(RightBorder)]);
    }
}
