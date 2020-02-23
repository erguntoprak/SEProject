using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SE.Business.Infrastructure.FluentValidation.Validations;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Data;
using SE.Web.Extentions;
using SE.Web.Infrastructure.EmailSenders;
using SE.Web.Infrastructure.Jwt;
using SE.Web.Model;
using SE.Web.Model.Validations;
using System;
using System.Text;

namespace SE.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<EntitiesDbContext>().AddDefaultTokenProviders();


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie().AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetSection("Token").GetSection("Issuer").Value,
                        ValidAudience = Configuration.GetSection("Token").GetSection("Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Token").GetSection("Key").Value))
                    };
                });
            string sqlConnection = @"Server =.; Database = EducationDb; Trusted_Connection = True;";
            services.AddDbContext<EntitiesDbContext>(dbcontextoption => dbcontextoption.UseSqlServer(sqlConnection, b => b.MigrationsAssembly("SE.Web")));
            services.DependencyRegister();
            services.AddTransient<IValidator<LoginModel>, LoginModelValidator>();
            services.AddSingleton<IValidator<EducationInsertDto>, EducationInsertDtoValidation>();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<JwtSecurityTokenSetting>(Configuration.GetSection("Token"));
            services.AddAutoMapper(typeof(Startup));
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
