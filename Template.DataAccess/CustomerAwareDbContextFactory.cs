using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Configurations.ConfigurationReader;

namespace Template.DataAccess;

public class CustomerAwareDbContextFactory : IAppDbContextFactory<AppDbContext>
{
    private readonly IConfigurationReader _configReader;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     Constructor used in DI at runtime.
    /// </summary>
    /// <param name="configReader">Configuration.</param>
    /// <param name="serviceProvider"></param>
    public CustomerAwareDbContextFactory(
        IConfigurationReader configReader,
        IServiceProvider serviceProvider)
    {
        _configReader = configReader;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    ///     Factory used in DI at runtime.
    /// </summary>
    /// <returns>A new AppDbContext instance.</returns>
    public AppDbContext Create()
    {
        var dbConnectionString = GetDbConnectionString(_configReader);
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(dbConnectionString,
                b =>
                {
                    b.EnableRetryOnFailure(_configReader.GetDbConnectionMaxRetryCount(),
                        TimeSpan.FromSeconds(_configReader.GetDbConnectionMaxRetryDelay()), null);
                })
            .AddInterceptors(_serviceProvider.GetRequiredService<AuditInterceptor>())
            .Options;
        return new AppDbContext(dbOptions, _serviceProvider.GetRequiredService<AuditInterceptor>());
    }

    private static string GetDbConnectionString(
        IConfigurationReader configurationReader)
    {
        return configurationReader.GetConnectionString() + configurationReader.GetDbName();
    }
}