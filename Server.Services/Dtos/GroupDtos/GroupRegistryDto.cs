using Server.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos.GroupDtos;

public class GroupRegistryDto : IValidatableObject
{
    [Range(1, uint.MaxValue - 1)]
    public uint? GroupId { get; set; }

    [Required]
    [Length(1, 30)]
    public string GroupCode { get; set; }

    [Required]
    [Range(1, uint.MaxValue - 1)]
    public uint SpecialtyId { get; set; }

    [Required]
    [Range(1, 3)]
    public EduLevels EduLevel { get; set; }

    [Required]
    [Range(1, 4)]
    public byte DurationOfStudy { get; set; }

    [Required]
    [Range(2020, 2155)]
    public short AdmissionYear { get; set; }

    [Required]
    [Range(1, 5)]
    public byte Nonparsemester { get; set; }

    [Required]
    [Range(1, 5)]
    public byte Parsemester { get; set; }

    [Required]
    public bool HasEnterChoise { get; set; }

    [Required]
    [Range(0, 2)]
    public byte ChoiceDifference { get; set; }

    [Length(26, 26)]
    public string? CuratorId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AdmissionYear > DateTime.UtcNow.Year)
        {
            yield return new ValidationResult(
                "Не можна створити групу на рік пізніше поточного",
                [nameof(AdmissionYear)]);
        }

        if (DurationOfStudy == 1 && HasEnterChoise == false)
        {
            yield return new ValidationResult(
                "Групи з однорічним навчанням без вибору при вступі не може бути",
                [nameof(DurationOfStudy)]);
        }

        if (ChoiceDifference + DurationOfStudy > 4)
        {
            yield return new ValidationResult(
                "Не може бути різниці вибору, яка вказує на 5 курс",
                [nameof(ChoiceDifference)]);
        }
    }
}
