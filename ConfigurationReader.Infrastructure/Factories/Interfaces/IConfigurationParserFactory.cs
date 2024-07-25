using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Parsers.Abstracts;

namespace ConfigurationReader.Infrastructure.Factories.Interfaces;

public interface IConfigurationParserFactory
{
    BaseConfigurationParser CreateParser(ConfigurationFileType configurationFileType);
}