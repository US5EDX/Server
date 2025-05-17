using Server.Services.Dtos.RecordDtos;

namespace Server.Services.Dtos.StudentDtos;

public class StudentWithRecordsDto
{
    public string StudentId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public bool Headman { get; set; }

    public List<RecordDiscAndStatusPairDto> Nonparsemester { get; set; } = null!;

    public List<RecordDiscAndStatusPairDto> Parsemester { get; set; } = null!;
}
