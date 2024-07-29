using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Extensions;
using ConfigurationReader.Infrastructure.Factories.Abstracts;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Parsers;
using ConfigurationReader.Infrastructure.Parsers.Interfaces;

namespace ConfigurationReader.Infrastructure.Factories;

public class ConfigurationParserFactory : BaseFactory, IConfigurationParserFactory
{
    public ConfigurationParserFactory(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public IConfigurationParser CreateParser(ConfigurationFileType configurationFileType)
    {
        return configurationFileType switch
        {
            ConfigurationFileType.Xml => CreateInstance<XmlConfigurationParser>(),
            ConfigurationFileType.Csv => CreateInstance<CsvConfigurationParser>(),
            _ => throw new Exception(string.Format(AllConsts.Errors.CantFindParserForThisConfigurationFormat,
                configurationFileType.GetName()))
        };
    }
}