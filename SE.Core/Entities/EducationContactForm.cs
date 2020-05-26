using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class EducationContactForm : BaseEntity
    {
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int EducationId { get; set; }
        public Education Education { get; set; }
    }
}
