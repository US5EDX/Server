using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;

namespace Server.Services.Services;

public class SpecialtiesService(ISpecialtyRepository specialtyRepository)
{
    public async Task<IEnumerable<SpecialtyDto>> GetByFacultyId(uint facultyId)
    {
        var specialties = await specialtyRepository.GetByFacultyId(facultyId);
        return specialties.Select(SpecialtyMapper.MapToSpecialtyDto);
    }

    public async Task<SpecialtyDto> AddSpecialty(SpecialtyDto specialty)
    {
        if (specialty.SpecialtyId is not null || specialty.FacultyId is null) throw new BadRequestException("Невалідні вхідні дані");

        var newSpecialty = await specialtyRepository.Add(SpecialtyMapper.MapToSpecialty(specialty));
        return SpecialtyMapper.MapToSpecialtyDto(newSpecialty);
    }

    public async Task<SpecialtyDto> UpdateOrThrow(SpecialtyDto specialty)
    {
        if (specialty.SpecialtyId is null) throw new BadRequestException("Невалідні вхідні дані");

        var updatedSpecialty = await specialtyRepository.Update(SpecialtyMapper.MapToSpecialty(specialty)) ??
            throw new NotFoundException("Вказана спеціальність не знайдена");

        return SpecialtyMapper.MapToSpecialtyDto(updatedSpecialty);
    }

    public async Task DeleteOrThrow(uint specialtyId)
    {
        var result = await specialtyRepository.DeleteAsync(specialtyId);
        result.ThrowIfFailed("Вказана спеціальність не знайдена", "Неможливо видалити, оскільки до спеціальності є прив'язані дані");
    }
}
