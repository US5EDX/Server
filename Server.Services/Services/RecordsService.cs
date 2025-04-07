using Server.Models.Interfaces;
using Server.Models.Models;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;

namespace Server.Services.Services
{
    public class RecordsService
    {
        private IRecordRepository _recordRepository;
        private IRecordDtoRepository _recordDtoRepository;
        private IHoldingRepository _holdingRepository;
        private IGroupRepository _groupRepository;

        public RecordsService(IRecordRepository recordRepository, IRecordDtoRepository recordDtoRepository,
            IHoldingRepository holdingRepository, IGroupRepository groupRepository)
        {
            _recordRepository = recordRepository;
            _recordDtoRepository = recordDtoRepository;
            _holdingRepository = holdingRepository;
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<RecordWithStudentInfoDto>> GetSignedStudents(uint disciplineId, byte semester)
        {
            var records = await _recordRepository.GetRecordsWithStudentInfo(disciplineId, semester);

            return records.Select(RecordMapper.MapToRecordWithStudentInfo);
        }

        public async Task<IEnumerable<StudentYearRecordsDto>> GetByStudentIdAndGroupId(string studentId, uint groupId)
        {
            var isSuccess = Ulid.TryParse(studentId, out Ulid ulidStudentId);

            if (!isSuccess)
                throw new InvalidCastException("Невалідний Id");

            var byteStudentId = ulidStudentId.ToByteArray();

            var groupInfo = await _groupRepository.GetById(groupId);

            if (groupInfo == null)
                throw new Exception("Групу не знайдено");

            var lastVisibleYear = CalcuationService.CalculateLastHoldingForGroup(groupInfo);

            var firstVisibleYear = groupInfo.HasEnterChoise ? groupInfo.AdmissionYear : groupInfo.AdmissionYear + 1;

            if (lastVisibleYear < firstVisibleYear)
                return Enumerable.Empty<StudentYearRecordsDto>();

            var yearsRange = new HashSet<int>(
                Enumerable.Range(firstVisibleYear, lastVisibleYear - firstVisibleYear + 1));

            var years = (await _holdingRepository.GetYearsBySet(yearsRange)).ToHashSet();

            var records = await _recordDtoRepository.GetStudentRecordsByYears(byteStudentId, years);

            var groupedRecords = records.GroupBy(r => r.Holding).ToDictionary(pair => pair.Key, pair => pair.ToList());

            foreach (var year in years)
                groupedRecords.TryAdd(year, new List<StudentYearsRecordsDto>(0));

            return groupedRecords.OrderByDescending(gr => gr.Key).Select(pair =>
            {
                var semesterGroups = pair.Value.GroupBy(r => r.Semester)
                .ToDictionary(g => g.Key, g => g.Select(RecordMapper.MapToPairDto).ToList());

                return new StudentYearRecordsDto
                {
                    EduYear = pair.Key,
                    Nonparsemester = semesterGroups.GetValueOrDefault<byte, List<RecordDiscAndStatusPairDto>>
                    (1, new List<RecordDiscAndStatusPairDto>(0)),
                    Parsemester = semesterGroups.GetValueOrDefault<byte, List<RecordDiscAndStatusPairDto>>
                    (2, new List<RecordDiscAndStatusPairDto>(0))
                };
            });
        }

        public async Task<IEnumerable<RecordWithDisciplineInfoDto>> GetRecordsByStudentIdAndYear(string studentId, short year)
        {
            var isSuccess = Ulid.TryParse(studentId, out Ulid ulidStudentId);

            if (!isSuccess)
                throw new InvalidCastException("Невалідний Id");

            var byteStudentId = ulidStudentId.ToByteArray();

            return await _recordDtoRepository.GetByStudentIdAndYear(byteStudentId, year);
        }

        public async Task<RecordWithDisciplineInfoDto> AddRecord(RecordRegistryDto record)
        {
            record.RecordId = 0;

            var newRecordId = await _recordRepository.Add(RecordMapper.MapToRecord(record));

            return await _recordDtoRepository.GetWithDisciplineById(newRecordId);
        }

        public async Task<RecordWithDisciplineInfoDto?> UpdateRecord(RecordRegistryDto record)
        {
            var updatedRecordId = await _recordRepository.Update(RecordMapper.MapToRecord(record));

            if (updatedRecordId is null)
                return null;

            return await _recordDtoRepository.GetWithDisciplineById(updatedRecordId.Value);
        }

        public async Task<bool?> DeleteRecord(uint recordId)
        {
            return await _recordRepository.Delete(recordId);
        }

        public async Task<bool> UpdateStatus(uint recordId)
        {
            return await _recordRepository.UpdateStatus(recordId);
        }
    }
}
