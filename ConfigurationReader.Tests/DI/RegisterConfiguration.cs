using ConfigurationReader.Infrastructure.DI;
using ConfigurationReader.Tests.Services;
using ConfigurationReader.Tests.Services.Interface;
using ConfigurationReader.Tests.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationReader.Tests.DI;

internal static class RegisterConfiguration
{
    public static IServiceProvider RegisterDependency()
    {
        var serviceCollection = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        serviceCollection.Configure<TestSettings>(configuration.GetSection(nameof(TestSettings)));
        serviceCollection.AddInfrastructure();
        serviceCollection.AddScoped<ITestService, TestService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider;
    }
}