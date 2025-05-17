using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos.StudentDtos;

namespace Server.Data.Repositories.StudentRepositories;

public class StudentDtoRepository(ElCoursesDbContext context) : IStudentDtoRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<IReadOnlyList<StudentWithAllRecordsInfo>> GetWithAllRecordsByGroupId(uint groupId) =>
        await _context.Students
            .Include(st => st.Records)
            .Where(st => st.Group == groupId)
            .Select(st => new StudentWithAllRecordsInfo
            {
                FullName = st.FullName,
                Records = st.Records
                    .Select(r => new RecordDisciplineInfo
                    {
                        DisciplineCode = r.Discipline.DisciplineCode,
                        DisciplineName = r.Discipline.DisciplineName,
                        EduYear = r.Holding,
                        Semester = r.Semester,
                    }),
            }).ToListAsync();
}
