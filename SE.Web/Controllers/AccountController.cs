using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using SE.Business.AccountServices;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Web.Infrastructure.Jwt;
using SE.Web.Model.Account;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ApiPolicy")]
    public class AccountController : ControllerBase
    {

        private readonly JwtSecurityTokenSetting _jwtSecurityTokenSetting;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public IConfiguration _config;


        public AccountController(IConfiguration config, IOptions<JwtSecurityTokenSetting> jwtSecurityTokenSetting, IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
            _jwtSecurityTokenSetting = jwtSecurityTokenSetting.Value;
            _config = config;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                var loginDto = _mapper.Map<LoginDto>(loginModel);
                var loginResult = await _accountService.LoginAsync(loginDto);

                if (loginResult)
                {

                    var userDto = await _accountService.GetUserDtoByEmailAsync(loginModel.Email);
                    var claims = new ClaimsIdentity();
                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userDto.Id));
                    claims.AddClaim(new Claim(ClaimTypes.Name, $"{userDto.Name} {userDto.Surname}"));
                    foreach (var role in userDto.Roles)
                    {
                        claims.AddClaim(new Claim(ClaimTypes.Role, $"{role}"));
                    }
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecurityTokenSetting.Key));
                    var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddDays(2),
                        SigningCredentials = creds,
                        Issuer = _jwtSecurityTokenSetting.Issuer,
                        Audience = _jwtSecurityTokenSetting.Audience

                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);


                    UserLoginModel userModel = new UserLoginModel()
                    {
                        Name = userDto.Name,
                        Surname = userDto.Surname,
                        Email = userDto.Email,
                        Token = tokenString,
                        Roles = userDto.Roles.ToList()
                    };
                    return Ok(userModel);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Email veya şifre yanlış, lütfen kontrol edip tekrar deneyiniz.");
                }
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            try
            {

                var registerDto = _mapper.Map<RegisterDto>(registerModel);
                var registerResult = await _accountService.RegisterAsync(registerDto);
                if (registerResult != false)
                {
                    return Ok();
                }
                return StatusCode(StatusCodes.Status400BadRequest, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }
        [HttpPost("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] string email)
        {
            try
            {
                await _accountService.ResendEmailConfirmation(email);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }

        [HttpPost("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromBody] EmailConfirmation emailConfirmation)
        {
            try
            {
                var emailConfirmationDto = _mapper.Map<EmailConfirmationDto>(emailConfirmation);
                var resultMessage = await _accountService.EmailConfirmation(emailConfirmationDto);
                return StatusCode(StatusCodes.Status200OK, new { emailConfirmationSuccessMessage = resultMessage });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetAllUserList")]
        public async Task<IActionResult> GetAllUserList()
        {
            try
            {
                var userList = await _accountService.GetAllUserList();
                var userListModel = _mapper.Map<List<UserListModel>>(userList);
                return Ok(userListModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }
        
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetAllRoleList")]
        public async Task<IActionResult> GetAllRoleList()
        {
            try
            {
                var roleList = await _accountService.GetAllRoleList();
                var roleListModel = _mapper.Map<List<RoleModel>>(roleList);
                return Ok(roleListModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody]RoleUpdateModel updateRoleModel)
        {
            try
            {
                await _accountService.UpdateUserRole(updateRoleModel.UserId, updateRoleModel.Roles);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }
        
        
        [HttpPost("UpdateEmailConfirmation")]
        public async Task<IActionResult> UpdateEmailConfirmation([FromBody] EmailConfirmationUpdateModel emailConfirmationUpdateModel)
        {
            try
            {
                await _accountService.UpdateEmailConfirmation(emailConfirmationUpdateModel.UserId, emailConfirmationUpdateModel.EmailConfirmation);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateUserActivate")]
        public async Task<IActionResult> UpdateUserActivate([FromBody] UserActivateModel userActivateModel)
        {
            try
            {
                await _accountService.UpdateUserActivate(userActivateModel.UserId, userActivateModel.IsActive);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody]UserUpdateModel userUpdateModel)
        {
            try
            {
                var userUpdateDto = _mapper.Map<UserUpdateDto>(userUpdateModel);
                await _accountService.UpdateUser(userUpdateDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }

        
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string userId)
        {
            try
            {
                await _accountService.DeleteUser(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateUserPassword")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UserPasswordUpdateModel userPasswordUpdateModel)
        {
            try
            {
                var userPasswordUpdateDto = _mapper.Map<UserPasswordUpdateDto>(userPasswordUpdateModel);
                await _accountService.UpdateUserPassword(userPasswordUpdateDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery]string userId)
        {
            try
            {
                var userModel = _mapper.Map<UserModel>(await _accountService.GetUserDtoByIdAsync(userId));
                return Ok(userModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            try
            {
                var userModel = _mapper.Map<UserModel>(await _accountService.GetUserDtoByEmailAsync(email));
                return Ok(userModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                await _accountService.ForgotPassword(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                var resetPasswordDto = _mapper.Map<ResetPasswordDto>(resetPasswordModel);
                await _accountService.ResetPassword(resetPasswordDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }


    }
}
