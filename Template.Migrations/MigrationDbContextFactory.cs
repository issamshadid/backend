using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Template.Configurations.ConfigurationReader;
using Template.DataAccess;

namespace Template.Migrations;

/// <summary>
///     AppDbContext factory used in migrations.
///     The application needs to have an appsettings.json file that contains an appSettings key "db:template" with the sql
///     connection string.
/// </summary>
/// <remarks>
///     The sql connection string is useful when migrating backwards.
/// </remarks>
public class MigrationDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(
        string[] args)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

        var configuration = configurationBuilder.Build();
        var configurationReader = new ConfigurationReader(configuration);

        var sqlConnectionString = args.Length != 1 ? configurationReader.GetConnectionString() : args[0];

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(sqlConnectionString,
            b =>
            {
                b.MigrationsAssembly("Template.Migrations");
                b.EnableRetryOnFailure(configurationReader.GetDbConnectionMaxRetryCount(),
                    TimeSpan.FromSeconds(configurationReader.GetDbConnectionMaxRetryDelay()), null);
            });
        // Manually create the AuditInterceptor for use during design-time migration
        var auditInterceptor = new AuditInterceptor(new HttpContextAccessor());
        // Add the interceptor to the options
        builder.AddInterceptors(auditInterceptor);

        return new AppDbContext(builder.Options, auditInterceptor);
    }
}