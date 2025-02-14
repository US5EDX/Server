using Server.Services.Validations;
using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class UserFullInfoDto : IValidatableObject
    {
        public string? Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(2, 4)]
        public byte Role { get; set; }

        [Required]
        [Length(1, 150)]
        public string FullName { get; set; }

        [Required]
        public FacultyDto Faculty { get; set; }

        [Required]
        [Length(1, 255)]
        public string Department { get; set; }

        [Required]
        [Length(1, 100)]
        public string Position { get; set; }

        public GroupShortDto? Group { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Role == 4 && Group == null)
            {
                yield return new ValidationResult(
                    "Коли додається студент він повинен мати групу",
                    [nameof(Group)]);
            }
        }
    }
}
