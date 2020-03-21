using Autofac;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using SE.Business.AccountServices;
using SE.Business.AddressServices;
using SE.Business.AttributeServices;
using SE.Business.CategoryServices;
using SE.Business.EducationServices;
using SE.Business.EmailSenders;
using SE.Business.Infrastructure.FluentValidation.Validations;
using SE.Core.DTO;
using SE.Data;


namespace SE.Business.Infrastructure.Autofac
{
    public class AutofacDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region Data Access Dependency Registration

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            #endregion

            #region Business Dependency Registration

            builder.RegisterType<EntitiesDbContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<AttributeService>().As<IAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<EducationService>().As<IEducationService>().InstancePerLifetimeScope();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().As<IEmailService>().InstancePerLifetimeScope();
           


            #endregion

        }
    }
}
