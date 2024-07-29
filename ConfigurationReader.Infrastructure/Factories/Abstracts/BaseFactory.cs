using ConfigurationReader.Infrastructure.Resources;

namespace ConfigurationReader.Infrastructure.Factories.Abstracts;

public abstract class BaseFactory
{
    private readonly IServiceProvider _serviceProvider;

    protected BaseFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected virtual TInstance CreateInstance<TInstance>() where TInstance : class
    {
        var instanceType = typeof(TInstance);

        if (_serviceProvider.GetService(instanceType) is not TInstance instance)
            throw new Exception(string.Format(ErrorMessages.CantCreateInstanceOfType, instanceType));

        return instance;
    }
}