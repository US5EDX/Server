namespace Server.Services.Dtos.DisciplineDtos;

public class DisciplineWithSubCountDto : DisciplineFullInfoDto
{
    public int NonparsemesterCount { get; set; }

    public int ParsemesterCount { get; set; }
}
