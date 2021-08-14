using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.ApplicationCore.DTO
{
    public class EmailConfig
    {
        public string[] To { get; set; }
        public string From { get; set; } = "person.info.dev@gmail.com";
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
