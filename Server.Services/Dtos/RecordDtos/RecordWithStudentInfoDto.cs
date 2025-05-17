using Server.Models.Enums;

namespace Server.Services.Dtos.RecordDtos;

public class RecordWithStudentInfoDto
{
    public string StudentId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string FacultyName { get; set; } = null!;

    public string GroupCode { get; set; } = null!;

    public Semesters Semester { get; set; }

    public RecordStatus Approved { get; set; }
}
