using Server.Models.Interfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;

namespace Server.Services.Services
{
    public class SpecialtiesService
    {
        private readonly ISpecialtyRepository _specialtyRepository;

        public SpecialtiesService(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }

        public async Task<IEnumerable<SpecialtyDto>> GetByFacultyId(uint facultyId)
        {
            var specialties = await _specialtyRepository.GetByFacultyId(facultyId);
            return specialties.Select(SpecialtyMapper.MapToSpecialtyDto);
        }

        public async Task<SpecialtyDto> AddSpecialty(SpecialtyDto specialty)
        {
            var newSpecialty = await _specialtyRepository.Add(
                new Models.Models.Specialty() { SpecialtyName = specialty.SpecialtyName, FacultyId = specialty.FacultyId.Value });

            return SpecialtyMapper.MapToSpecialtyDto(newSpecialty);
        }

        public async Task<SpecialtyDto?> UpdateSpecialty(SpecialtyDto specialty)
        {
            var updatedSpecialty = await _specialtyRepository.Update(SpecialtyMapper.MapToSpecialty(specialty));

            if (updatedSpecialty is null)
                return null;

            return SpecialtyMapper.MapToSpecialtyDto(updatedSpecialty);
        }

        public async Task<bool?> DeleteSpecialty(uint specialtyId)
        {
            return await _specialtyRepository.Delete(specialtyId);
        }
    }
}
