using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Account
{
    public class EmailConfirmation
    {
        public string UserId { get; set; }
        public string ConfirmationToken { get; set; }
    }
}
