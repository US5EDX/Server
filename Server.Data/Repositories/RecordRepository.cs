using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly ElCoursesDbContext _context;

        public RecordRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Record>> GetRecordsWithStudentInfo(uint disciplineId, byte semester)
        {
            return await _context.Records
                .Include(r => r.Student)
                .ThenInclude(s => s.User)
                .Include(r => r.Student)
                .ThenInclude(s => s.FacultyNavigation)
                .Include(r => r.Student)
                .ThenInclude(s => s.GroupNavigation)
                .Where(r => r.DisciplineId == disciplineId && r.Semester == semester)
                .ToListAsync();
        }

        public async Task<IEnumerable<Record>> GetStudentRecordsByYears(byte[] studentId, IEnumerable<short> years)
        {
            return await _context.Records.Include(r => r.Discipline)
                .Where(r => years.Contains(r.Holding) && r.StudentId == studentId).ToListAsync();
        }
    }
}
