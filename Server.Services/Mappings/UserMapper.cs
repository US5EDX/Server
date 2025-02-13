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

        public static WorkerFullInfoDto MapToWorkerFullInfoDto(User worker)
        {
            return new WorkerFullInfoDto()
            {
                Id = new Ulid(worker.UserId).ToString(),
                Email = worker.Email,
                Role = worker.Role,
                FullName = worker.Worker.FullName,
                Faculty = FacultyMapper.MapToFacultyDto(worker.Worker.FacultyNavigation),
                Department = worker.Worker.Department,
                Position = worker.Worker.Position,
                Group = worker.Worker.GroupNavigation is null ? null : GroupMapper.MapToGroupShortDto(worker.Worker.GroupNavigation)
            };
        }

        public static User MapToUserWithoutId(WorkerFullInfoDto worker)
        {
            return new User()
            {
                Email = worker.Email,
                Role = worker.Role,
                Worker = new Worker()
                {
                    FullName = worker.FullName,
                    Faculty = worker.Faculty.FacultyId,
                    Department = worker.Department,
                    Position = worker.Position,
                    Group = worker.Group?.GroupId
                }
            };
        }
    }
}
