using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IRecordRepository
    {
        Task<Record> Add(Record record);
        Task<bool?> Delete(uint recordId);
        Task<IEnumerable<Record>> GetByStudentIdAndYear(byte[] studentId, short year);
        Task<IEnumerable<Record>> GetRecordsWithStudentInfo(uint disciplineId, byte semester);
        Task<IEnumerable<Record>> GetStudentRecordsByYears(byte[] studentId, IEnumerable<short> years);
        Task<Record?> Update(Record record);
        Task<bool> UpdateStatus(uint recordId);
    }
}