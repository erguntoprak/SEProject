using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using SE.Core.Entities;
using SE.Web.Infrastructure.EmailSenders;
using SE.Web.Infrastructure.Helpers;
using SE.Web.Infrastructure.Jwt;
using SE.Web.Model;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtSecurityTokenSetting _jwtSecurityTokenSetting;
        private readonly IEmailService _emailService;
        public IConfiguration _config;


        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration config, IOptions<JwtSecurityTokenSetting> jwtSecurityTokenSetting, IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            //_emailSender = emailSender;
            _jwtSecurityTokenSetting = jwtSecurityTokenSetting.Value;
            _config = config;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    responseModel.ErrorMessage = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(responseModel);
                }

                if (loginModel != null)
                {
                    var user = await _userManager.FindByEmailAsync(loginModel.Email);
                    if (user != null)
                    {
                        var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

                        if (result.Succeeded)
                        {
                            var claims = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, user.Id.ToString())
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
                                Id = user.Id,
                                Name = user.FirsName,
                                Surname = user.LastName,
                                Email = user.Email,
                                UserName = user.UserName,
                                Token = tokenString
                            };
                            responseModel.Data = userModel;
                            return Ok(responseModel);
                        }
                    }
                }
                responseModel.ErrorMessage.Add("Email veya password yanlış");
                return NotFound(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.ErrorMessage.Add("Bilinmeyen bir hata oluştu.Lütfen işlemi tekrar deneyiniz.");
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (registerModel != null)
                {
                    if (!ModelState.IsValid)
                    {
                        responseModel.ErrorMessage = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                        return BadRequest(responseModel);
                    }
                    if (_userManager.Users.Any(d => d.Email == registerModel.Email))
                    {
                        responseModel.ErrorMessage.Add("E-posta daha önce kullanılmıştır.");
                        return BadRequest(responseModel);
                    }
                  
                    User user = new User
                    {
                        UserName = "egitimkurumu",
                        Email = registerModel.Email,
                        PhoneNumber = registerModel.Phone
                    };
                    var result = await _userManager.CreateAsync(user, registerModel.Password);

                    if (result.Succeeded)
                    {
                        var savedUser = await _userManager.FindByEmailAsync(registerModel.Email);
                        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(savedUser);
                        var confirmationLink = $"{HelperMethods.GetBaseUrl(HttpContext)}/e-posta-onay?userId={savedUser.Id}&confirmation-token={confirmationToken}";
                        await _emailService.SendAsync(savedUser.Email, "E-posta adresinizi onaylayın!", EmailMessages.GetEmailConfirmationHtml(savedUser.UserName, confirmationLink));
                        return Ok(responseModel);

                    }
                }
                responseModel.ErrorMessage.Add("Tüm bilgileri eksiksiz giriniz.");
                return NotFound(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.ErrorMessage.Add("Bilinmeyen bir hata oluştu.Lütfen işlemi tekrar deneyiniz.");
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }
    }
}
