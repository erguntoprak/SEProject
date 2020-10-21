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
        Task<UserDto> GetUserDtoByIdAsync(string userId);
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<bool> ResendEmailConfirmation(string email);
        Task<string> EmailConfirmation(EmailConfirmationDto emailConfirmation);
        Task<List<UserListDto>> GetAllUserList();
        Task<List<RoleDto>> GetAllRoleList();
        Task UpdateUserRole(string userId,List<string> roles);
        Task UpdateEmailConfirmation(string userId,bool emailConfirmation);
        Task UpdateUserActivate(string userId, bool isActive);
        Task UpdateUserPassword(UserPasswordUpdateDto userPasswordUpdateDto);
        Task UpdateUser(UserUpdateDto userUpdateDto);
        Task DeleteUser(string userId);
        Task ForgotPassword(string email);
        Task ResetPassword(ResetPasswordDto resetPasswordDto);

    }
}
