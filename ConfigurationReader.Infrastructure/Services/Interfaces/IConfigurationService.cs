using ConfigurationReader.Infrastructure.DTO;

namespace ConfigurationReader.Infrastructure.Services.Interfaces;

public interface IConfigurationService
{
    List<Configuration> GetConfigurationsFromDirectoryPath(string path);
    List<Configuration> GetConfigurationFromFilesPaths(string[] filesPaths);
    Configuration? GetConfigurationFromFilePath(string filePath);
}