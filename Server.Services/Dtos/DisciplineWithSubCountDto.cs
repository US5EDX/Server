using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Dtos
{
    public class DisciplineWithSubCountDto
    {
        public uint DisciplineId { get; set; }

        public string DisciplineCode { get; set; }

        public byte CatalogType { get; set; }

        public FacultyDto Faculty { get; set; }

        public SpecialtyDto? Specialty { get; set; }

        public string DisciplineName { get; set; }

        public byte EduLevel { get; set; }

        public string Course { get; set; }

        public byte Semester { get; set; }

        public string Prerequisites { get; set; }

        public string Interest { get; set; }

        public int MaxCount { get; set; }

        public int MinCount { get; set; }

        public string Url { get; set; }

        public int NonparsemesterCount { get; set; }

        public int ParsemesterCount { get; set; }

        public short Holding { get; set; }

        public bool IsYearLong { get; set; }

        public bool IsOpen { get; set; }
    }
}
