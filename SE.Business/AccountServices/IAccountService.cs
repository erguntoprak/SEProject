using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.AccountServices
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(LoginDto loginDto);
        Task<UserDto> GetUserDtoByEmailAsync(string email);
        Task<bool> RegisterAsync(RegisterDto registerDto);
    }
}
