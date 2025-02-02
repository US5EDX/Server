using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Dtos
{
    public class FacultyDto
    {
        public uint FacultyId { get; set; }

        public string FacultyName { get; set; } = null!;
    }
}
