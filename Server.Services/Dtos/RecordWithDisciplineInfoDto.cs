using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server.Services.Dtos
{
    public class RecordWithDisciplineInfoDto
    {
        public uint RecordId { get; set; }

        public byte ChosenSemester { get; set; }

        public bool Approved { get; set; }

        public uint DisciplineId { get; set; }

        public string DisciplineCode { get; set; }

        public string DisciplineName { get; set; }

        public string Course { get; set; }

        public byte EduLevel { get; set; }

        public byte Semester { get; set; }

        public int SubscribersCount { get; set; }

        public bool IsOpen { get; set; }
    }
}
