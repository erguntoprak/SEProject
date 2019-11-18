using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SE.Business.AttributeServices;
using SE.Business.CategoryServices;
using SE.Core.Entities;
using SE.Data;

namespace SE.Web.Extentions
{
    public static class DependencyResolver
    {
        public static void DependencyRegister(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddScoped<DbContext, EntitiesDbContext>();
            serviceProvider.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            serviceProvider.AddScoped<IAttributeService, AttributeService>();
            serviceProvider.AddScoped<ICategoryService, CategoryService>();



        }
    }
}
