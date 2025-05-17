using Server.Models.Enums;

namespace Server.Services.Dtos.StudentDtos;

public class StudentYearsRecordsDto
{
    public short Holding { get; set; }

    public Semesters Semester { get; set; }

    public RecordStatus Approved { get; set; }

    public string DisciplineCode { get; set; } = null!;

    public string DisciplineName { get; set; } = null!;
}
