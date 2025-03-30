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
            existingGroup.Course = group.Course;
            existingGroup.EduLevel = group.EduLevel;
            existingGroup.Nonparsemester = group.Nonparsemester;
            existingGroup.Parsemester = group.Parsemester;
            existingGroup.CuratorId = group.CuratorId;

            await _context.SaveChangesAsync();

            return await GetByIdFullInfo(existingGroup.GroupId);
        }

        public async Task<bool?> Delete(uint groupId)
        {
            bool hasDependencies = await _context.Groups
                    .Where(g => g.GroupId == groupId && g.Course != 0).AnyAsync();

            if (hasDependencies)
                return null;

            var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (existingGroup is null)
                return false;

            _context.Groups.Remove(existingGroup);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task UpdateGroupsCourse(uint facultyId)
        {
            await _context.Groups
                        .Where(g => g.Specialty.FacultyId == facultyId)
                        .ExecuteUpdateAsync(setters => setters
                        .SetProperty(g => g.Course, g => g.Course == 0 ||
                        (g.Course + 1 == 5 || g.Course + 1 == 7 || g.Course + 1 == 13) ? 0 : g.Course + 1));
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
