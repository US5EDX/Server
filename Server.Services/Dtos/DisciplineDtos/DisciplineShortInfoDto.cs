namespace Server.Services.Dtos.DisciplineDtos;

public class DisciplineShortInfoDto
{
    public uint DisciplineId { get; set; }

    public string DisciplineCodeName { get; set; } = null!;

    public bool IsYearLong { get; set; }
}
