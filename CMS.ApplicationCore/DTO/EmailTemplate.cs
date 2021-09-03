using CMS.ApplicationCore.Enum;
using System;

namespace CMS.ApplicationCore.DTO
{
    public class EmailTemplate
    {
        public Guid Id { get; set; }
        public EmailType EmailType { get; set; }
        public string Content { get; set; }
        public bool Active { get; set; }
    }
}
