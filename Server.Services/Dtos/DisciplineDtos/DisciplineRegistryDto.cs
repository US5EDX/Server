﻿using Server.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos.DisciplineDtos;

public class DisciplineRegistryDto : IValidatableObject
{
    [Range(0, uint.MaxValue - 1)]
    public uint DisciplineId { get; set; }

    [Required]
    [Length(1, 50)]
    public string DisciplineCode { get; set; } = null!;

    [Required]
    [Range(1, 2)]
    public CatalogTypes CatalogType { get; set; }

    [Required]
    [Range(1, uint.MaxValue - 1)]
    public uint FacultyId { get; set; }

    [Range(1, uint.MaxValue - 1)]
    public uint? SpecialtyId { get; set; }

    [Required]
    [Length(1, 200)]
    public string DisciplineName { get; set; } = null!;

    [Required]
    [Range(1, 3)]
    public EduLevels EduLevel { get; set; }

    [Required]
    [Range(1, 15)]
    public byte Course { get; set; }

    [Required]
    [Range(0, 2)]
    public Semesters Semester { get; set; }

    [Required]
    [Length(1, 500)]
    public string Prerequisites { get; set; } = null!;

    [Required]
    [Length(1, 1000)]
    public string Interest { get; set; } = null!;

    [Required]
    [Range(0, 500)]
    public int MaxCount { get; set; }

    [Required]
    [Range(0, 500)]
    public int MinCount { get; set; }

    [Required]
    [Length(1, 500)]
    public string Url { get; set; } = null!;

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

        if (Semester != Semesters.Both && IsYearLong)
        {
            yield return new ValidationResult(
                "Дисципліна має бути розрахована на 2 семестри, щоб вивчатись протягом року",
                [nameof(IsYearLong)]);
        }
    }
}
