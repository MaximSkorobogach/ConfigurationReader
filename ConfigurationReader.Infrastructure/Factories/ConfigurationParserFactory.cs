using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Extensions;
using ConfigurationReader.Infrastructure.Factories.Abstracts;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Parsers;
using ConfigurationReader.Infrastructure.Parsers.Abstracts;

namespace ConfigurationReader.Infrastructure.Factories;

public class ConfigurationParserFactory : BaseFactory, IConfigurationParserFactory
{
    public ConfigurationParserFactory(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public BaseConfigurationParser CreateParser(ConfigurationFileType configurationFileType)
    {
        return configurationFileType switch
        {
            ConfigurationFileType.xml => CreateInstance<XmlConfigurationParser>(),
            ConfigurationFileType.csv => CreateInstance<CsvConfigurationParser>(),
            _ => throw new Exception(string.Format(AllConsts.Errors.CantFindParserForThisConfigurationFormat,
                configurationFileType.GetName()))
        };
    }
}