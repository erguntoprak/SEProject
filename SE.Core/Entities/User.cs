﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class User:IdentityUser
    {
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public ICollection<Education> Educations { get; set; }
    }
}
