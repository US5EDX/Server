using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings;

public static class SpecialtyMapper
{
    public static SpecialtyDto MapToSpecialtyDto(Specialty specialty) =>
        new() { SpecialtyId = specialty.SpecialtyId, SpecialtyName = specialty.SpecialtyName, FacultyId = specialty.FacultyId };

    public static SpecialtyDto? MapToNullableSpecialtyDto(Specialty? specialty) =>
        specialty is null ? null : MapToSpecialtyDto(specialty);

    public static Specialty MapToSpecialty(SpecialtyDto specailty) =>
        new()
        {
            SpecialtyId = specailty.SpecialtyId ?? 0,
            SpecialtyName = specailty.SpecialtyName,
            FacultyId = specailty.FacultyId ?? 0
        };
}