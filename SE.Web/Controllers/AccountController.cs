﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SE.Core.Entities;
using SE.Web.Model;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public IConfiguration _config;


        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
                            var claims = new[]
                            {
                            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                        };

                            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Token").GetSection("Key").Value));
                            //İlerki sürümlerde kullanılcak
                            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

                            var token = new JwtSecurityToken(
                                issuer: _config.GetSection("Token").GetSection("Issuer").Value,
                                audience: _config.GetSection("Token").GetSection("Audience").Value,
                                claims: claims,
                                expires: DateTime.UtcNow.AddMinutes(30),
                                signingCredentials: creds
                                );

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
            catch (Exception)
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

                    User user = new User
                    {
                        UserName = registerModel.UserName,
                        Email = registerModel.Email
                    };
                    var result = await _userManager.CreateAsync(user, registerModel.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };

                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            _config["Tokens:Issuer"],
                            _config["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(30),
                            signingCredentials: creds
                            );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                        var savedUser = await _userManager.FindByEmailAsync(registerModel.Email);

                        UserModel userModel = new UserModel()
                        {
                            Id = savedUser.Id,
                            Name = savedUser.FirsName,
                            Surname = savedUser.LastName,
                            Email = savedUser.Email,
                            UserName = savedUser.UserName,
                            Token = tokenString
                        };
                        responseModel.Data = userModel;
                        return Ok(responseModel);

                    }
                }
                responseModel.ErrorMessage.Add("Tüm bilgileri eksiksiz giriniz.");
                return NotFound(responseModel);
            }
            catch (Exception)
            {
                responseModel.ErrorMessage.Add("Bilinmeyen bir hata oluştu.Lütfen işlemi tekrar deneyiniz.");
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }           
        }
    }
}
