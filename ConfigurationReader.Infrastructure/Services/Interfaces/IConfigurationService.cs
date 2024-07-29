using ConfigurationReader.Infrastructure.DTO;

namespace ConfigurationReader.Infrastructure.Services.Interfaces;

public interface IConfigurationService
{
    Task<List<Configuration>> GetConfigurationsFromDirectoryPath(string path);
    Task<List<Configuration>> GetConfigurationFromFilesPaths(string[] filesPaths);
    Task<Configuration?> GetConfigurationFromFilePathAsync(string filePath);
}