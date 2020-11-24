using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class UserUpdateDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
