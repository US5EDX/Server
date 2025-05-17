using Server.Models.Enums;

namespace Server.Services.Dtos.DisciplineDtos;

public class DisciplineFullInfoDto
{
    public uint DisciplineId { get; set; }

    public string DisciplineCode { get; set; } = null!;

    public CatalogTypes CatalogType { get; set; }

    public FacultyDto Faculty { get; set; } = null!;

    public SpecialtyDto? Specialty { get; set; }

    public string DisciplineName { get; set; } = null!;

    public EduLevels EduLevel { get; set; }

    public string Course { get; set; } = null!;

    public Semesters Semester { get; set; }

    public string Prerequisites { get; set; } = null!;

    public string Interest { get; set; } = null!;

    public int MaxCount { get; set; }

    public int MinCount { get; set; }

    public string Url { get; set; } = null!;

    public short Holding { get; set; }

    public bool IsYearLong { get; set; }

    public bool IsOpen { get; set; }
}
