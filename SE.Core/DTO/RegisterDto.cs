﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; } 
        public string Phone { get; set; }
        public string Password { get; set; }    
    }
}
