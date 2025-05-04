using Server.Models.Interfaces;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;

namespace Server.Services.Services
{
    public class RecordsService
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IRecordDtoRepository _recordDtoRepository;
        private readonly IHoldingRepository _holdingRepository;
        private readonly IGroupRepository _groupRepository;

        private readonly TimeZoneInfo _kiyvTimeZone;

        public RecordsService(IRecordRepository recordRepository, IRecordDtoRepository recordDtoRepository,
            IHoldingRepository holdingRepository, IGroupRepository groupRepository)
        {
            _recordRepository = recordRepository;
            _recordDtoRepository = recordDtoRepository;
            _holdingRepository = holdingRepository;
            _groupRepository = groupRepository;

            _kiyvTimeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
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

        public async Task<IEnumerable<RecordShortDisciplineInfoDto>> GetWithDisciplineShort(string studentId, short year)
        {
            var isSuccess = Ulid.TryParse(studentId, out Ulid ulidStudentId);
            if (!isSuccess)
                throw new InvalidCastException("Невалідний Id");
            var byteStudentId = ulidStudentId.ToByteArray();
            return await _recordDtoRepository.GetWithDisciplineShort(byteStudentId, year);
        }

        public async Task<IEnumerable<StudentYearsRecordsDto>> GetMadeChoices(string studentId)
        {
            var isSuccess = Ulid.TryParse(studentId, out Ulid ulidStudentId);
            if (!isSuccess)
                throw new InvalidCastException("Невалідний Id");

            var byteStudentId = ulidStudentId.ToByteArray();
            return await _recordDtoRepository.GetMadeChoices(byteStudentId);
        }

        public async Task<RecordWithDisciplineInfoDto> AddRecord(RecordRegistryDto record)
        {
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

        public async Task<bool> UpdateStatus(uint recordId, byte status)
        {
            return await _recordRepository.UpdateStatus(recordId, status);
        }

        public async Task<uint> RegisterRecord(RecordRegistryWithoutStudent inRecord, string studentId)
        {
            var kiyvDate = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(DateTime.UtcNow, _kiyvTimeZone));

            var holding = await _holdingRepository.GetLastWithDates();

            if (holding is null || holding.StartDate > kiyvDate || holding.EndDate < kiyvDate ||
                inRecord.Holding != holding.EduYear)
                throw new InvalidOperationException("Вибір недоступний");

            if (!Ulid.TryParse(studentId, out Ulid ulidStudentId))
                throw new InvalidCastException("Невалідний Id");

            var byteStudentId = ulidStudentId.ToByteArray();

            var groupInfo = await _groupRepository.GetGroupInfoByStudentId(byteStudentId);

            if (groupInfo is null)
                throw new Exception("Проблеми з id");

            var course = CalcuationService.CalculateGroupCourse(groupInfo);

            if (course == 0 || course == groupInfo.DurationOfStudy ||
                (holding.EduYear == groupInfo.AdmissionYear && !groupInfo.HasEnterChoise))
                throw new Exception("Вибір для вас не запланований");

            course += groupInfo.ChoiceDifference;

            byte courseMask = (byte)(1 << (course - ((course == 1 && groupInfo.HasEnterChoise) ? 1 : 0)));

            var record = RecordMapper.MapToRecord(inRecord);

            record.StudentId = byteStudentId;

            var res = record.RecordId == 0 ? await _recordRepository.AddRecord(record, groupInfo.EduLevel, courseMask,
                inRecord.Semester == 1 ? groupInfo.Nonparsemester : groupInfo.Parsemester) :
                await _recordRepository.UpdateRecord(record, groupInfo.EduLevel, courseMask);

            return res;
        }
    }
}
