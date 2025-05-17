using Server.Services.Dtos.RecordDtos;

namespace Server.Services.Dtos.StudentDtos;

public class StudentYearRecordsDto
{
    public short EduYear { get; set; }

    public List<RecordDiscAndStatusPairDto> Nonparsemester { get; set; } = null!;

    public List<RecordDiscAndStatusPairDto> Parsemester { get; set; } = null!;
}
