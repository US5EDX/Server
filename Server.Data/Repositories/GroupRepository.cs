using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ElCoursesDbContext _context;

        public GroupRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<Group?> GetById(uint id)
        {
            return await _context.Groups.FindAsync(id);
        }

        public async Task<IEnumerable<Group>> GetByFacultyId(uint facultyId)
        {
            return await _context.Groups
                .Include(g => g.Specialty)
                .Include(g => g.Curator)
                .Where(g => g.Specialty.FacultyId == facultyId)
                .ToListAsync();
        }

        public async Task<Group> Add(Group group)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();

            return await GetByIdFullInfo(group.GroupId);
        }

        public async Task<Group?> Update(Group group)
        {
            var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == group.GroupId);

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
            existingGroup.CuratorId = group.CuratorId;

            await _context.SaveChangesAsync();

            return await GetByIdFullInfo(existingGroup.GroupId);
        }

        public async Task<bool?> Delete(uint groupId)
        {
            var currDate = DateTime.Today;

            bool hasDependencies = await _context.Groups
                    .Where(g => g.GroupId == groupId && g.DurationOfStudy >=
                    ((currDate.Month > 6 ? currDate.Year : (currDate.Year - 1)) - g.AdmissionYear + 1)) // after june as current edu year
                    .AnyAsync(g => g.Students.Any());

            if (hasDependencies)
                return null;

            var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (existingGroup is null)
                return false;

            await DeleteStudentsUserInfoWithoutSave(existingGroup.GroupId);

            _context.Groups.Remove(existingGroup);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// to delete group with students full info
        /// </summary>
        private async Task DeleteStudentsUserInfoWithoutSave(uint groupId)
        {
            var students = await _context.Students
                .Where(s => s.Group == groupId)
                .Include(s => s.User)
                .ToListAsync();

            _context.Users.RemoveRange(students.Select(s => s.User));
        }

        private async Task<Group> GetByIdFullInfo(uint groupId)
        {
            return await _context.Groups
                .Include(g => g.Specialty)
                .Include(g => g.Curator)
                .FirstAsync(g => g.GroupId == groupId);
        }
    }
}
