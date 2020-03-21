using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
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
                    var claims = new ClaimsIdentity(new Claim[]
                    {
                                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
                    });

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecurityTokenSetting.Key));
                    var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = creds,
                        Issuer = _jwtSecurityTokenSetting.Issuer,
                        Audience = _jwtSecurityTokenSetting.Audience

                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);


                    UserModel userModel = new UserModel()
                    {
                        Id = userDto.Id,
                        Name = userDto.Name,
                        Surname = userDto.Surname,
                        Token = tokenString
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
                return StatusCode(StatusCodes.Status400BadRequest, ex.Errors.Select(d => d.ErrorMessage));
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
                if(registerResult != false)
                {
                    return Ok();
                }
                return StatusCode(StatusCodes.Status400BadRequest, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Errors.Select(d => d.ErrorMessage));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi daha sonra tekrar deneyiniz.");
            }
        }
    }
}
