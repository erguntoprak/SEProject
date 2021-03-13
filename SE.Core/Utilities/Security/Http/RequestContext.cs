using Microsoft.AspNetCore.Http;
using SE.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using SE.Core.Extensions;

namespace SE.Core.Utilities.Security.Http
{
    public class RequestContext : IRequestContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequestContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string NameSurname => _httpContextAccessor.HttpContext.User.ClaimNameSurname();

        public string Email => _httpContextAccessor.HttpContext.User.ClaimEmail();

        public string UserId => _httpContextAccessor.HttpContext.User.ClaimUserId();

        public List<string> Roles => _httpContextAccessor.HttpContext.User.ClaimRoles();
    }
}
