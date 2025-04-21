using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos;

namespace Server.Data.Repositories
{
    public class StudentDtoRepository : IStudentDtoRepository
    {
        private readonly ElCoursesDbContext _context;

        public StudentDtoRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentWithAllRecordsInfo>> GetWithAllRecordsByGroupId(uint groupId)
        {
            return await _context.Students
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
    }
}
