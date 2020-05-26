using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Core;
using SE.Business.AccountServices;
using SE.Business.AddressServices;
using SE.Business.AttributeServices;
using SE.Business.BlogServices;
using SE.Business.CategoryServices;
using SE.Business.EducationServices;
using SE.Business.EmailSenders;
using SE.Business.ImageServices;
using SE.Business.Infrastructure.FluentValidation.Validations;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Data;

namespace SE.Web.Extentions
{
    public static class DependencyResolver
    {
        public static void DependencyRegister(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddScoped<DbContext, EntitiesDbContext>();
            //serviceProvider.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            serviceProvider.AddScoped<IUnitOfWork, UnitOfWork>();

            serviceProvider.AddScoped<IEmailService, EmailSender>();
            serviceProvider.AddScoped<IImageService, ImageService>();
            serviceProvider.AddScoped<IAttributeService, AttributeService>();
            serviceProvider.AddScoped<ICategoryService, CategoryService>();
            serviceProvider.AddScoped<IAddressService, AddressService>();
            serviceProvider.AddScoped<IEducationService, EducationService>();
            serviceProvider.AddScoped<IAccountService, AccountService>();
            serviceProvider.AddScoped<IBlogService, BlogService>();
            serviceProvider.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
            serviceProvider.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
            serviceProvider.AddScoped<IValidator<EducationInsertDto>, EducationInsertDtoValidator>();
            serviceProvider.AddScoped<IValidator<EducationInsertDto>, EducationInsertDtoValidator>();
            serviceProvider.AddScoped<IValidator<EducationUpdateDto>, EducationUpdateDtoValidator>();
            serviceProvider.AddScoped<IValidator<BlogInsertDto>, BlogInsertDtoValidator>();
            serviceProvider.AddScoped<IValidator<BlogUpdateDto>, BlogUpdateDtoValidator>();





        }
    }
}
