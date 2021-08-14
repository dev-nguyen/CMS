using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.ApplicationCore.DTO
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
        public bool Lock { get; set; }
    }
}
