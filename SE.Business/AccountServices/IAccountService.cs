using SE.Core.DTO;
using SE.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.AccountServices
{
    public interface IAccountService
    {
        Task<IResult> LoginAsync(LoginDto loginDto);
        Task<IDataResult<UserDto>> GetUserDtoByEmailAsync(string email);
        Task<IDataResult<UserDto>> GetUserDtoByIdAsync(string userId);
        Task<IResult> RegisterAsync(RegisterDto registerDto);
        Task<IResult> ResendEmailConfirmationAsync(string email);
        Task<IResult> EmailConfirmationAsync(EmailConfirmationDto emailConfirmation);
        Task<IDataResult<IEnumerable<UserListDto>>> GetAllUserListAsync();
        Task<IDataResult<IEnumerable<RoleDto>>> GetAllRoleListAsync();
        Task<IResult> UpdateUserRoleAsync(string userId,List<string> roles);
        Task<IResult> UpdateEmailConfirmationAsync(string userId,bool emailConfirmation);
        Task<IResult> UpdateUserActivateAsync(string userId, bool isActive);
        Task<IResult> UpdateUserPasswordAsync(UserPasswordUpdateDto userPasswordUpdateDto);
        Task<IResult> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<IResult> DeleteUserAsync(string userId);
        Task<IResult> ForgotPasswordAsync(string email);
        Task<IResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

    }
}
