using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;
using System.Data;

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

        public async Task<uint> AddRecord(Record record, int choicesCount)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

            var checkResult = await CheckRepeat(
                record.StudentId, record.DisciplineId, record.Holding, record.Semester);

            if (!checkResult)
                throw new InvalidOperationException("Дублювання вибору.");

            var discipline = await GetNewDiscipline(record.DisciplineId, record.Holding, record.Semester);

            var existingCount = await _context.Records
                .FromSql($@"SELECT * FROM Record WHERE studentId = {record.StudentId} 
AND holding = {record.Holding} AND semester = {record.Semester} FOR UPDATE")
                .CountAsync();

            if (existingCount >= choicesCount)
                throw new InvalidOperationException("Спроба обрати більше дисциплін, ніж дозволено.");

            if (await IsNeedLock(discipline.DisciplineId, record.Holding, record.Semester, discipline.MaxCount))
                discipline.IsOpen = false;

            await _context.Records.AddAsync(record);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return record.RecordId;
        }

        public async Task<uint> UpdateRecord(Record record)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

            var existingRecord = await _context.Records
                .FromSql($@"SELECT * FROM Record WHERE recordId = {record.RecordId} AND studentId = {record.StudentId} 
AND holding = {record.Holding} AND semester = {record.Semester} AND approved = FALSE FOR UPDATE").FirstOrDefaultAsync();

            if (existingRecord is null)
                throw new InvalidOperationException("Неправильні дані");

            var checkResult = await CheckRepeat(
                existingRecord.StudentId, record.DisciplineId, existingRecord.Holding, existingRecord.Semester);

            if (!checkResult)
                throw new InvalidOperationException("Дублювання вибору.");

            var discipline = await GetNewDiscipline(record.DisciplineId, existingRecord.Holding, existingRecord.Semester);

            if (await IsNeedLock(discipline.DisciplineId, existingRecord.Holding, existingRecord.Semester, discipline.MaxCount))
                discipline.IsOpen = false;

            existingRecord.DisciplineId = record.DisciplineId;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return existingRecord.RecordId;
        }

        private async Task<bool> CheckRepeat(byte[] studentId, uint disciplineId, short holding, byte semester)
        {
            var duplicates = await _context.Records
                .FromSql($@"SELECT * FROM Record 
            WHERE studentId = {studentId} AND disciplineId = {disciplineId} AND holding = {holding} FOR UPDATE")
                .Include(r => r.Discipline)
                .Select(selector => new
                {
                    selector.Semester,
                })
                .ToListAsync();

            if (duplicates.Count == 0)
                return true;

            return duplicates.Any(r => r.Semester == semester); //one more condition later for allowed two semesters choice
        }

        private async Task<Discipline> GetNewDiscipline(uint disciplineId, short holding, byte semester)
        {
            var discipline = await _context.Disciplines
                .FromSql($@"SELECT * FROM Discipline WHERE disciplineId = {disciplineId} AND holding = {holding} 
AND (semester = 0 OR semester = {semester}) AND isOpen = TRUE FOR UPDATE")
                .FirstOrDefaultAsync();

            if (discipline is null)
                throw new InvalidOperationException("Неправильна дисципліна");

            return discipline;
        }

        private async Task<bool> IsNeedLock(uint disciplineId, short holding, byte semester, int maxCount)
        {
            var studentsCount = await _context.Records
                .Where(r => r.DisciplineId == disciplineId
                && r.Holding == holding && r.Semester == semester)
                .CountAsync();

            return (studentsCount + 1) == maxCount;
        }
    }
}
