using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;

namespace Server.Services.Services;

public class FacultiesService(IFacultyRepository facultyRepository)
{
    public async Task<IEnumerable<FacultyDto>> GetAll()
    {
        var faculties = await facultyRepository.GetAll();
        return faculties.Select(FacultyMapper.MapToFacultyDto);
    }

    public async Task<FacultyDto> AddFaculty(string facultyName)
    {
        var newFaculty = await facultyRepository.Add(new Models.Models.Faculty() { FacultyName = facultyName });
        return FacultyMapper.MapToFacultyDto(newFaculty);
    }

    public async Task<FacultyDto> UpdateOrThrow(FacultyDto faculty)
    {
        var isSuccess = await facultyRepository.Update(FacultyMapper.MapToFaculty(faculty));

        if (!isSuccess) throw new NotFoundException("Факультет не знайдено");

        return faculty;
    }

    public async Task DeleteOrThrow(uint facultyId)
    {
        var result = await facultyRepository.Delete(facultyId);
        result.ThrowIfFailed("Вказаний факультет не знайдено", "Неможливо видалити, оскільки до факультету є прив'язані дані");
    }
}
