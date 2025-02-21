using Server.Models.Interfaces;
using Server.Models.Models;
using Server.Services.Dtos;
using Server.Services.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Services
{
    public class RecordsService
    {
        private IRecordRepository _recordRepository;
        private IHoldingRepository _holdingRepository;

        public RecordsService(IRecordRepository recordRepository, IHoldingRepository holdingRepository)
        {
            _recordRepository = recordRepository;
            _holdingRepository = holdingRepository;
        }

        public async Task<IEnumerable<RecordWithStudentInfoDto>> GetSignedStudents(uint disciplineId, byte semester)
        {
            var records = await _recordRepository.GetRecordsWithStudentInfo(disciplineId, semester);

            return records.Select(RecordMapper.MapToRecordWithStudentInfo);
        }

        public async Task<IEnumerable<StudentYearRecordsDto>> GetByStudentIdAndCourse(string studentId, byte course)
        {
            var isSuccess = Ulid.TryParse(studentId, out Ulid ulidStudentId);

            if (!isSuccess)
                throw new InvalidCastException("Невалідний Id");

            var byteStudentId = ulidStudentId.ToByteArray();

            int limit = ChooseLimit(course);

            if (limit < 1)
                throw new InvalidCastException("Неіснуючий курс");

            var years = await _holdingRepository.GetLastNYears(limit);

            var records = await _recordRepository.GetStudentRecordsByYears(byteStudentId, years);

            var groupedRecords = records.GroupBy(r => r.Holding).ToDictionary(pair => pair.Key, pair => pair.ToList());

            foreach (var year in years)
                groupedRecords.TryAdd(year, new List<Record>(0));

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

        private int ChooseLimit(byte course)
        {
            if (course < 4)
                return course;

            if (course == 4)
                return 3;

            if (course < 7)
                return course - 4;

            if (course < 12)
                return course - 6;

            if (course == 12)
                return 5;

            return -1;
        }
    }
}
