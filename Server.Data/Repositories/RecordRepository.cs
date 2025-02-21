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

        public async Task<IEnumerable<Record>> GetStudentRecordsByYears(byte[] studentId, IEnumerable<short> years)
        {
            return await _context.Records.Include(r => r.Discipline)
                .Where(r => years.Contains(r.Holding) && r.StudentId.SequenceEqual(studentId)).ToListAsync();
        }

        public async Task<IEnumerable<Record>> GetByStudentIdAndYear(byte[] studentId, short year)
        {
            return await _context.Records
                .Include(r => r.Discipline)
                .Where(r => r.Holding == year && r.StudentId.SequenceEqual(studentId))
                .ToListAsync();
        }

        public async Task<Record> Add(Record record)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Records.AddAsync(record);
                await _context.SaveChangesAsync();

                await UpdateSubscribersCount(record.DisciplineId, true);

                await transaction.CommitAsync();

                return _context.Records.Include(r => r.Discipline).First(r => r.RecordId == record.RecordId);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Record?> Update(Record record)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingRecord = await _context.Records.FindAsync(record.RecordId);

                if (existingRecord is null)
                    return null;

                uint oldDisciplineId = existingRecord.DisciplineId;

                existingRecord.DisciplineId = record.DisciplineId;
                existingRecord.Approved = false;
                await _context.SaveChangesAsync();

                if (oldDisciplineId != record.DisciplineId)
                {
                    await UpdateSubscribersCount(oldDisciplineId, false);
                    await UpdateSubscribersCount(record.DisciplineId, true);
                }

                await transaction.CommitAsync();
                return _context.Records.Include(r => r.Discipline).First(r => r.RecordId == record.RecordId);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingRecord = await _context.Records.FindAsync(recordId);

                if (existingRecord is null)
                    return false;

                _context.Records.Remove(existingRecord);
                await _context.SaveChangesAsync();

                await UpdateSubscribersCount(existingRecord.DisciplineId, false);

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task UpdateSubscribersCount(uint disciplineId, bool IsAdded)
        {
            var discipline = await _context.Disciplines.FindAsync(disciplineId);

            if (discipline is not null)
            {
                discipline.SubscribersCount = IsAdded ? discipline.SubscribersCount + 1 : discipline.SubscribersCount - 1;
                await _context.SaveChangesAsync();
            }
        }
    }
}
