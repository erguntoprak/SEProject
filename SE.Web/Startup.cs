using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SE.Business.EmailSenders;
using SE.Business.Infrastructure.Autofac;
using SE.Business.Infrastructure.ConfigurationManager;
using SE.Core.Entities;
using SE.Data;
using SE.Web.Extentions;
using SE.Web.Infrastructure;
using SE.Web.Infrastructure.Jwt;
using System;
using System.IO;
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
                cfg.Password.RequireDigit = false;
                cfg.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<EntitiesDbContext>().AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                            options.TokenLifespan = TimeSpan.FromDays(1));

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
            services.AddCors(options =>
            {
                options.AddPolicy("ApiPolicy",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                  });
            });
            services.AddDirectoryBrowser();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<JwtSecurityTokenSetting>(Configuration.GetSection("Token"));
            services.Configure<Configuration>(Configuration.GetSection("Configuration"));
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.Admin, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole("Admin");
                });
                options.AddPolicy(Policy.User, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole("User","Admin");
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager,
RoleManager<IdentityRole> roleManager)
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
            IdentitySeedData.SeedData(userManager, roleManager);
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("ApiPolicy");
            app.UseAuthentication();
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/MyImages"
            });
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
