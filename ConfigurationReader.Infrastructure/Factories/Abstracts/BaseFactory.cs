using ConfigurationReader.Infrastructure.Consts;

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

        var instance = (TInstance)_serviceProvider.GetService(instanceType);

        if (instance is null)
            throw new Exception(string.Format(AllConsts.Errors.CantCreateInstanceOfType, instanceType));

        return instance;
    }
}