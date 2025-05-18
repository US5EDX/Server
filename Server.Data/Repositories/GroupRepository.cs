using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Models;
using Server.Services.Services.StaticServices;

namespace Server.Data.Repositories;

public class GroupRepository(ElCoursesDbContext context) : IGroupRepository
{
    private readonly ElCoursesDbContext _context = context;

    public async Task<Group?> GetById(uint id) => await _context.Groups.FindAsync(id);

    public async Task<IReadOnlyList<Group>> GetByFacultyAndCuratorId(uint facultyId, byte[]? curatorId)
    {
        var query = _context.Groups
            .Include(g => g.Specialty)
            .Include(g => g.Curator)
            .Where(g => g.Specialty.FacultyId == facultyId);

        if (curatorId is not null)
            query = query.Where(g => g.CuratorId != null && g.CuratorId.SequenceEqual(curatorId));

        return await query.ToListAsync();
    }

    public async Task<Group?> GetGroupInfoByStudentId(byte[] studentId) =>
        await _context.Students
            .Where(s => s.StudentId.SequenceEqual(studentId))
            .Select(s => new Group()
            {
                AdmissionYear = s.GroupNavigation.AdmissionYear,
                DurationOfStudy = s.GroupNavigation.DurationOfStudy,
                HasEnterChoise = s.GroupNavigation.HasEnterChoise,
                EduLevel = s.GroupNavigation.EduLevel,
                Nonparsemester = s.GroupNavigation.Nonparsemester,
                Parsemester = s.GroupNavigation.Parsemester,
                ChoiceDifference = s.GroupNavigation.ChoiceDifference,
            })
            .SingleOrDefaultAsync();

    public async Task<Group> Add(Group group)
    {
        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();

        return await GetByIdFullInfo(group.GroupId);
    }

    public async Task<Group?> Update(Group group)
    {
        var existingGroup = await _context.Groups.SingleOrDefaultAsync(g => g.GroupId == group.GroupId);

        if (existingGroup is null)
            return null;

        existingGroup.GroupCode = group.GroupCode;
        existingGroup.SpecialtyId = group.SpecialtyId;
        existingGroup.EduLevel = group.EduLevel;
        existingGroup.DurationOfStudy = group.DurationOfStudy;
        existingGroup.AdmissionYear = group.AdmissionYear;
        existingGroup.Nonparsemester = group.Nonparsemester;
        existingGroup.Parsemester = group.Parsemester;
        existingGroup.HasEnterChoise = group.HasEnterChoise;
        existingGroup.ChoiceDifference = group.ChoiceDifference;
        existingGroup.CuratorId = group.CuratorId;

        await _context.SaveChangesAsync();

        return await GetByIdFullInfo(existingGroup.GroupId);
    }

    public async Task<DeleteResultEnum> Delete(uint groupId, DateTime currDate)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);

        bool hasDependencies = await _context.Groups
                .Where(g => g.GroupId == groupId && g.DurationOfStudy >=
                ((currDate.Month > 6 ? currDate.Year : currDate.Year - 1) - g.AdmissionYear + 1)) // after june as current edu year
                .AnyAsync(g => g.Students.Any());

        if (hasDependencies) return DeleteResultEnum.HasDependencies;

        var existingGroup = await _context.Groups.SingleOrDefaultAsync(g => g.GroupId == groupId);

        if (existingGroup is null) return DeleteResultEnum.ValueNotFound;

        await _context.Students.Where(s => s.Group == existingGroup.GroupId).Select(s => s.User).ExecuteDeleteAsync();

        _context.Groups.Remove(existingGroup);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return DeleteResultEnum.Success;
    }

    public async Task<bool> DeleteGraduated(uint facultyId, DateTime currDate)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);

        var forDelete = await _context.Groups.Include(g => g.Specialty).Where(g => g.Specialty.FacultyId == facultyId &&
        g.DurationOfStudy < ((currDate.Month > 6 ? currDate.Year : currDate.Year - 1) - g.AdmissionYear + 1)).ToListAsync();

        if (forDelete.Count == 0) return false;

        var groupsIdSet = forDelete.Select(g => g.GroupId).ToList();

        await _context.Users.Where(u => _context.Students
            .Any(s => s.StudentId.SequenceEqual(u.UserId) && groupsIdSet.Contains(s.Group))).ExecuteDeleteAsync();

        _context.Groups.RemoveRange(forDelete);

        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return true;
    }

    private async Task<Group> GetByIdFullInfo(uint groupId) =>
        await _context.Groups
            .Include(g => g.Specialty)
            .Include(g => g.Curator)
            .SingleAsync(g => g.GroupId == groupId);
}
