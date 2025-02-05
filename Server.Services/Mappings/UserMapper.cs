using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings
{
    public class UserMapper
    {
        public static WorkerInfoDto MapToWorkerInfoDto(Worker worker)
        {
            return new WorkerInfoDto()
            {
                FullName = worker.FullName,
                Faculty = FacultyMapper.MapToFacultyDto(worker.FacultyNavigation),
                Department = worker.Department,
                Position = worker.Position,
                Group = worker.Group
            };
        }

        public static StudentInfoDto MapToStudentInfoDto(Student student)
        {
            return new StudentInfoDto()
            {
                FullName = student.FullName,
                Group = GroupMapper.MapToGroupDto(student.GroupNavigation),
                Faculty = FacultyMapper.MapToFacultyDto(student.FacultyNavigation),
                Headman = student.Headman
            };
        }
    }
}
