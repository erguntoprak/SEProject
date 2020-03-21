using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;
using SE.Business.EmailSenders;
using SE.Core.DTO;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<LoginDto> _loginDtoValidator;
        private readonly IValidator<RegisterDto> _registerDtoValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailSender;


        public AccountService(SignInManager<User> signInManager, UserManager<User> userManager, IValidator<LoginDto> loginDtoValidator,IValidator<RegisterDto> registerDtoValidator, IHttpContextAccessor httpContextAccessor, IEmailService emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _loginDtoValidator = loginDtoValidator;
            _registerDtoValidator = registerDtoValidator;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
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
                    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                    return result.Succeeded;
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
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.FirsName,
                Surname = user.LastName
            };
            return userDto;
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
                UserName = "egitimkurumu",
                Email = registerDto.Email,
                PhoneNumber = registerDto.Phone
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                var savedUser = await _userManager.FindByEmailAsync(registerDto.Email);
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(savedUser);
                var confirmationLink = $"{GetBaseUrl()}/e-posta-onay?userId={savedUser.Id}&confirmation-token={confirmationToken}";
                await _emailSender.SendAsync(savedUser.Email, "E-posta adresinizi onaylayın!", EmailMessages.GetEmailConfirmationHtml(savedUser.UserName, confirmationLink));
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
    }
}
