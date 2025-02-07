using Server.Data.Repositories;
using Server.Services.Dtos;
using Server.Services.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Services
{
    public class FacultiesService
    {
        private readonly IFacultyRepository _facultyRepository;

        public FacultiesService(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }

        public async Task<IEnumerable<FacultyDto>> GetAll()
        {
            var faculties = await _facultyRepository.GetAll();
            return faculties.Select(FacultyMapper.MapToFacultyDto);
        }

        public async Task<FacultyDto> AddFaculty(string facultyName)
        {
            var newFaculty = await _facultyRepository.Add(new Models.Models.Faculty() { FacultyName = facultyName });
            return FacultyMapper.MapToFacultyDto(newFaculty);
        }

        public async Task<FacultyDto?> UpdateFaculty(FacultyDto faculty)
        {
            var updatedFaculty = await _facultyRepository.Update(FacultyMapper.MapToFaculty(faculty));

            if (updatedFaculty is null)
                return null;

            return FacultyMapper.MapToFacultyDto(updatedFaculty);
        }

        public async Task<bool?> DeleteFaculty(uint facultyId)
        {
            return await _facultyRepository.Delete(facultyId);
        }
    }
}
