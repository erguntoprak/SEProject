﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class EducationContactFormInsertDto
    {
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int EducationId { get; set; }
    }
}
