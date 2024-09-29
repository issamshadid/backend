using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Template.Business.Infrastructure;
using Template.Business.Persistence;
using Template.DataAccess;

namespace Template.Migrations;

public static class Program
{
    public static Task Main(string[] args)
    {
        if (args.Length <= 0)
            throw new ArgumentException("DB migration requires the connection string.");

        var dbConnectionString = args[0]; // 1st param is the db connection string

        async Task ExecuteDatabaseMigration()
        {
            await MigrateDatabaseAsync(dbConnectionString);
        }

        return ExecuteDatabaseMigration();
    }

    public static async Task MigrateDatabaseAsync(string dbConnectionString)
    {
        try
        {
            var container = GetContainer(dbConnectionString);

            await using var localScopedContainer = container.BeginLifetimeScope();
            Console.WriteLine("Migrating DB with connection string '{0}'...", dbConnectionString);
            await localScopedContainer.Resolve<AppDbContext>().Database.MigrateAsync();
            Console.WriteLine("Migration finished.");

            Console.WriteLine("Creating Reference Data....");
            ReferenceData.CreateAsync(localScopedContainer).Wait();
            Console.WriteLine("Creating Reference Data Done...");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetBaseException().Message, "Migration terminated unexpectedly");
            throw;
        }
    }

    private static IContainer GetContainer(string dbConnectionString)
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule<DependencyInjectionModule>();

        // DB
        builder
            .Register
            (t => t.Resolve<IDesignTimeDbContextFactory<AppDbContext>>()
                .CreateDbContext
                (new[]
                {
                    dbConnectionString
                }))
            .As<DbContext>()
            .As<AppDbContext>()
            .InstancePerDependency();

        builder.RegisterType<MigrationDbContextFactory>()
            .AsImplementedInterfaces()
            .SingleInstance();

        return builder.Build();
    }
}