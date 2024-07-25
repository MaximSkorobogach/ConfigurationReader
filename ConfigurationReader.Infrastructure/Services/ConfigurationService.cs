using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Extensions;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

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

    public List<Configuration> GetConfigurationsFromDirectoryPath(string directoryPath)
    {
        var files = _fileService.GetAllFilesFromDirectoryPath(directoryPath);
        return GetConfigurationsFromFiles(files);
    }

    public List<Configuration> GetConfigurationFromFilesPaths(string[] filesPaths)
    {
        var files = _fileService.GetFilesFromFilesPaths(filesPaths);
        return GetConfigurationsFromFiles(files);
    }

    public Configuration? GetConfigurationFromFilePath(string filePath)
    {
        var file = _fileService.GetFileFromFilePath(filePath);
        return TryGetConfigurationFromFile(file);
    }

    private List<Configuration> GetConfigurationsFromFiles(List<FileDto> files)
    {
        var configurations = new List<Configuration>();

        files.ForEach(file =>
        {

                var configuration = TryGetConfigurationFromFile(file, ignoreNotAvailableForParsing: true);

                if (configuration is not null)
                    configurations.Add(configuration);
        });

        return configurations;
    }

    private Configuration? TryGetConfigurationFromFile(FileDto file, bool ignoreNotAvailableForParsing = false)
    {
        Configuration configuration;

        try
        {
            var configurationFileType = file.FilePath.GetConfigurationFileTypeFromPath();

            if (configurationFileType is null)
            {
                if (ignoreNotAvailableForParsing)
                    return null;

                throw new Exception(AllConsts.Errors.FileFormatNotAvailableForParsing);
            }

            var parser = _configurationParserFactory.CreateParser(configurationFileType.Value);
            var fileBytes = File.ReadAllBytes(file.FilePath);
            configuration = parser.Parse(fileBytes);
        }
        catch (Exception e)
        {
            throw new Exception(string.Format(AllConsts.Errors.ParsingFileHasError, file.FilePath, e.Message));
        }

        return configuration;
    }
}
