using ConfigurationReader.Infrastructure.DTO;

namespace ConfigurationReader.Infrastructure.Parsers.Interfaces;

public interface IConfigurationParser
{
    Configuration Parse(byte[] fileBytes);
}