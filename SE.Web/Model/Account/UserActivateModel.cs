using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Account
{
    public class UserActivateModel
    {
        public string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
