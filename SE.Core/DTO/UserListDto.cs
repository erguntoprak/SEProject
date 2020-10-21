using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class UserListDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
        public string EmailConfirmed { get; set; }
        public string Activated { get; set; }
    }
}
