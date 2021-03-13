using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Core.Utilities.Security.Http
{
    public interface IRequestContext
    {
        string NameSurname { get; }
        string Email { get; }
        string UserId { get; }
        List<string> Roles { get; }
    }
}
