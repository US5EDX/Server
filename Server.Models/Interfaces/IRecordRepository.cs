using Server.Models.Enums;
using Server.Models.Models;

namespace Server.Models.Interfaces;

public interface IRecordRepository
{
    Task<uint> Add(Record record);
    Task<uint?> Update(Record record);
    Task<bool> UpdateStatus(uint recordId, RecordStatus status);
    Task<DeleteResultEnum> Delete(uint recordId);
    Task<uint> AddRecordOrThrow(Record record, EduLevels eduLevel, byte courseMask, int choicesCount);
    Task<uint> UpdateRecordOrThrow(Record record, EduLevels eduLevel, byte courseMask);
}