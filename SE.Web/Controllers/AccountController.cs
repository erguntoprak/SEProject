using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SE.Business.AccountServices;
using SE.Core.DTO;
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
            var loginDto = _mapper.Map<LoginDto>(loginModel);
            var result = await _accountService.LoginAsync(loginDto);

            if (result.Success)
            {
                var userDto = await _accountService.GetUserDtoByEmailAsync(loginModel.Email);
                if (!userDto.Success)
                    return BadRequest(userDto.Message);

                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userDto.Data.Id));
                claims.AddClaim(new Claim(ClaimTypes.Email, userDto.Data.Email));
                claims.AddClaim(new Claim(ClaimTypes.Name, $"{userDto.Data.Name} {userDto.Data.Surname}"));
                foreach (var role in userDto.Data.Roles)
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
                    UserId = userDto.Data.Id,
                    Name = userDto.Data.Name,
                    Surname = userDto.Data.Surname,
                    Email = userDto.Data.Email,
                    Token = tokenString,
                    Roles = userDto.Data.Roles.ToList()
                };
                return Ok(userModel);
            }
            return BadRequest(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var registerDto = _mapper.Map<RegisterDto>(registerModel);
            var result = await _accountService.RegisterAsync(registerDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] string email)
        {
            var result = await _accountService.ResendEmailConfirmationAsync(email);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromBody] EmailConfirmation emailConfirmation)
        {
            var emailConfirmationDto = _mapper.Map<EmailConfirmationDto>(emailConfirmation);
            var result = await _accountService.EmailConfirmationAsync(emailConfirmationDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetAllUserList")]
        public async Task<IActionResult> GetAllUserList()
        {
            var result = await _accountService.GetAllUserListAsync();
            if (result.Success)
                return Ok(_mapper.Map<List<UserListModel>>(result.Data));
            return BadRequest(result);

        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetAllRoleList")]
        public async Task<IActionResult> GetAllRoleList()
        {
            var result = await _accountService.GetAllRoleListAsync();
            if (result.Success)
                return Ok(_mapper.Map<List<RoleModel>>(result.Data));
            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] RoleUpdateModel updateRoleModel)
        {
            var result = await _accountService.UpdateUserRoleAsync(updateRoleModel.UserId, updateRoleModel.Roles);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPost("UpdateEmailConfirmation")]
        public async Task<IActionResult> UpdateEmailConfirmation([FromBody] EmailConfirmationUpdateModel emailConfirmationUpdateModel)
        {
            var result = await _accountService.UpdateEmailConfirmationAsync(emailConfirmationUpdateModel.UserId, emailConfirmationUpdateModel.EmailConfirmation);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateUserActivate")]
        public async Task<IActionResult> UpdateUserActivate([FromBody] UserActivateModel userActivateModel)
        {
            var result = await _accountService.UpdateUserActivateAsync(userActivateModel.UserId, userActivateModel.IsActive);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateModel userUpdateModel)
        {
            var userUpdateDto = _mapper.Map<UserUpdateDto>(userUpdateModel);
            var result = await _accountService.UpdateUserAsync(userUpdateDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }


        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string userId)
        {
            var result = await _accountService.DeleteUserAsync(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateUserPassword")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UserPasswordUpdateModel userPasswordUpdateModel)
        {
            var userPasswordUpdateDto = _mapper.Map<UserPasswordUpdateDto>(userPasswordUpdateModel);
            var result = await _accountService.UpdateUserPasswordAsync(userPasswordUpdateDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery] string userId)
        {
            var result = await _accountService.GetUserDtoByIdAsync(userId);
            if (result.Success)
                return Ok(_mapper.Map<UserModel>(result.Data));
            return BadRequest(result);


        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var result = await _accountService.GetUserDtoByEmailAsync(email);
            if (result.Success)
                return Ok(_mapper.Map<UserModel>(result.Data));
            return BadRequest(result);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _accountService.ForgotPasswordAsync(email);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var resetPasswordDto = _mapper.Map<ResetPasswordDto>(resetPasswordModel);
            var result = await _accountService.ResetPasswordAsync(resetPasswordDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }


    }
}
