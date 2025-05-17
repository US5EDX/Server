using Server.Models.Enums;

namespace Server.Services.Dtos.DisciplineDtos;

public class DisciplineInfoForStudent
{
    public uint DisciplineId { get; set; }

    public string DisciplineCode { get; set; } = null!;

    public string DisciplineName { get; set; } = null!;

    public string Course { get; set; } = null!;

    public Semesters Semester { get; set; }

    public bool IsYearLong { get; set; }

    public FacultyDto Faculty { get; set; } = null!;
}
