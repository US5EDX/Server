using Server.Models.Enums;

namespace Server.Services.Dtos.DisciplineDtos;

public class DisciplinePrintInfo
{
    public string DisciplineCode { get; set; } = null!;

    public string DisciplineName { get; set; } = null!;

    public int StudentsCount { get; set; }

    public string? SpecialtyName { get; set; }

    public EduLevels EduLevel { get; set; }

    public string Course { get; set; } = null!;

    public Semesters Semester { get; set; }

    public int MinCount { get; set; }

    public int MaxCount { get; set; }

    public bool IsOpen { get; set; }

    public string ColorStatus { get; set; } = null!;
}
