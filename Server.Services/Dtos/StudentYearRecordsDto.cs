using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Dtos
{
    public class StudentYearRecordsDto
    {
        public short EduYear { get; set; }

        public List<RecordDiscAndStatusPairDto> Nonparsemester { get; set; }

        public List<RecordDiscAndStatusPairDto> Parsemester { get; set; }
    }
}
