using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings
{
    public class UserMapper
    {
        public static WorkerInfoDto MapToWorkerInfoDto(Worker worker, byte role)
        {
            return new WorkerInfoDto(role)
            {
                WorkerId = worker.WorkerId,
                FullName = worker.FullName,
                Faculty = FacultyMapper.MapToFacultyDto(worker.FacultyNavigation),
                Department = worker.Department,
                Position = worker.Position,
                Group = worker.Group
            };
        }

        public static StudentInfoDto MapToStudentInfoDto(Student student, byte role)
        {
            return new StudentInfoDto(role)
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Group = GroupMapper.MapToGroupDto(student.GroupNavigation),
                Faculty = FacultyMapper.MapToFacultyDto(student.FacultyNavigation),
                Headman = student.Headman
            };
        }
    }
}
