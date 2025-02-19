using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Dtos
{
    public class RecordWithStudentInfoDto
    {
        public string StudentId { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string FacultyName { get; set; }

        public string GroupCode { get; set; }

        public byte Semester { get; set; }

        public bool Approved { get; set; }
    }
}
