using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SE.Business.AccountServices;
using SE.Business.AddressServices;
using SE.Business.AttributeCategoryServices;
using SE.Business.AttributeServices;
using SE.Business.BlogServices;
using SE.Business.CategoryServices;
using SE.Business.CommonServices;
using SE.Business.EducationServices;
using SE.Business.EmailSenders;
using SE.Business.ImageServices;
using SE.Business.Infrastructure.FluentValidation.Validations;
using SE.Core.DTO;
using SE.Core.Utilities.Interceptors;
using SE.Data;


namespace SE.Business.Infrastructure.Autofac
{
    public class AutofacDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<EntitiesDbContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();



            builder.RegisterType<AttributeService>().As<IAttributeService>().InstancePerLifetimeScope().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            });
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            });
            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerLifetimeScope().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            });
            builder.RegisterType<EducationService>().As<IEducationService>().InstancePerLifetimeScope().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            });
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            });
            
            builder.RegisterType<ImageService>().As<IImageService>().InstancePerLifetimeScope().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            });

            builder.RegisterType<BlogService>().As<IBlogService>().InstancePerLifetimeScope().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            });
            builder.RegisterType<AttributeCategoryService>().As<IAttributeCategoryService>().InstancePerLifetimeScope().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            });

            builder.RegisterType<CommonService>().As<ICommonService>().InstancePerLifetimeScope().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            });
            builder.RegisterType<LoginDtoValidator>().As<IValidator<LoginDto>>().InstancePerLifetimeScope();
            builder.RegisterType<RegisterDtoValidator>().As<IValidator<RegisterDto>>().InstancePerLifetimeScope();
            builder.RegisterType<EducationInsertDtoValidator>().As<IValidator<EducationInsertDto>>().InstancePerLifetimeScope();
            builder.RegisterType<EducationInsertDtoValidator>().As<IValidator<EducationInsertDto>>().InstancePerLifetimeScope();
            builder.RegisterType<EducationUpdateDtoValidator>().As<IValidator<EducationUpdateDto>>().InstancePerLifetimeScope();
            builder.RegisterType<BlogInsertDtoValidator>().As<IValidator<BlogInsertDto>>().InstancePerLifetimeScope();
            builder.RegisterType<BlogUpdateDtoValidator>().As<IValidator<BlogUpdateDto>>().InstancePerLifetimeScope();






        }
    }
}
