using Autofac;
using Microsoft.EntityFrameworkCore;
using Template.Business.Managers;
using Template.DataAccess;
using Template.DataAccess.Repositories;

namespace Template.Business.Infrastructure;

public class DependencyInjectionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        #region Managers

        builder
            .RegisterType<CategoryManager>()
            .As<ICategoryWebManager>()
            .InstancePerLifetimeScope();

        #endregion

        #region Repositories

        builder
            .RegisterGeneric(typeof(Repository<,>))
            .As(typeof(IRepository<,>))
            .InstancePerLifetimeScope();

        builder
            .RegisterGeneric(typeof(Repository<>))
            .As(typeof(IRepository<>))
            .InstancePerLifetimeScope();

        builder
            .RegisterType<CategoryRepository>()
            .As<ICategoryRepository>()
            .InstancePerLifetimeScope();

        #endregion

        #region DbContext

        builder.RegisterType<CustomerAwareDbContextFactory>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder
            .Register(t => t.Resolve<IAppDbContextFactory<AppDbContext>>().Create())
            .As<AppDbContext>()
            .InstancePerLifetimeScope();

        builder
            .Register(t => t.Resolve<IAppDbContextFactory<AppDbContext>>().Create())
            .As<DbContext>()
            .InstancePerLifetimeScope();

        #endregion
    }
}