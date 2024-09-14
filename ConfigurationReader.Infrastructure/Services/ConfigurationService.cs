using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Extensions;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Resources;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using System.Text;

namespace ConfigurationReader.Infrastructure.Services;

internal class ConfigurationService(IFileService fileService, IConfigurationParserFactory configurationParserFactory)
    : IConfigurationService
{
    public async Task<List<Configuration>> GetConfigurationsFromDirectoryPath(string directoryPath)
    {
        var files = fileService.GetFilesFromDirectoryPath(directoryPath);
        return await GetConfigurationsFromFilesAsync(files, true);
    }

    public async Task<List<Configuration>> GetConfigurationFromFilesPaths(string[] filesPaths)
    {
        var files = fileService.GetFilesFromFilesPaths(filesPaths);
        return await GetConfigurationsFromFilesAsync(files);
    }

    public async Task<Configuration?> GetConfigurationFromFilePathAsync(string filePath)
    {
        var file = fileService.GetFileFromFilePath(filePath);
        return await TryGetConfigurationFromFileAsync(file);
    }

    private async Task<List<Configuration>> GetConfigurationsFromFilesAsync(List<FileDto> files,
        bool filesGetFromDirectoryPath = false)
    {
        var configurations = new List<Configuration>();
        var errors = new List<string>();

        foreach (var fileDto in files)
        {
            try
            {
                var configuration = await TryGetConfigurationFromFileAsync(fileDto,
                    filesGetFromDirectoryPath);

                if (configuration is null)
                    continue;

                configurations.Add(configuration);
            }
            catch (Exception e)
            {
                errors.Add(e.Message);
            }
        }

        ThrowIfAnyError(errors);

        return configurations;
    }

    private void ThrowIfAnyError(List<string> errors)
    {
        if (!errors.Any()) return;

        var errorMessage = new StringBuilder();
        errorMessage.AppendLine(ErrorMessages.ArrayFilesHaveErrors);

        foreach (var error in errors)
        {
            errorMessage.AppendLine(error);
        }

        throw new ArrayFilesHaveException(errorMessage.ToString());
    }

    private async Task<Configuration?> TryGetConfigurationFromFileAsync(FileDto file,
        bool ignoreNotAvailableForParsing = false)
    {
        Configuration? configuration = null;

        try
        {
            var configurationFileType = file.GetConfigurationFileType();

            if (configurationFileType is null)
            {
                if (ignoreNotAvailableForParsing)
                    return configuration;

                throw new NoAvailableFileFormatException(ErrorMessages.FileFormatNotAvailableForParsing);
            }

            var parser = configurationParserFactory.CreateParser(configurationFileType.Value);
            var fileBytes = await File.ReadAllBytesAsync(file.FilePath);
            configuration = await parser.ParseAsync(fileBytes);
        }
        catch (Exception e)
        {
            throw new ProcessingFileException(string.Format(ErrorMessages.ParsingFileHasError, file.FilePath, e.Message), e);
        }

        return configuration;
    }
}