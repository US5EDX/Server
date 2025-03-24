using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IRecordRepository
    {
        Task<uint> Add(Record record);
        Task<bool?> Delete(uint recordId);
        Task<IEnumerable<Record>> GetRecordsWithStudentInfo(uint disciplineId, byte semester);
        Task<IEnumerable<Record>> GetStudentRecordsByYears(byte[] studentId, IEnumerable<short> years);
        Task<uint?> Update(Record record);
        Task<bool> UpdateStatus(uint recordId);
    }
}