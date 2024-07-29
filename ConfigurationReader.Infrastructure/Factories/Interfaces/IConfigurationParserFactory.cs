using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Parsers.Interfaces;

namespace ConfigurationReader.Infrastructure.Factories.Interfaces;

public interface IConfigurationParserFactory
{
    IConfigurationParser CreateParser(ConfigurationFileType configurationFileType);
}