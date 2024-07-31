using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationReader.Tests.DI;

internal class Resolver
{
    private static readonly IServiceProvider ServiceProvider;
    private static readonly object Lock = new();

    static Resolver()
    {
        ServiceProvider = RegisterConfiguration.RegisterDependency();
    }

    public static T Resolve<T>() where T : notnull
    {
        lock (Lock)
        {
            using var scope = ServiceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}