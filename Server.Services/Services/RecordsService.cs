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
    public class RecordsService
    {
        private IRecordRepository _recordRepository;

        public RecordsService(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task<IEnumerable<RecordWithStudentInfoDto>> GetSignedStudents(uint disciplineId, byte semester)
        {
            var records = await _recordRepository.GetRecordsWithStudentInfo(disciplineId, semester);

            return records.Select(RecordMapper.MapToRecordWithStudentInfo);
        }
    }
}
