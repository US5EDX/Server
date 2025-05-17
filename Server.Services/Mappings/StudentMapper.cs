using Server.Models.Enums;
using Server.Models.Models;
using Server.Services.Converters;
using Server.Services.Dtos.StudentDtos;

namespace Server.Services.Mappings;

public static class StudentMapper
{
    public static StudentWithRecordsDto MapToStudentWithRecordsDto(Student student) =>
        new()
        {
            StudentId = UlidConverter.ByteIdToString(student.StudentId),
            Email = student.User.Email,
            FullName = student.FullName,
            Headman = student.Headman,
            Nonparsemester = student.Records.Where(r => r.Semester == Semesters.Fall).Select(RecordMapper.MapToPairDto).ToList(),
            Parsemester = student.Records.Where(r => r.Semester == Semesters.Spring).Select(RecordMapper.MapToPairDto).ToList(),
        };

    public static Student MapToStudentWithUserWithoutId(StudentRegistryDto student) =>
        new()
        {
            FullName = student.FullName,
            Faculty = student.Faculty,
            Group = student.Group,
            Headman = student.Headman,
            User = new User() { Email = student.Email }
        };

    public static StudentRegistryDto MapToStudentRegistryDto(Student student) =>
        new()
        {
            StudentId = UlidConverter.ByteIdToString(student.StudentId),
            Email = student.User.Email,
            FullName = student.FullName,
            Faculty = student.Faculty,
            Group = student.Group,
            Headman = student.Headman,
        };
}