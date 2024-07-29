using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Parsers;
using ConfigurationReader.Infrastructure.Services;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using ConfigurationReader.Tests.Services;
using ConfigurationReader.Tests.Services.Interface;
using Microsoft.Extensions.Logging;
using Moq;
using ConfigurationReader.Infrastructure.Extensions;

namespace ConfigurationReader.Tests.Tests.ServicesTests;

public class ConfigurationServiceTests
{
    private readonly Mock<IFileService> _mockFileService;
    private readonly Mock<IConfigurationParserFactory> _mockConfigurationParserFactory;
    private readonly Mock<ILogger<CsvConfigurationParser>> _mockCsvLogger;
    private readonly Mock<ILogger<XmlConfigurationParser>> _mockXmlLogger;
    private readonly ConfigurationService _configurationService;
    private readonly ITestService _testService = new TestService();

    public ConfigurationServiceTests()
    {
        _mockFileService = new Mock<IFileService>();
        _mockCsvLogger = new Mock<ILogger<CsvConfigurationParser>>();
        _mockXmlLogger = new Mock<ILogger<XmlConfigurationParser>>();
        _mockConfigurationParserFactory = new Mock<IConfigurationParserFactory>();
        _configurationService = new ConfigurationService(_mockFileService.Object, _mockConfigurationParserFactory.Object);
    }

    #region GetConfigurationsFromDirectoryPath

    [Fact]
    public async Task GetConfigurationsFromDirectoryPath_AllFilesCorrect_ReturnsConfigurations()
    {
        var directoryPath = "test_directory";
        var files = new List<FileDto>
        {
            new FileDto("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath()),
            new FileDto("file1.xml", ".xml", $"{directoryPath}\\file1.xml".GetPlatformSpecificPath()),
        };
        var filesPaths = files.Select(x => x.FilePath).ToArray();
        
        _testService.SetupTestDirectory(directoryPath, filesPaths);

        var configuration = new Configuration { };

        ConfigureMocks(configuration, directoryPath, files);

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
            new FileDto("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath()),
            new FileDto("file1.xml", ".xml", $"{directoryPath}\\file1.xml".GetPlatformSpecificPath()),
        };
        var filesPaths = files.Select(x => x.FilePath).ToArray();

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        var configuration = new Configuration { };

        ConfigureMocks(configuration, directoryPath, files);

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
            new FileDto("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath()),
            new FileDto("file1.xml", ".xml", $"{directoryPath}\\file1.xml".GetPlatformSpecificPath()),
            new FileDto("file1.unknown", ".unknown", $"{directoryPath}\\file1.unknown".GetPlatformSpecificPath()),
        };
        var filesPaths = files.Select(x => x.FilePath).ToArray();

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        var configuration = new Configuration { };

        ConfigureMocks(configuration, directoryPath, files);

        var exception =
            await Assert.ThrowsAsync<Exception>(
                async () => await _configurationService.GetConfigurationFromFilesPaths(filesPaths));

        Assert.Equal(
            string.Format(AllConsts.Errors.ParsingFileHasError, files.Last().FilePath,
                AllConsts.Errors.FileFormatNotAvailableForParsing), exception.Message);

        _testService.CleanupTestDirectory(directoryPath);
    }

    #endregion

    #region GetConfigurationFromFilePath

    [Fact]
    public void GetConfigurationFromFilePath_AllFilesCorrect_ReturnsConfiguration()
    {
        var directoryPath = "test_directory";
        var file = new FileDto("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath());

        var filesPaths = new string[] { file.FilePath };

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        var configuration = new Configuration { };

        ConfigureMocks(configuration, directoryPath, new List<FileDto>() { file });

        var result = _configurationService.GetConfigurationFromFilePathAsync(file.FilePath);

        Assert.NotNull(result);

        _testService.CleanupTestDirectory(directoryPath);
    }

    [Fact]
    public async Task GetConfigurationFromFilePath_NullConfiguration_ReturnsNull()
    {
        var directoryPath = "test_directory";
        var file = new FileDto("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath());

        var filesPaths = new string[] { file.FilePath };
        Configuration configuration = null;

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        ConfigureMocks(configuration, directoryPath, new List<FileDto>(){ file });

        var result = await _configurationService.GetConfigurationFromFilePathAsync(file.FilePath);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetConfigurationFromFilePath_ParserThrowsException_ThrowsException()
    {
        var directoryPath = "test_directory";
        var file = new FileDto("file1.csv", ".csv", $"{directoryPath}\\file1.csv".GetPlatformSpecificPath());

        var filesPaths = new string[] { file.FilePath };
        Configuration configuration = null;

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        _mockFileService.Setup(fs => fs.GetFileFromFilePath(file.FilePath)).Returns(file);

        var parserCsvMock = new Mock<CsvConfigurationParser>(_mockCsvLogger.Object);
        parserCsvMock
            .Setup(p => p.ParseAsync(It.IsAny<byte[]>()))
            .Throws(() =>
                new ParserAlgorithmException(AllConsts.Errors.HasErrorInParsingAlgorithm));

        _mockConfigurationParserFactory
            .Setup(pf => pf.CreateParser(ConfigurationFileType.Csv))
            .Returns(parserCsvMock.Object);

        var exception =
            await Assert.ThrowsAsync<Exception>(
                async () => await _configurationService.GetConfigurationFromFilePathAsync(file.FilePath));
        
        Assert.Equal(
            string.Format(AllConsts.Errors.ParsingFileHasError, file.FilePath,
                AllConsts.Errors.HasErrorInParsingAlgorithm), exception.Message);
    }

    #endregion

    #region Helper Methods

    private void ConfigureMocks(Configuration configuration, string directoryPath, List<FileDto> files)
    {
        ConfigureFileServiceMocks(directoryPath, files);

        ConfigureParserMocks(configuration);
    }

    private void ConfigureParserMocks(Configuration configuration)
    {
        var parserCsvMock = new Mock<CsvConfigurationParser>(_mockCsvLogger.Object);
        parserCsvMock.Setup(p => p.ParseAsync(It.IsAny<byte[]>())).ReturnsAsync(configuration);

        var parserXmlMock = new Mock<CsvConfigurationParser>(_mockCsvLogger.Object);
        parserXmlMock.Setup(p => p.ParseAsync(It.IsAny<byte[]>())).ReturnsAsync(configuration);

        _mockConfigurationParserFactory
            .Setup(pf => pf.CreateParser(ConfigurationFileType.Csv))
            .Returns(parserCsvMock.Object);

        _mockConfigurationParserFactory
            .Setup(pf => pf.CreateParser(ConfigurationFileType.Xml))
            .Returns(parserXmlMock.Object);
    }

    private void ConfigureFileServiceMocks(string directoryPath, List<FileDto> files)
    {
        _mockFileService
            .Setup(fs => fs.GetFilesFromDirectoryPath(directoryPath))
            .Returns(files);

        var path = files.First().FilePath;
        _mockFileService
            .Setup(fs => fs.GetFileFromFilePath(path))
            .Returns(files.First);

        var filesPaths = files.Select(x => x.FilePath).ToArray();
        _mockFileService
            .Setup(fs => fs.GetFilesFromFilesPaths(filesPaths))
            .Returns(files);
    }

    #endregion
}