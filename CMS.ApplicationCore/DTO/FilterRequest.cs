using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.ApplicationCore.DTO
{
    public class FilterRequest
    {
        public string field { get; set; }
        public string type { get; set; }
        public string operand { get; set; }
        public string value { get; set; }
        public string logic { get; set; }
    }
}
