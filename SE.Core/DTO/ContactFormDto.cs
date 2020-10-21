using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class ContactFormDto
    {
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
