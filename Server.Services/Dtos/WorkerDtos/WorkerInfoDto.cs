namespace Server.Services.Dtos.WorkerDtos;

public class WorkerInfoDto
{
    public string FullName { get; set; } = null!;

    public FacultyDto Faculty { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string Position { get; set; } = null!;
}
