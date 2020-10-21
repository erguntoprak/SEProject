using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class EmailConfirmationDto
    {
        public string UserId { get; set; }
        public string ConfirmationToken { get; set; }
    }
}
