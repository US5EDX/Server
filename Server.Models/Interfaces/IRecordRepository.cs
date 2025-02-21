using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IRecordRepository
    {
        Task<IEnumerable<Record>> GetRecordsWithStudentInfo(uint disciplineId, byte semester);
        Task<IEnumerable<Record>> GetStudentRecordsByYears(byte[] studentId, IEnumerable<short> years);
    }
}