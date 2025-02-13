using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Dtos
{
    public class MailOptions
    {
        public string SenderName { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; } = default!;
        public string SmtpPassword { get; set; } = default!;
    }
}
