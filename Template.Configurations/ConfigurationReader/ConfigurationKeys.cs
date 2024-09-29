namespace Template.Configurations.ConfigurationReader;

public static class ConfigurationKeys
{
    public const string DbName = "Template";
    public const string DbConnectionString = "db:template";
    public const string MaxRetryCount = "db:maxretrycount";
    public const string MaxRetryDelay = "db:maxretrydelay";
    public const string JwtSettingsIssuer = "JwtSettings:Issuer";
    public const string JwtSettingsAudience = "JwtSettings:Audience";
    public const string JwtSettingsKey = "JwtSettings:Key";
}