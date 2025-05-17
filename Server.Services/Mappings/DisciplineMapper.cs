using Server.Models.Models;
using Server.Services.Converters;
using Server.Services.Dtos.DisciplineDtos;

namespace Server.Services.Mappings;

public static class DisciplineMapper
{
    public static DisciplineFullInfoDto MapToDisciplineFullInfoDto(Discipline discipline) =>
        new()
        {
            DisciplineId = discipline.DisciplineId,
            DisciplineCode = discipline.DisciplineCode,
            CatalogType = discipline.CatalogType,
            Faculty = FacultyMapper.MapToFacultyDto(discipline.Faculty),
            Specialty = SpecialtyMapper.MapToNullableSpecialtyDto(discipline.Specialty),
            DisciplineName = discipline.DisciplineName,
            EduLevel = discipline.EduLevel,
            Course = CourseMaskConverter.GetCourseMaskString(discipline.Course),
            Semester = discipline.Semester,
            Prerequisites = discipline.Prerequisites,
            Interest = discipline.Interest,
            MaxCount = discipline.MaxCount,
            MinCount = discipline.MinCount,
            Url = discipline.Url,
            Holding = discipline.Holding,
            IsYearLong = discipline.IsYearLong,
            IsOpen = discipline.IsOpen,
        };

    public static Discipline MapToDiscipline(DisciplineRegistryDto discipline) =>
        new()
        {
            DisciplineId = discipline.DisciplineId,
            DisciplineCode = discipline.DisciplineCode,
            CatalogType = discipline.CatalogType,
            FacultyId = discipline.FacultyId,
            SpecialtyId = discipline.SpecialtyId,
            DisciplineName = discipline.DisciplineName,
            EduLevel = discipline.EduLevel,
            Course = discipline.Course,
            Semester = discipline.Semester,
            Prerequisites = discipline.Prerequisites,
            Interest = discipline.Interest,
            MaxCount = discipline.MaxCount,
            MinCount = discipline.MinCount,
            Url = discipline.Url,
            Holding = discipline.Holding,
            IsYearLong = discipline.IsYearLong,
            IsOpen = discipline.IsOpen,
        };
}
