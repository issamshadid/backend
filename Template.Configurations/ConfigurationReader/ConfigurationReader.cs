using Microsoft.Extensions.Configuration;

namespace Template.Configurations.ConfigurationReader;

public class ConfigurationReader : IConfigurationReader
{
    public ConfigurationReader(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void Reload()
    {
        var configRoot = (IConfigurationRoot)Configuration;
        configRoot.Reload();
    }

    public int GetDbConnectionMaxRetryCount()
    {
        return Get(ConfigurationKeys.MaxRetryCount, 4);
    }

    public int GetDbConnectionMaxRetryDelay()
    {
        return Get<int>(ConfigurationKeys.MaxRetryDelay);
    }

    public string GetConnectionString()
    {
        return Get<string>(ConfigurationKeys.DbConnectionString);
    }

    public string GetDbName()
    {
        return ConfigurationKeys.DbName;
    }

    public string GetJwtSettingsIssuer()
    {
        return Get<string>(ConfigurationKeys.JwtSettingsIssuer);
    }

    public string GetJwtSettingsAudience()
    {
        return Get<string>(ConfigurationKeys.JwtSettingsAudience);
    }

    public string GetJwtSettingsKey()
    {
        return Get<string>(ConfigurationKeys.JwtSettingsKey);
    }

    public T Get<T>(string key, T @default, bool throwExceptionIfEmpty = false) where T : IConvertible
    {
        return GetKey(key, @default, throwExceptionIfEmpty);
    }

    public T Get<T>(string key, bool throwExceptionIfEmpty = false) where T : IConvertible
    {
        return GetKey<T>(key, throwExceptionIfEmpty);
    }

    private T GetKey<T>(string key, bool throwExceptionIfEmpty) where T : IConvertible
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key), "Key name is required!");

        if (!IsKeyExist(key)) return default!;

        var value = Configuration.GetValue<T>(key);

        if (typeof(T) == typeof(string)
            && (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            && throwExceptionIfEmpty)
            throw new ArgumentOutOfRangeException(nameof(key), $"The value for key '{key}' can not be empty!");

        return value!;
    }

    private T GetKey<T>(string key, T @default, bool throwExceptionIfEmpty) where T : IConvertible
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key), "Key name is required!");

        var keyExist = IsKeyExist(key);

        if (!keyExist && @default == null)
            throw new ArgumentNullException(nameof(@default), "Default value is required! and cannot be null");

        // We read key value as string in case the type is integer and value is empty because the value will be 0 using Configuration.GetValue<T>(key)

        // Get general value for all customers as string.
        var valueAsString = !keyExist ? @default.ToString() : Configuration.GetValue<string>(key);

        // if data type is not string and value is null or empty, use default value.
        var value = !keyExist || IsNullOrEmptyAndNotStringType<T>(valueAsString!)
            ? @default
            : Configuration.GetValue<T>(key);

        // throw an exception the key type is tring, value is empty or null and throw exception param true.
        if (typeof(T) == typeof(string)
            && string.IsNullOrWhiteSpace(valueAsString)
            && throwExceptionIfEmpty)
            throw new ArgumentOutOfRangeException(nameof(key), $"The value for key '{key}' can not be empty!");

        return value!;
    }

    private bool IsKeyExist(string key)
    {
        return Configuration.GetSection(key).Exists();
    }

    private static bool IsNullOrEmptyAndNotStringType<T>(string value)
        where T : IConvertible
    {
        return string.IsNullOrEmpty(value) && typeof(T) != typeof(string);
    }
}