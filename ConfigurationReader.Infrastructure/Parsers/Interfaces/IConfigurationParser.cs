using ConfigurationReader.Infrastructure.DTO;

namespace ConfigurationReader.Infrastructure.Parsers.Interfaces;

public interface IConfigurationParser
{
    Task<Configuration> ParseAsync(byte[] fileBytes);
}