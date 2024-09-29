using Microsoft.Extensions.Configuration;

namespace Template.Configurations.ConfigurationReader;

public interface IConfigurationReader
{
    IConfiguration Configuration { get; }
    public void Reload();
    int GetDbConnectionMaxRetryCount();
    int GetDbConnectionMaxRetryDelay();
    string GetConnectionString();
    string GetDbName();
    string GetJwtSettingsIssuer();
    string GetJwtSettingsAudience();
    string GetJwtSettingsKey();
    T Get<T>(string key, T @default, bool throwExceptionIfEmpty = false) where T : IConvertible;
    T Get<T>(string key, bool throwExceptionIfEmpty = false) where T : IConvertible;
}