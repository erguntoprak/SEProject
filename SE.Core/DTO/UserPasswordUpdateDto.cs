using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class UserPasswordUpdateDto
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
