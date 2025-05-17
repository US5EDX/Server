using Server.Models.Enums;

namespace Server.Services.Dtos.StudentDtos;

public class StudentWithAllRecordsInfo
{
    public string FullName { get; set; } = null!;

    public IEnumerable<RecordDisciplineInfo> Records { get; set; } = null!;
}

public class RecordDisciplineInfo
{
    public string DisciplineCode { get; set; } = null!;

    public string DisciplineName { get; set; } = null!;

    public short EduYear { get; set; }

    public Semesters Semester { get; set; }
}
