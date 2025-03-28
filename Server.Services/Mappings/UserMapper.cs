﻿using Server.Models.Models;
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

        public static UserFullInfoDto MapToUserFullInfoDto(User user)
        {
            return new UserFullInfoDto()
            {
                Id = new Ulid(user.UserId).ToString(),
                Email = user.Email,
                Role = user.Role,

                FullName = user.Role < 4 ? user.Worker.FullName : user.Student.FullName,

                Faculty = FacultyMapper.MapToFacultyDto(user.Role < 4 ? user.Worker.FacultyNavigation : user.Student.FacultyNavigation),

                Department = user.Role < 4 ? user.Worker.Department : "-",

                Position = user.Role < 4 ? user.Worker.Position : ("студент - " + user.Student.GroupNavigation.Course),

                Group = user.Worker?.GroupNavigation is null ?
                (user.Student is null ? null : GroupMapper.MapToGroupShortDto(user.Student.GroupNavigation)) :
                    GroupMapper.MapToGroupShortDto(user.Worker.GroupNavigation)
            };
        }

        public static User MapToUserFromWorkerWithoutId(UserFullInfoDto worker)
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

        public static User MapToUserFromStudentWithoutId(StudentRegistryDto student)
        {
            return new User()
            {
                Email = student.Email,
                Role = 4,
                Student = new Student()
                {
                    FullName = student.FullName,
                    Faculty = student.Faculty,
                    Group = student.Group,
                    Headman = student.Headman,
                }
            };
        }

        public static StudentRegistryDto MapToStudentRegistry(User user)
        {
            return new StudentRegistryDto()
            {
                StudentId = new Ulid(user.Student.StudentId).ToString(),
                Email = user.Email,
                FullName = user.Student.FullName,
                Faculty = user.Student.Faculty,
                Group = user.Student.Group,
                Headman = user.Student.Headman,
            };
        }
    }
}
