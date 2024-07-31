using ConfigurationReader.Infrastructure.Factories;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Parsers;
using ConfigurationReader.Infrastructure.Services;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConfigurationReader.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddParsers()
            .AddFactories()
            .AddServices()
            .AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            });

        return serviceCollection;
    }

    private static IServiceCollection AddParsers(this IServiceCollection services)
    {
        services.AddTransient<XmlConfigurationParser>();
        services.AddTransient<CsvConfigurationParser>();

        return services;
    }

    private static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddSingleton<IConfigurationParserFactory, ConfigurationParserFactory>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IConfigurationService, ConfigurationService>();

        return services;
    }
}