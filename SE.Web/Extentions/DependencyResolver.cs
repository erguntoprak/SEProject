using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SE.Business.AttributeServices;
using SE.Core.Entities;
using SE.Data;

namespace SE.Web.Extentions
{
    public static class DependencyResolver
    {
        public static void DependencyRegister(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddScoped<DbContext, EntitiesDbContext>();
            serviceProvider.AddScoped<IRepository<EducationAttribute>, Repository<EducationAttribute>>();
            serviceProvider.AddScoped<IRepository<EducationAttributeCategory>, Repository<EducationAttributeCategory>>();
            serviceProvider.AddScoped<IAttributeService, AttributeService>();


        }
    }
}
