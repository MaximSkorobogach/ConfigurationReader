using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Extensions;
using ConfigurationReader.Infrastructure.Factories.Abstracts;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Parsers;
using ConfigurationReader.Infrastructure.Parsers.Interfaces;
using ConfigurationReader.Infrastructure.Resources;

namespace ConfigurationReader.Infrastructure.Factories;

internal class ConfigurationParserFactory(IServiceProvider serviceProvider)
    : BaseFactory(serviceProvider), IConfigurationParserFactory
{
    public IConfigurationParser CreateParser(ConfigurationFileType configurationFileType)
    {
        return configurationFileType switch
        {
            ConfigurationFileType.Xml => CreateInstance<XmlConfigurationParser>(),
            ConfigurationFileType.Csv => CreateInstance<CsvConfigurationParser>(),
            _ => throw new Exception(string.Format(ErrorMessages.CantFindParserForThisConfigurationFormat,
                configurationFileType.GetName()))
        };
    }
}