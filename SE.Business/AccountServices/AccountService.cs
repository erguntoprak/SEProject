using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Core;
using SE.Business.EmailSenders;
using SE.Business.Infrastructure.ConfigurationManager;
using SE.Core.DTO;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SE.Business.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IValidator<LoginDto> _loginDtoValidator;
        private readonly IValidator<RegisterDto> _registerDtoValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailSender;
        private readonly Configuration _configuration;



        public AccountService(UserManager<User> userManager, IValidator<LoginDto> loginDtoValidator, IValidator<RegisterDto> registerDtoValidator, IHttpContextAccessor httpContextAccessor, IEmailService emailSender, IOptions<Configuration> configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _loginDtoValidator = loginDtoValidator;
            _registerDtoValidator = registerDtoValidator;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _configuration = configuration.Value;
            _roleManager = roleManager;
        }
        public async Task<bool> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var loginDtoValidate = _loginDtoValidator.Validate(loginDto, ruleSet: "all");
                if (!loginDtoValidate.IsValid)
                {
                    throw new ValidationException(loginDtoValidate.Errors);
                }
                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user != null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                    if (result)
                    {
                        if (!user.EmailConfirmed)
                        {
                            throw new ValidationException("E-posta adresinizi doğrulamanız gerekiyor, lütfen e-postanıza gelen bağlantıya tıklayın.");
                        }
                        if (!user.IsActive)
                        {
                            throw new InvalidOperationException("Üzgünüz, bu hesap aktif değil. Lütfen iletişime geçin.");
                        }
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                throw;
            }


        }
        public async Task<UserDto> GetUserDtoByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            if (user == null || roles.Count < 1)
            {
                throw new Exception();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.FirsName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Surname = user.LastName,
                Roles = roles
            };
            return userDto;
        }

        public async Task<UserDto> GetUserDtoByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            if (user == null || roles.Count < 1)
            {
                throw new Exception();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.FirsName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Surname = user.LastName,
                Roles = roles
            };
            return userDto;
        }

        public async Task<List<UserListDto>> GetAllUserList()
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
            return userList;
        }
        public async Task<List<RoleDto>> GetAllRoleList()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(d => new RoleDto { Id = d.Id, Name = d.Name }).ToList();
        }
        public async Task UpdateUserRole(string userId, List<string> roles)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var userRoles = await _userManager.GetRolesAsync(user);

                var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);

                if (removeResult.Succeeded)
                {
                    var insertResult = await _userManager.AddToRolesAsync(user, roles);

                    if (!insertResult.Succeeded)
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw;
            }

        }
        public async Task UpdateEmailConfirmation(string userId, bool emailConfirmation)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                user.EmailConfirmed = emailConfirmation;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task UpdateUserActivate(string userId, bool isActive)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                user.IsActive = isActive;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userUpdateDto.UserId);

                if (user.Email != userUpdateDto.Email)
                {
                    user.EmailConfirmed = false;
                    user.Email = userUpdateDto.Email;
                    var resendResult = await ResendEmailConfirmation(userUpdateDto.Email);

                    if (!resendResult)
                    {
                        throw new Exception();
                    }
                }
                user.FirsName = userUpdateDto.Name;
                user.LastName = userUpdateDto.Surname;
                user.PhoneNumber = userUpdateDto.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
        }
        public async Task UpdateUserPassword(UserPasswordUpdateDto userPasswordUpdateDto)
        {
            var user = await _userManager.FindByIdAsync(userPasswordUpdateDto.UserId);

            if (user != null)
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userPasswordUpdateDto.Password);

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task ForgotPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var confirmationToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(confirmationToken);
                    var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

                    var confirmationLink = $"{_configuration.BaseUrl}/sifre-sifirla?userId={user.Id}&confirmation-token={codeEncoded}";
                    await _emailSender.SendAsync(user.Email, "İzmir Eğitim Kurumları Şifre Sıfırlama Servisi!", EmailMessages.GetForgotPasswordHtml(confirmationLink));
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(resetPasswordDto.UserId);

                if (user != null)
                {
                    var codeDecodedBytes = WebEncoders.Base64UrlDecode(resetPasswordDto.Token);
                    var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

                    var resetPassResult = await _userManager.ResetPasswordAsync(user, codeDecoded, resetPasswordDto.Password);

                    if (!resetPassResult.Succeeded)
                    {
                        throw new Exception();
                    }

                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var registerDtoValidate = _registerDtoValidator.Validate(registerDto, ruleSet: "all");

            if (!registerDtoValidate.IsValid)
            {
                throw new ValidationException(registerDtoValidate.Errors);
            }

            if (_userManager.Users.Any(d => d.Email == registerDto.Email))
            {
                throw new ValidationException("E-posta daha önce kullanılmış.");
            }
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

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                var savedUser = await _userManager.FindByEmailAsync(registerDto.Email);
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(savedUser);
                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(confirmationToken);
                var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                var confirmationLink = $"{_configuration.BaseUrl}/e-posta-onay?userId={savedUser.Id}&confirmation-token={codeEncoded}";
                await _emailSender.SendAsync(savedUser.Email, "E-posta adresinizi onaylayın!", EmailMessages.GetEmailConfirmationHtml(confirmationLink));
                return true;
            }
            return false;
        }

        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            return $"{request.Scheme}://{host}{pathBase}";
        }

        public async Task<bool> ResendEmailConfirmation(string email)
        {
            var savedUser = await _userManager.FindByEmailAsync(email);

            if (savedUser == null)
            {
                throw new ArgumentNullException();
            }

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(savedUser);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(confirmationToken);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var confirmationLink = $"{_configuration.BaseUrl}/e-posta-onay?userId={savedUser.Id}&confirmation-token={codeEncoded}";
            await _emailSender.SendAsync(savedUser.Email, "E-posta adresinizi onaylayın!", EmailMessages.GetEmailConfirmationHtml(confirmationLink));
            return true;
        }

        public async Task<string> EmailConfirmation(EmailConfirmationDto emailConfirmation)
        {
            var savedUser = await _userManager.FindByIdAsync(emailConfirmation.UserId);

            if (savedUser == null)
            {
                throw new ArgumentNullException();
            }

            if (savedUser.EmailConfirmed)
            {
                return "E-posta adresiniz zaten onaylanmış.";
            }
            var codeDecodedBytes = WebEncoders.Base64UrlDecode(emailConfirmation.ConfirmationToken);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
            var result = await _userManager.ConfirmEmailAsync(savedUser, codeDecoded);

            if (result.Succeeded)
            {
                return "E-posta adresiniz onaylandı.";
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
