using Server.Models.Interfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Services
{
    public class AcademiciansService
    {
        private readonly IAcademicianRepository _academicianRepository;

        public AcademiciansService(IAcademicianRepository academicianRepository)
        {
            _academicianRepository = academicianRepository;
        }

        public async Task<int> GetCount(uint facultyId, byte? roleFilter)
        {
            var academiciansCount = roleFilter is null ? await _academicianRepository.GetCount(facultyId) :
                await _academicianRepository.GetCount(facultyId, roleFilter.Value);

            return academiciansCount;
        }

        public async Task<IEnumerable<UserFullInfoDto>> GetAcademicians(int pageNumber, int pageSize, uint facultyId, byte? roleFilter)
        {
            var workers = roleFilter is null ? await _academicianRepository.GetWorkers(pageNumber, pageSize, facultyId) :
                await _academicianRepository.GetWorkers(pageNumber, pageSize, facultyId, roleFilter.Value);

            return workers.Select(UserMapper.MapToUserFullInfoDto);
        }
    }
}
