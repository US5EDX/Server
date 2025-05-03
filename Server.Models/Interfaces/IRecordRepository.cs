using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IRecordRepository
    {
        Task<uint> Add(Record record);
        Task<bool?> Delete(uint recordId);
        Task<IEnumerable<Record>> GetRecordsWithStudentInfo(uint disciplineId, byte semester);
        Task<uint?> Update(Record record);
        Task<bool> UpdateStatus(uint recordId, byte status);
        Task<uint> AddRecord(Record record, byte eduLevel, byte courseMask, int choicesCount);
        Task<uint> UpdateRecord(Record record, byte eduLevel, byte courseMask);
    }
}