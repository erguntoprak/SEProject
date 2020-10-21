using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class ResetPasswordDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
