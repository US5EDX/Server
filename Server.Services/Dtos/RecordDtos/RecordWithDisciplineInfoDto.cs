using Server.Models.Enums;

namespace Server.Services.Dtos.RecordDtos;

public class RecordShortDisciplineInfoDto
{
    public uint RecordId { get; set; }

    public Semesters ChosenSemester { get; set; }

    public RecordStatus Approved { get; set; }

    public uint DisciplineId { get; set; }

    public string DisciplineCode { get; set; } = null!;

    public string DisciplineName { get; set; } = null!;

    public bool IsYearLong { get; set; }
}

public class RecordWithDisciplineInfoDto : RecordShortDisciplineInfoDto
{
    public string Course { get; set; } = null!;

    public EduLevels EduLevel { get; set; }

    public Semesters Semester { get; set; }

    public int SubscribersCount { get; set; }

    public bool IsOpen { get; set; }
}
