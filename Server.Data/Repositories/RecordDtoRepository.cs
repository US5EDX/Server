using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Models;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos;
using System.Linq.Expressions;

namespace Server.Data.Repositories
{
    public class RecordDtoRepository : IRecordDtoRepository
    {
        private readonly ElCoursesDbContext _context;

        public RecordDtoRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecordWithDisciplineInfoDto>> GetByStudentIdAndYear(byte[] studentId, short year)
        {
            return await GetAsQueryable(r => r.Holding == year && r.StudentId.SequenceEqual(studentId))
                .ToListAsync();
        }

        public async Task<RecordWithDisciplineInfoDto?> GetWithDisciplineById(uint recordId)
        {
            return await GetAsQueryable(r => r.RecordId == recordId)
                .FirstOrDefaultAsync();
        }

        private IQueryable<RecordWithDisciplineInfoDto> GetAsQueryable(Expression<Func<Record, bool>> wherePredicate)
        {
            return _context.Records
                .Include(r => r.Discipline)
                .Where(wherePredicate)
                .Select(r => new RecordWithDisciplineInfoDto()
                {
                    RecordId = r.RecordId,
                    ChosenSemester = r.Semester,
                    Approved = r.Approved,
                    DisciplineId = r.Discipline.DisciplineId,
                    DisciplineCode = r.Discipline.DisciplineCode,
                    DisciplineName = r.Discipline.DisciplineName,
                    Course = r.Discipline.Course,
                    EduLevel = r.Discipline.EduLevel,
                    Semester = r.Discipline.Semester,
                    SubscribersCount = _context.Records
                    .Count(rs => rs.DisciplineId == r.DisciplineId && rs.Holding == r.Holding && rs.Semester == r.Semester),
                    IsOpen = r.Discipline.IsOpen,
                });
        }
    }
}
