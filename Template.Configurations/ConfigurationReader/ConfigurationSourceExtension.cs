using Microsoft.Extensions.Configuration;

namespace Template.Configurations.ConfigurationReader;

public static class ConfigurationSourceExtension
{
    public static ConfigurationReader TemplateConfigurationReader(string configFile = "appsettings.json")
    {
        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configFile)
            .AddEnvironmentVariables();

        var configurationRoot = configurationBuilder.Build();

        return new ConfigurationReader(configurationRoot);
    }
}