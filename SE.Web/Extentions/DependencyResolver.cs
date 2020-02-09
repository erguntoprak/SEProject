using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Core;
using SE.Business.AddressServices;
using SE.Business.AttributeServices;
using SE.Business.CategoryServices;
using SE.Business.EducationServices;
using SE.Core.Entities;
using SE.Data;
using SE.Web.Infrastructure.EmailSenders;

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
            serviceProvider.AddScoped<IAttributeService, AttributeService>();
            serviceProvider.AddScoped<ICategoryService, CategoryService>();
            serviceProvider.AddScoped<IAddressService, AddressService>();
            serviceProvider.AddScoped<IEducationService, EducationService>();




        }
    }
}
