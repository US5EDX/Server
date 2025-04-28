using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Dtos
{
    public class DisciplineFullInfoDto : IValidatableObject
    {
        [Range(0, uint.MaxValue - 1)]
        public uint DisciplineId { get; set; }

        [Required]
        [Length(1, 50)]
        public string DisciplineCode { get; set; }

        [Required]
        [Range(1, 2)]
        public byte CatalogType { get; set; }

        [Required]
        public FacultyDto Faculty { get; set; }

        public SpecialtyDto? Specialty { get; set; }

        [Required]
        [Length(1, 200)]
        public string DisciplineName { get; set; }

        [Required]
        [Range(1, 3)]
        public byte EduLevel { get; set; }

        [Required]
        [Length(1, 250)]
        public string Course { get; set; }

        [Required]
        [Range(0, 2)]
        public byte Semester { get; set; }

        [Required]
        [Length(1, 500)]
        public string Prerequisites { get; set; }

        [Required]
        [Length(1, 1000)]
        public string Interest { get; set; }

        [Required]
        [Range(0, 500)]
        public int MaxCount { get; set; }

        [Required]
        [Range(0, 500)]
        public int MinCount { get; set; }

        [Required]
        [Length(1, 500)]
        public string Url { get; set; }

        [Required]
        [Range(2020, 2155)]
        public short Holding { get; set; }

        [Required]
        public bool IsYearLong { get; set; }

        [Required]
        public bool IsOpen { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MaxCount != 0 && MaxCount < MinCount)
            {
                yield return new ValidationResult(
                    "Максимум студентів не може бути менше за мінімум",
                    [nameof(MaxCount)]);
            }

            if (!Uri.TryCreate(Url, UriKind.Absolute, out var outUri)
                   || !(outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
            {
                yield return new ValidationResult(
                    "Значення не є посиланням",
                    [nameof(Url)]);
            }

            if (Semester != 0 && IsYearLong)
            {
                yield return new ValidationResult(
                    "Дисципліна має бути розрахована на 2 семестри, щоб вивчатись протягом року",
                    [nameof(IsYearLong)]);
            }
        }
    }
}
