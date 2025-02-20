using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Dtos
{
    public class StudentWithRecordsDto
    {
        public string StudentId { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public bool Headman { get; set; }

        public List<RecordDiscAndStatusPairDto> Nonparsemester { get; set; }

        public List<RecordDiscAndStatusPairDto> Parsemester { get; set; }
    }
}
