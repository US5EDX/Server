using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;
using Server.Services.Dtos;
using Server.Services.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly ElCoursesDbContext _context;

        public FacultyRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Faculty>> GetAll()
        {
            return await _context.Faculties.ToListAsync();
        }

        public async Task<Faculty> Add(Faculty faculty)
        {
            await _context.Faculties.AddAsync(faculty);
            await _context.SaveChangesAsync();

            return faculty;
        }

        public async Task<Faculty?> Update(Faculty faculty)
        {
            var existingFaculty = await _context.Faculties.FirstOrDefaultAsync(f => f.FacultyId == faculty.FacultyId);

            if (existingFaculty is null)
                return null;

            existingFaculty.FacultyName = faculty.FacultyName;
            await _context.SaveChangesAsync();

            return existingFaculty;
        }

        public async Task<bool?> Delete(uint facultyId)
        {
            bool hasDependencies = await _context.Faculties
                    .Where(f => f.FacultyId == facultyId)
                    .AnyAsync(f =>
                        f.Disciplines.Any() ||
                        f.Specialties.Any() ||
                        f.Students.Any() ||
                        f.Workers.Any());

            if (hasDependencies)
                return null;

            var existingFaculty = await _context.Faculties.FirstOrDefaultAsync(f => f.FacultyId == facultyId);

            if (existingFaculty is null)
                return false;

            _context.Faculties.Remove(existingFaculty);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
