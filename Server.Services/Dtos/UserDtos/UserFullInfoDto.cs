using Server.Models.Enums;

namespace Server.Services.Dtos.UserDtos;

public class UserFullInfoDto
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Roles Role { get; set; }

    public string FullName { get; set; } = null!;

    public FacultyDto Faculty { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string Position { get; set; } = null!;

    public uint StudentGroupId { get; set; }
}
