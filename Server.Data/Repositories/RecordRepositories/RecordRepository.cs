using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Models;
using System.Data;

namespace Server.Data.Repositories.RecordRepositories;

public class RecordRepository(ElCoursesDbContext context) : IRecordRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<uint> Add(Record record)
    {
        await _context.Records.AddAsync(record);
        await _context.SaveChangesAsync();

        return record.RecordId;
    }

    public async Task<uint?> Update(Record record)
    {
        var existingRecord = await _context.Records.FindAsync(record.RecordId);

        if (existingRecord is null) return null;

        existingRecord.DisciplineId = record.DisciplineId;
        existingRecord.Approved = 0;
        await _context.SaveChangesAsync();

        return existingRecord.RecordId;
    }

    public async Task<bool> UpdateStatus(uint recordId, RecordStatus status)
    {
        var existingRecord = await _context.Records.FindAsync(recordId);

        if (existingRecord is null) return false;

        existingRecord.Approved = status;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<DeleteResultEnum> Delete(uint recordId)
    {
        bool hasDependencies = await _context.Records
                .AnyAsync(r => r.RecordId == recordId && r.Approved != 0);

        if (hasDependencies) return DeleteResultEnum.HasDependencies;

        var existingRecord = await _context.Records.FindAsync(recordId);

        if (existingRecord is null) return DeleteResultEnum.ValueNotFound;

        _context.Records.Remove(existingRecord);
        await _context.SaveChangesAsync();

        return DeleteResultEnum.Success;
    }

    public async Task<uint> AddRecordOrThrow(Record record, EduLevels eduLevel, byte courseMask, int choicesCount)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        var checkResult = await CheckRepeat(record.StudentId, record.DisciplineId, record.Holding, record.Semester);

        if (checkResult) throw new BadRequestException("Дублювання вибору.");

        var discipline = await GetNewDisciplineOrThrow(record.DisciplineId, record.Holding, eduLevel, courseMask, record.Semester);

        var existingCount = await _context.Records.FromSql($@"SELECT * FROM Record WHERE studentId = {record.StudentId} 
AND holding = {record.Holding} AND semester = {record.Semester} FOR UPDATE").CountAsync();

        if (existingCount >= choicesCount) throw new BadRequestException("Спроба обрати більше дисциплін, ніж дозволено.");

        if (await IsNeedLock(discipline.DisciplineId, record.Holding, record.Semester, discipline.MaxCount))
            discipline.IsOpen = false;

        await _context.Records.AddAsync(record);

        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return record.RecordId;
    }

    public async Task<uint> UpdateRecordOrThrow(Record record, EduLevels eduLevel, byte courseMask)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        var existingRecord = await _context.Records
            .FromSql($@"SELECT * FROM Record WHERE recordId = {record.RecordId} AND studentId = {record.StudentId} 
AND holding = {record.Holding} AND semester = {record.Semester} AND approved = {RecordStatus.NotApproved} FOR UPDATE")
            .SingleOrDefaultAsync() ?? throw new NotFoundException("Неправильні дані про запис");

        var checkResult = await CheckRepeat(existingRecord.StudentId, record.DisciplineId,
            existingRecord.Holding, existingRecord.Semester);

        if (checkResult) throw new BadRequestException("Дублювання вибору.");

        var discipline = await GetNewDisciplineOrThrow(record.DisciplineId, existingRecord.Holding,
            eduLevel, courseMask, existingRecord.Semester);

        if (await IsNeedLock(discipline.DisciplineId, existingRecord.Holding, existingRecord.Semester, discipline.MaxCount))
            discipline.IsOpen = false;

        existingRecord.DisciplineId = record.DisciplineId;

        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return existingRecord.RecordId;
    }

    private async Task<bool> CheckRepeat(byte[] studentId, uint disciplineId, short holding, Semesters semester)
    {
        var duplicates = await _context.Records
            .FromSql($@"SELECT * FROM Record 
            WHERE studentId = {studentId} AND disciplineId = {disciplineId} AND holding = {holding} FOR UPDATE")
            .Include(r => r.Discipline)
            .Select(selector => new
            {
                selector.Semester,
                IsYearDisciplineDuration = selector.Discipline.IsYearLong
            })
            .ToListAsync();

        if (duplicates.Count == 0) return false;

        return duplicates.Any(r => r.Semester == semester || !r.IsYearDisciplineDuration);
    }

    private async Task<Discipline> GetNewDisciplineOrThrow(uint disciplineId, short holding, EduLevels eduLevel,
        byte courseMask, Semesters semester) =>
        await _context.Disciplines
            .FromSql($@"SELECT * FROM Discipline WHERE disciplineId = {disciplineId} AND holding = {holding} 
AND eduLevel = {eduLevel} AND (course & {courseMask}) > 0 AND (semester = {Semesters.Both} OR semester = {semester}) 
AND isOpen = TRUE FOR UPDATE")
            .SingleOrDefaultAsync() ?? throw new NotFoundException("Неправильна дисципліна");

    private async Task<bool> IsNeedLock(uint disciplineId, short holding, Semesters semester, int maxCount)
    {
        var studentsCount = await _context.Records
            .Where(r => r.DisciplineId == disciplineId && r.Holding == holding && r.Semester == semester)
            .CountAsync();

        return studentsCount + 1 == maxCount;
    }
}
