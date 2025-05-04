using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Dtos
{
    public class GroupDto
    {
        public uint GroupId { get; set; }

        public string GroupCode { get; set; } = null!;

        public byte EduLevel { get; set; }

        public byte Course { get; set; }

        public byte DurationOfStudy { get; set; }

        public short AdmissionYear { get; set; }

        public byte Nonparsemester { get; set; }

        public byte Parsemester { get; set; }

        public bool HasEnterChoise { get; set; }

        public byte ChoiceDifference { get; set; }
    }
}
