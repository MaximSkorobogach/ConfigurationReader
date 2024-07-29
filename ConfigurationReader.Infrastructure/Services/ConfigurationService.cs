using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Extensions;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Resources;
using ConfigurationReader.Infrastructure.Services.Interfaces;

namespace ConfigurationReader.Infrastructure.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IFileService _fileService;
    private readonly IConfigurationParserFactory _configurationParserFactory;

    public ConfigurationService(IFileService fileService, IConfigurationParserFactory configurationParserFactory)
    {
        _fileService = fileService;
        _configurationParserFactory = configurationParserFactory;
    }

    public async Task<List<Configuration>> GetConfigurationsFromDirectoryPath(string directoryPath)
    {
        var files = _fileService.GetFilesFromDirectoryPath(directoryPath);
        return await GetConfigurationsFromFilesAsync(files, filesGetFromDirectoryPath: true);
    }

    public async Task<List<Configuration>> GetConfigurationFromFilesPaths(string[] filesPaths)
    {
        var files = _fileService.GetFilesFromFilesPaths(filesPaths);
        return await GetConfigurationsFromFilesAsync(files);
    }

    public async Task<Configuration?> GetConfigurationFromFilePathAsync(string filePath)
    {
        var file = _fileService.GetFileFromFilePath(filePath);
        return await TryGetConfigurationFromFileAsync(file);
    }

    private async Task<List<Configuration>> GetConfigurationsFromFilesAsync(List<FileDto> files,
        bool filesGetFromDirectoryPath = false)
    {
        var configurations = new List<Configuration>();
        var tasks = files.Select(async file =>
        {
            var configuration = await TryGetConfigurationFromFileAsync(file, 
                ignoreNotAvailableForParsing: filesGetFromDirectoryPath);
            return configuration;
        });

        var results = await Task.WhenAll(tasks);

        configurations.AddRange(results.OfType<Configuration>());

        return configurations;
    }

    private async Task<Configuration?> TryGetConfigurationFromFileAsync(FileDto file, bool ignoreNotAvailableForParsing = false)
    {
        Configuration? configuration = null;

        try
        {
            var configurationFileType = file.GetConfigurationFileType();

            if (configurationFileType is null)
            {
                if (ignoreNotAvailableForParsing)
                    return configuration;

                throw new Exception(ErrorMessages.FileFormatNotAvailableForParsing);
            }

            var parser = _configurationParserFactory.CreateParser(configurationFileType.Value);
            var fileBytes = await File.ReadAllBytesAsync(file.FilePath);
            configuration = await parser.ParseAsync(fileBytes);
        }
        catch (Exception e)
        {
            throw new Exception(string.Format(ErrorMessages.ParsingFileHasError, file.FilePath, e.Message));
        }

        return configuration;
    }
}
