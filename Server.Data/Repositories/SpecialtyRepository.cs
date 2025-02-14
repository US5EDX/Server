using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Models.Interfaces;
using Server.Models.Models;

namespace Server.Data.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly ElCoursesDbContext _context;

        public SpecialtyRepository(ElCoursesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Specialty>> GetByFacultyId(uint facultyId)
        {
            return await _context.Specialties.Where(f => f.FacultyId == facultyId).ToListAsync();
        }

        public async Task<Specialty> Add(Specialty specialty)
        {
            await _context.Specialties.AddAsync(specialty);
            await _context.SaveChangesAsync();

            return specialty;
        }

        public async Task<Specialty?> Update(Specialty specialty)
        {
            var existingSpecialty = await _context.Specialties.FirstOrDefaultAsync(s => s.SpecialtyId == specialty.SpecialtyId);

            if (existingSpecialty is null)
                return null;

            existingSpecialty.SpecialtyName = specialty.SpecialtyName;
            await _context.SaveChangesAsync();

            return existingSpecialty;
        }

        public async Task<bool?> Delete(uint specialtyId)
        {
            bool hasDependencies = await _context.Specialties
                    .Where(s => s.SpecialtyId == specialtyId)
                    .AnyAsync(s =>
                        s.Disciplines.Any() ||
                        s.Groups.Any());

            if (hasDependencies)
                return null;

            var existingSpecialty = await _context.Specialties.FirstOrDefaultAsync(s => s.SpecialtyId == specialtyId);

            if (existingSpecialty is null)
                return false;

            _context.Specialties.Remove(existingSpecialty);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
