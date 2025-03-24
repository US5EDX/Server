using Server.Models.Interfaces;
using Server.Models.Models;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Services
{
    public class DisciplinesService
    {
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly IDisciplineDtoRepository _disciplineDtoRepository;

        public DisciplinesService(IDisciplineRepository disciplineRepository, IDisciplineDtoRepository disciplineDtoRepository)
        {
            _disciplineRepository = disciplineRepository;
            _disciplineDtoRepository = disciplineDtoRepository;
        }

        public async Task<int> GetCount(uint facultyId, short holdingFilter, byte? catalogFilter)
        {
            var disciplineCount = catalogFilter is null ? await _disciplineRepository.GetCount(facultyId, holdingFilter) :
                await _disciplineRepository.GetCount(facultyId, holdingFilter, catalogFilter.Value);

            return disciplineCount;
        }

        public async Task<IEnumerable<DisciplineWithSubCountDto>> GetDisciplines(int pageNumber, int pageSize,
            uint facultyId, short holdingFilter, byte? catalogFilter)
        {
            var disciplines = catalogFilter is null ?
                await _disciplineDtoRepository.GetDisciplines(pageNumber, pageSize, facultyId, holdingFilter) :
                await _disciplineDtoRepository.GetDisciplines(pageNumber, pageSize, facultyId, holdingFilter, catalogFilter.Value);

            return disciplines;
        }

        public async Task<DisciplineFullInfoDto?> GetById(uint disciplineId)
        {
            var discipline = await _disciplineRepository.GetById(disciplineId);

            return discipline is null ? null : DisciplineMapper.MapToDisciplineFullInfoDto(discipline);
        }

        public async Task<IEnumerable<DisciplineShortInfoDto>> GetByCodeSearchYearEduLevelSemester(
            string code, short year, byte eduLevel, byte semester)
        {
            var disciplines = await _disciplineRepository.GetShortInfoByCodeEduYearEduLevelSemester(code, year, eduLevel, semester);

            return disciplines.Select(DisciplineMapper.MapToShortDisciplineInfo);
        }

        public async Task<DisciplineFullInfoDto> AddDiscipline(DisciplineFullInfoDto discipline, string userId)
        {
            discipline.DisciplineId = 0;
            discipline.IsOpen = true;

            var newDiscipline = DisciplineMapper.MapToDiscipline(discipline);
            newDiscipline.CreatorId = Ulid.Parse(userId).ToByteArray();

            var addedDiscipline = await _disciplineRepository.Add(newDiscipline);

            return DisciplineMapper.MapToDisciplineFullInfoDto(addedDiscipline);
        }

        public async Task<DisciplineFullInfoDto?> UpdateDiscipline(DisciplineFullInfoDto discipline)
        {
            var updatedDiscipline = await _disciplineRepository.Update(DisciplineMapper.MapToDiscipline(discipline));

            if (updatedDiscipline is null)
                return null;

            return DisciplineMapper.MapToDisciplineFullInfoDto(updatedDiscipline);
        }

        public async Task<bool?> DeleteDiscipline(uint disciplineId)
        {
            return await _disciplineRepository.Delete(disciplineId);
        }

        public async Task<bool> UpdateStatus(uint disciplineId)
        {
            return await _disciplineRepository.UpdateStatus(disciplineId);
        }
    }
}
