using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Extensions;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Parsers.Interfaces;
using ConfigurationReader.Infrastructure.Resources;
using ConfigurationReader.Infrastructure.Services;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using ConfigurationReader.Tests.DI;
using ConfigurationReader.Tests.Services.Interface;
using Moq;
using System.Text;

namespace ConfigurationReader.Tests.Tests.ServicesTests;

public class ConfigurationServiceTests
{
    private readonly Mock<IConfigurationParserFactory> _mockConfigurationParserFactory = new();
    private readonly Mock<IFileService> _mockFileService = new();
    private readonly ITestService _testService = Resolver.Resolve<ITestService>();
    private readonly IConfigurationService _configurationService;

    public ConfigurationServiceTests()
    {
        _configurationService =
            new ConfigurationService(_mockFileService.Object, _mockConfigurationParserFactory.Object);
    }

    #region GetConfigurationsFromDirectoryPath

    [Fact]
    public async Task GetConfigurationsFromDirectoryPath_AllFilesCorrect_ReturnsConfigurations()
    {
        var directoryPath = "test_directory";
        var files = new List<FileDto>
        {
            new("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath()),
            new("file1.xml", ".xml", $"{directoryPath}\\file1.xml".GetPlatformSpecificPath())
        };
        var filesPaths = files.Select(x => x.FilePath).ToArray();

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        var configuration = new Configuration();

        ConfigureMocks(configuration, files);

        var result = await _configurationService.GetConfigurationsFromDirectoryPath(directoryPath);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        _testService.CleanupTestDirectory(directoryPath);
    }

    #endregion

    #region GetConfigurationFromFilesPaths

    [Fact]
    public async Task GetConfigurationFromFilesPaths_AllFilesCorrect_ReturnsConfigurations()
    {
        var directoryPath = "test_directory";
        var files = new List<FileDto>
        {
            new("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath()),
            new("file1.xml", ".xml", $"{directoryPath}\\file1.xml".GetPlatformSpecificPath())
        };
        var filesPaths = files.Select(x => x.FilePath).ToArray();

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        var configuration = new Configuration();

        ConfigureMocks(configuration, files);

        var result = await _configurationService.GetConfigurationFromFilesPaths(filesPaths);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        _testService.CleanupTestDirectory(directoryPath);
    }

    [Fact]
    public async Task GetConfigurationFromFilesPaths_AnyFileNotSupported_ThrowsException()
    {
        var directoryPath = "test_directory";
        var files = new List<FileDto>
        {
            new("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath()),
            new("file1.xml", ".xml", $"{directoryPath}\\file1.xml".GetPlatformSpecificPath()),
            new("file1.unknown", ".unknown", $"{directoryPath}\\file1.unknown".GetPlatformSpecificPath())
        };
        var filesPaths = files.Select(x => x.FilePath).ToArray();

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        var configuration = new Configuration();

        ConfigureMocks(configuration, files);

        var exception =
            await Assert.ThrowsAsync<ArrayFilesHaveException>(
                async () => await _configurationService.GetConfigurationFromFilesPaths(filesPaths));

        var errors = new List<string>()
        {
            string.Format(ErrorMessages.ParsingFileHasError, files.Last().FilePath,
                ErrorMessages.FileFormatNotAvailableForParsing)
        };

        Assert.Equal(CreateExceptedArrayFileError(errors), exception.Message);

        _testService.CleanupTestDirectory(directoryPath);
    }

    private string CreateExceptedArrayFileError(List<string> errors)
    {
        var errorMessage = new StringBuilder();
        errorMessage.AppendLine(ErrorMessages.ArrayFilesHaveErrors);

        foreach (var error in errors)
        {
            errorMessage.AppendLine(error);
        }

        return errorMessage.ToString();
    }

    #endregion

    #region GetConfigurationFromFilePath

    [Fact]
    public void GetConfigurationFromFilePath_AllFilesCorrect_ReturnsConfiguration()
    {
        var directoryPath = "test_directory";
        var file = new FileDto("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath());

        var filesPaths = new[] { file.FilePath };

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        var configuration = new Configuration();

        ConfigureMocks(configuration, [file]);

        var result = _configurationService.GetConfigurationFromFilePathAsync(file.FilePath);

        Assert.NotNull(result);

        _testService.CleanupTestDirectory(directoryPath);
    }

    [Fact]
    public async Task GetConfigurationFromFilePath_NullConfiguration_ReturnsNull()
    {
        var directoryPath = "test_directory";
        var file = new FileDto("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath());

        var filesPaths = new[] { file.FilePath };
        Configuration configuration = null!;

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        ConfigureMocks(configuration, [file]);

        var result = await _configurationService.GetConfigurationFromFilePathAsync(file.FilePath);

        Assert.Null(result);

        _testService.CleanupTestDirectory(directoryPath);
    }

    [Fact]
    public async Task GetConfigurationFromFilePath_ParserThrowsException_ThrowsException()
    {
        var directoryPath = "test_directory";
        var file = new FileDto("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath());

        var filesPaths = new[] { file.FilePath };

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        _mockFileService.Setup(fs => fs.GetFileFromFilePath(file.FilePath)).Returns(file);

        var mockParser = new Mock<IConfigurationParser>();
        mockParser
            .Setup(p => p.ParseAsync(It.IsAny<byte[]>()))
            .Throws(() =>
                new ParserAlgorithmException(ErrorMessages.HasErrorInParsingAlgorithm));

        _mockConfigurationParserFactory
            .Setup(pf => pf.CreateParser(ConfigurationFileType.Csv))
            .Returns(mockParser.Object);

        var exception =
            await Assert.ThrowsAsync<ProcessingFileException>(
                async () => await _configurationService.GetConfigurationFromFilePathAsync(file.FilePath));

        Assert.Equal(
            string.Format(ErrorMessages.ParsingFileHasError, file.FilePath,
                ErrorMessages.HasErrorInParsingAlgorithm), exception.Message);

        _testService.CleanupTestDirectory(directoryPath);
    }

    #endregion

    #region Helper Methods

    private void ConfigureMocks(Configuration configuration, List<FileDto> files)
    {
        ConfigureFileServiceMocks(files);

        ConfigureParserMocks(configuration);
    }

    private void ConfigureParserMocks(Configuration configuration)
    {
        var parserMock = new Mock<IConfigurationParser>();
        parserMock.Setup(p => p.ParseAsync(It.IsAny<byte[]>())).ReturnsAsync(configuration);

        _mockConfigurationParserFactory
            .Setup(pf => pf.CreateParser(It.IsAny<ConfigurationFileType>()))
            .Returns(parserMock.Object);
    }

    private void ConfigureFileServiceMocks(List<FileDto> files)
    {
        _mockFileService
            .Setup(fs => fs.GetFilesFromDirectoryPath(It.IsAny<string>()))
            .Returns(files);

        _mockFileService
            .Setup(fs => fs.GetFileFromFilePath(It.IsAny<string>()))
            .Returns(files.First());

        _mockFileService
            .Setup(fs => fs.GetFilesFromFilesPaths(It.IsAny<string[]>()))
            .Returns(files);
    }

    #endregion
}