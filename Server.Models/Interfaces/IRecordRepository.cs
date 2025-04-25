using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IRecordRepository
    {
        Task<uint> Add(Record record);
        Task<bool?> Delete(uint recordId);
        Task<IEnumerable<Record>> GetRecordsWithStudentInfo(uint disciplineId, byte semester);
        Task<uint?> Update(Record record);
        Task<bool> UpdateStatus(uint recordId);
        Task<uint> AddRecord(Record record, int choicesCount);
        Task<uint> UpdateRecord(Record record);
    }
}