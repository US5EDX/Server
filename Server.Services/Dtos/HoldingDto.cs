using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos;

public class HoldingDto : IValidatableObject
{
    [Required]
    [Range(1901, 2155)]
    public short EduYear { get; set; }

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate >= EndDate)
            yield return new ValidationResult(
                "Початкова дата не може бути пізніше або дорівнювати кінцевій",
                [nameof(StartDate)]);
    }
}