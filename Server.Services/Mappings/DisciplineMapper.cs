using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings
{
    public class DisciplineMapper
    {
        public static DisciplineFullInfoDto MapToDisciplineFullInfoDto(Discipline discipline)
        {
            return new DisciplineFullInfoDto()
            {
                DisciplineId = discipline.DisciplineId,
                DisciplineCode = discipline.DisciplineCode,
                CatalogType = discipline.CatalogType,
                Faculty = FacultyMapper.MapToFacultyDto(discipline.Faculty),
                Specialty = discipline.Specialty is null ? null : SpecialtyMapper.MapToSpecialtyDto(discipline.Specialty),
                DisciplineName = discipline.DisciplineName,
                EduLevel = discipline.EduLevel,
                Course = discipline.Course,
                Semester = discipline.Semester,
                Prerequisites = discipline.Prerequisites,
                Interest = discipline.Interest,
                MaxCount = discipline.MaxCount,
                MinCount = discipline.MinCount,
                Url = discipline.Url,
                SubscribersCount = discipline.SubscribersCount,
                Holding = discipline.Holding,
                IsOpen = discipline.IsOpen,
                CreatorId = new Ulid(discipline.CreatorId).ToString()
            };
        }

        public static Discipline MapToDiscipline(DisciplineFullInfoDto discipline)
        {
            return new Discipline()
            {
                DisciplineId = discipline.DisciplineId,
                DisciplineCode = discipline.DisciplineCode,
                CatalogType = discipline.CatalogType,
                FacultyId = discipline.Faculty.FacultyId,
                SpecialtyId = discipline.Specialty?.SpecialtyId,
                DisciplineName = discipline.DisciplineName,
                EduLevel = discipline.EduLevel,
                Course = discipline.Course,
                Semester = discipline.Semester,
                Prerequisites = discipline.Prerequisites,
                Interest = discipline.Interest,
                MaxCount = discipline.MaxCount,
                MinCount = discipline.MinCount,
                Url = discipline.Url,
                SubscribersCount = discipline.SubscribersCount,
                Holding = discipline.Holding,
                IsOpen = discipline.IsOpen,
                CreatorId = Ulid.Parse(discipline.CreatorId).ToByteArray()
            };
        }
    }
}
