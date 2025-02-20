using Server.Models.Models;
using Server.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Mappings
{
    public class StudentMapper
    {
        public static StudentWithRecordsDto MapToStudentWithRecordsDto(Student student)
        {
            return new StudentWithRecordsDto()
            {
                StudentId = new Ulid(student.StudentId).ToString(),
                Email = student.User.Email,
                FullName = student.FullName,
                Headman = student.Headman,
                Nonparsemester = student.Records.Where(r => r.Semester == 1).Select(RecordMapper.MapToPairDto).ToList(),
                Parsemester = student.Records.Where(r => r.Semester == 2).Select(RecordMapper.MapToPairDto).ToList(),
            };
        }

        public static Student MapToStudentWithUserWithoutId(StudentRegistryDto student)
        {
            return new Student()
            {
                FullName = student.FullName,
                Faculty = student.Faculty,
                Group = student.Group,
                Headman = student.Headman,
                User = new User()
                {
                    Email = student.Email
                }
            };
        }

        public static StudentRegistryDto MapToStudentRegistryDto(Student student)
        {
            return new StudentRegistryDto()
            {
                StudentId = new Ulid(student.StudentId).ToString(),
                Email = student.User.Email,
                FullName = student.FullName,
                Faculty = student.Faculty,
                Group = student.Group,
                Headman = student.Headman,
            };
        }
    }
}
