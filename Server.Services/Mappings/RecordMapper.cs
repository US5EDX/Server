using Server.Models.Models;
using Server.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Mappings
{
    public class RecordMapper
    {
        public static RecordWithStudentInfoDto MapToRecordWithStudentInfo(Record record)
        {
            return new RecordWithStudentInfoDto()
            {
                StudentId = new Ulid(record.StudentId).ToString(),
                Email = record.Student.User.Email,
                FullName = record.Student.FullName,
                FacultyName = record.Student.FacultyNavigation.FacultyName,
                GroupCode = record.Student.GroupNavigation.GroupCode,
                Semester = record.Semester,
                Approved = record.Approved
            };
        }
    }
}
