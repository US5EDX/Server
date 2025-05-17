using Server.Services.Dtos.GroupDtos;

namespace Server.Services.Dtos.StudentDtos;

public class StudentInfoDto
{
    public string FullName { get; set; } = null!;

    public GroupDto Group { get; set; } = null!;

    public FacultyDto Faculty { get; set; } = null!;

    public bool Headman { get; set; }
}
