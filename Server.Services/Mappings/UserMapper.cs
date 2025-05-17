using Server.Models.Enums;
using Server.Models.Models;
using Server.Services.Converters;
using Server.Services.Dtos.StudentDtos;
using Server.Services.Dtos.UserDtos;
using Server.Services.Dtos.WorkerDtos;
using Server.Services.Services.StaticServices;

namespace Server.Services.Mappings;

public static class UserMapper
{
    public static WorkerInfoDto MapToWorkerInfoDto(Worker worker) =>
        new()
        {
            FullName = worker.FullName,
            Faculty = FacultyMapper.MapToFacultyDto(worker.FacultyNavigation),
            Department = worker.Department,
            Position = worker.Position
        };

    public static StudentInfoDto MapToStudentInfoDto(Student student) =>
        new()
        {
            FullName = student.FullName,
            Group = GroupMapper.MapToGroupDto(student.GroupNavigation),
            Faculty = FacultyMapper.MapToFacultyDto(student.FacultyNavigation),
            Headman = student.Headman
        };

    public static UserFullInfoDto MapToUserFullInfoDto(User user) =>
        new()
        {
            Id = UlidConverter.ByteIdToString(user.UserId),
            Email = user.Email,
            Role = user.Role,

            FullName = user.Worker is not null ? user.Worker.FullName : user.Student.FullName,

            Faculty = FacultyMapper.MapToFacultyDto(user.Worker is not null ?
            user.Worker.FacultyNavigation : user.Student.FacultyNavigation),

            Department = user.Worker is not null ? user.Worker.Department : user.Student.GroupNavigation.GroupCode,

            Position = user.Worker is not null ? user.Worker.Position :
            ("студент - " + CalcuationService.CalculateGroupCourse(user.Student.GroupNavigation)),

            StudentGroupId = user.Student is not null ? user.Student.Group : 0
        };

    public static User MapToUserFromWorkerWithoutId(WorkerRegistryDto worker) =>
        new()
        {
            Email = worker.Email,
            Role = worker.Role,
            Worker = new Worker()
            {
                FullName = worker.FullName,
                Faculty = worker.FacultyId,
                Department = worker.Department,
                Position = worker.Position
            }
        };

    public static User MapToUserFromStudentWithoutId(StudentRegistryDto student) =>
        new()
        {
            Email = student.Email,
            Role = Roles.Student,
            Student = new Student()
            {
                FullName = student.FullName,
                Faculty = student.Faculty,
                Group = student.Group,
                Headman = student.Headman,
            }
        };

    public static StudentRegistryDto MapToStudentRegistry(User user) =>
        new()
        {
            StudentId = UlidConverter.ByteIdToString(user.Student.StudentId),
            Email = user.Email,
            FullName = user.Student.FullName,
            Faculty = user.Student.Faculty,
            Group = user.Student.Group,
            Headman = user.Student.Headman,
        };
}
