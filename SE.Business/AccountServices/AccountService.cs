using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SE.Business.Constants;
using SE.Business.EmailSenders;
using SE.Business.Infrastructure.ConfigurationManager;
using SE.Business.Infrastructure.FluentValidation.Validations;
using SE.Core.Aspects.Autofac.Logging;
using SE.Core.Aspects.Autofac.Validation;
using SE.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.AccountServices
{
    [LogAspect(typeof(FileLogger))]
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly Configuration _configuration;



        public AccountService(UserManager<User> userManager, IEmailSender emailSender, IOptions<Configuration> configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration.Value;
            _roleManager = roleManager;
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(LoginDtoValidator), Priority = 1)]
        public async Task<IResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return new ErrorDataResult<UserDto>(Messages.EmailAndPasswordNotMatch);

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
                return new ErrorResult(Messages.EmailAndPasswordNotMatch);

            if (!user.EmailConfirmed)
                return new ErrorResult(Messages.YouNeedToVerifyYourEmailAddress);

            if (!user.IsActive)
                return new ErrorResult(Messages.AccountIsNotActive);

            return new SuccessResult(Messages.SuccessfulLogin);
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(RegisterDtoValidator), Priority = 1)]
        public async Task<IResult> RegisterAsync(RegisterDto registerDto)
        {
            if (_userManager.Users.Any(d => d.Email == registerDto.Email))
                return new ErrorResult(Messages.EmailUsedBefore);

            User user = new User
            {
                FirsName = registerDto.Name,
                LastName = registerDto.Surname,
                UserName = Guid.NewGuid().ToString(),
                Email = registerDto.Email,
                PhoneNumber = registerDto.Phone,
                IsActive = false
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return new ErrorResult(Messages.NotAdded);

            await _userManager.AddToRoleAsync(user, "User");
            var savedUser = await _userManager.FindByEmailAsync(registerDto.Email);
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(savedUser);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(confirmationToken);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var confirmationLink = $"{_configuration.BaseUrl}/e-posta-onay?userId={savedUser.Id}&confirmation-token={codeEncoded}";
            await _emailSender.SendAsync(savedUser.Email, "E-posta adresinizi onaylayın!", EmailMessages.GetEmailConfirmationHtml(confirmationLink, _configuration.ApiUrl));

            return new SuccessResult(Messages.SuccessfulRegister);
        }
        
        
        public async Task<IDataResult<UserDto>> GetUserDtoByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            if (user == null || !roles.Any())
                return new ErrorDataResult<UserDto>(Messages.UserNotFound);

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.FirsName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Surname = user.LastName,
                Roles = roles
            };
            return new SuccessDataResult<UserDto>(userDto);
        }
        public async Task<IDataResult<UserDto>> GetUserDtoByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            if (user == null || !roles.Any())
                return new ErrorDataResult<UserDto>(Messages.UserNotFound);

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.FirsName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Surname = user.LastName,
                Roles = roles
            };
            return new SuccessDataResult<UserDto>(userDto);
        }
        public async Task<IDataResult<IEnumerable<UserListDto>>> GetAllUserListAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            List<UserListDto> userList = new List<UserListDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserListDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirsName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList(),
                    EmailConfirmed = user.EmailConfirmed ? "Onaylandı" : "Onaylanmadı",
                    Activated = user.IsActive ? "Aktif" : "Aktif Değil"
                });
            }
            return new SuccessDataResult<IEnumerable<UserListDto>>(userList);
        }
        public async Task<IDataResult<IEnumerable<RoleDto>>> GetAllRoleListAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return new SuccessDataResult<IEnumerable<RoleDto>>(roles.Select(d => new RoleDto { Id = d.Id, Name = d.Name }).ToList());
        }
        public async Task<IResult> UpdateUserRoleAsync(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (user == null || !userRoles.Any())
                return new ErrorResult(Messages.UserNotFound);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
                return new ErrorResult(Messages.CouldNotDeleteRoles);

            var insertResult = await _userManager.AddToRolesAsync(user, roles);

            if (!insertResult.Succeeded)
                return new ErrorResult(Messages.CouldNotAddRoles);

            return new SuccessResult(Messages.Updated);

        }
        public async Task<IResult> UpdateEmailConfirmationAsync(string userId, bool emailConfirmation)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            user.EmailConfirmed = emailConfirmation;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return new ErrorResult(Messages.NotUpdated);

            return new SuccessResult(Messages.Updated);
        }
        public async Task<IResult> UpdateUserActivateAsync(string userId, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);
            user.IsActive = isActive;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return new ErrorResult(Messages.NotUpdated);

            return new SuccessResult(Messages.Updated);
        }
        public async Task<IResult> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var user = await _userManager.FindByIdAsync(userUpdateDto.UserId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            if (user.Email != userUpdateDto.Email)
            {
                if (_userManager.Users.Any(d => d.Email == userUpdateDto.Email))
                    return new ErrorResult(Messages.EmailUsedBefore);

                user.EmailConfirmed = false;
                user.Email = userUpdateDto.Email;
                var resendResult = await ResendEmailConfirmationAsync(userUpdateDto.Email);

                if (!resendResult.Success)
                    return new ErrorResult(Messages.NotUpdated);
            }

            if (user.UserName != userUpdateDto.UserName)
            {
                if (_userManager.Users.Any(d => d.UserName == userUpdateDto.UserName))
                    return new ErrorResult(Messages.UserNameUsedBefore);

                user.UserName = userUpdateDto.UserName;
            }

            user.FirsName = userUpdateDto.Name;
            user.LastName = userUpdateDto.Surname;
            user.PhoneNumber = userUpdateDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return new ErrorResult(Messages.NotUpdated);

            return new SuccessResult(Messages.Updated);
        }
        public async Task<IResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return new ErrorResult(Messages.NotDeleted);

            return new SuccessResult(Messages.Deleted);
        }
        public async Task<IResult> UpdateUserPasswordAsync(UserPasswordUpdateDto userPasswordUpdateDto)
        {
            var user = await _userManager.FindByIdAsync(userPasswordUpdateDto.UserId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userPasswordUpdateDto.Password);

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return new ErrorResult(Messages.NotUpdated);

            return new SuccessResult(Messages.Updated);
        }
        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            var confirmationToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(confirmationToken);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

            var confirmationLink = $"{_configuration.BaseUrl}/sifre-sifirla?userId={user.Id}&confirmation-token={codeEncoded}";
            await _emailSender.SendAsync(user.Email, "İzmir Eğitim Kurumları Şifre Sıfırlama Servisi!", EmailMessages.GetForgotPasswordHtml(confirmationLink, _configuration.ApiUrl));

            return new SuccessResult(Messages.EmailSended);
        }
        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByIdAsync(resetPasswordDto.UserId);

            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            var codeDecodedBytes = WebEncoders.Base64UrlDecode(resetPasswordDto.Token);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

            var resetPassResult = await _userManager.ResetPasswordAsync(user, codeDecoded, resetPasswordDto.Password);

            if (!resetPassResult.Succeeded)
                return new ErrorResult(Messages.NotUpdated);

            return new SuccessResult(Messages.Updated);
        }
        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> ResendEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(confirmationToken);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var confirmationLink = $"{_configuration.BaseUrl}/e-posta-onay?userId={user.Id}&confirmation-token={codeEncoded}";
            await _emailSender.SendAsync(user.Email, "E-posta adresinizi onaylayın!", EmailMessages.GetEmailConfirmationHtml(confirmationLink, _configuration.ApiUrl));

            return new SuccessResult(Messages.EmailSended);
        }
        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> EmailConfirmationAsync(EmailConfirmationDto emailConfirmation)
        {
            var user = await _userManager.FindByIdAsync(emailConfirmation.UserId);

            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            if (user.EmailConfirmed)
                return new ErrorResult(Messages.EmailAlreadyConfirmed);

            var codeDecodedBytes = WebEncoders.Base64UrlDecode(emailConfirmation.ConfirmationToken);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
            var result = await _userManager.ConfirmEmailAsync(user, codeDecoded);

            if (!result.Succeeded)
                return new ErrorResult(Messages.EmailNotConfirmed);

            return new SuccessResult(Messages.EmailConfirmed);

        }
    }
}
