using Server.Models.Models;
using Server.Services.Dtos.RecordDtos;
using Server.Services.Dtos.StudentDtos;

namespace Server.Services.Mappings;

public static class RecordMapper
{
    public static RecordDiscAndStatusPairDto MapToPairDto(Record record) =>
        new()
        {
            CodeName = $"{record.Discipline.DisciplineCode} {record.Discipline.DisciplineName}",
            Approved = record.Approved
        };

    public static RecordDiscAndStatusPairDto MapToPairDto(StudentYearsRecordsDto record) =>
        new()
        {
            CodeName = $"{record.DisciplineCode} {record.DisciplineName}",
            Approved = record.Approved
        };

    public static Record MapToRecord(RecordRegistryDto record) =>
        new()
        {
            RecordId = record.RecordId ?? 0,
            StudentId = Ulid.Parse(record.StudentId).ToByteArray(),
            DisciplineId = record.DisciplineId,
            Semester = record.Semester,
            Holding = record.Holding,
            Approved = Models.Enums.RecordStatus.NotApproved,
        };

    public static Record MapToRecord(RecordRegistryWithoutStudent record) =>
        new()
        {
            RecordId = record.RecordId ?? 0,
            DisciplineId = record.DisciplineId,
            Semester = record.Semester,
            Holding = record.Holding,
        };
}