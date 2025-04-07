using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

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

        public async Task<uint> Add(Record record)
        {
            await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();

            return record.RecordId;
        }

        public async Task<uint?> Update(Record record)
        {
            var existingRecord = await _context.Records.FindAsync(record.RecordId);

            if (existingRecord is null)
                return null;

            existingRecord.DisciplineId = record.DisciplineId;
            existingRecord.Approved = false;
            await _context.SaveChangesAsync();

            return existingRecord.RecordId;
        }

        public async Task<bool> UpdateStatus(uint recordId)
        {
            var existingRecord = await _context.Records.FindAsync(recordId);

            if (existingRecord is null)
                return false;

            existingRecord.Approved = !existingRecord.Approved;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool?> Delete(uint recordId)
        {
            bool hasDependencies = await _context.Records
                    .Where(r => r.RecordId == recordId && r.Approved == true).AnyAsync();

            if (hasDependencies)
                return null;

            var existingRecord = await _context.Records.FindAsync(recordId);

            if (existingRecord is null)
                return false;

            _context.Records.Remove(existingRecord);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
