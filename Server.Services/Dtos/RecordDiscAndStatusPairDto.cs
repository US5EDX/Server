using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Dtos
{
    public class RecordDiscAndStatusPairDto
    {
        public string CodeName { get; set; }

        public byte Approved { get; set; }
    }
}
