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
    public class GroupRepository : IGroupRepository
    {
        private readonly ElCoursesDbContext _context;

        public GroupRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> GetByFacultyId(uint facultyId)
        {
            return await _context.Groups
                .Include(g => g.Specialty)
                .Where(g => g.Specialty.FacultyId == facultyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetByFacultyIdAndCodeFilter(uint facultyId, string codeFilter)
        {
            return await _context.Groups
                .Include(g => g.Specialty)
                .Where(g => g.Specialty.FacultyId == facultyId && g.GroupCode.StartsWith(codeFilter))
                .ToListAsync();
        }
    }
}
