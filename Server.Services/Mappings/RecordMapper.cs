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

        public static RecordDiscAndStatusPairDto MapToPairDto(Record record)
        {
            return new RecordDiscAndStatusPairDto()
            {
                CodeName = $"{record.Discipline.DisciplineCode} {record.Discipline.DisciplineName}",
                Approved = record.Approved
            };
        }

        public static RecordWithDisciplineInfoDto MapToRecordWithDisciplineDto(Record record)
        {
            return new RecordWithDisciplineInfoDto()
            {
                RecordId = record.RecordId,
                ChosenSemester = record.Semester,
                Approved = record.Approved,
                DisciplineId = record.Discipline.DisciplineId,
                DisciplineCode = record.Discipline.DisciplineCode,
                DisciplineName = record.Discipline.DisciplineName,
                Course = record.Discipline.Course,
                EduLevel = record.Discipline.EduLevel,
                Semester = record.Discipline.Semester,
                SubscribersCount = record.Discipline.SubscribersCount,
                IsOpen = record.Discipline.IsOpen,
            };
        }

        public static Record MapToRecord(RecordRegistryDto record)
        {
            return new Record()
            {
                RecordId = record.RecordId,
                StudentId = Ulid.Parse(record.StudentId).ToByteArray(),
                DisciplineId = record.DisciplineId,
                Semester = record.Semester,
                Holding = record.Holding,
                Approved = false,
            };
        }
    }
}
