using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Parsers;
using ConfigurationReader.Infrastructure.Resources;
using ConfigurationReader.Tests.Services;
using ConfigurationReader.Tests.Services.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConfigurationReader.Tests.Tests.ParsersTests;

public class CsvConfigurationParserTests
{
    private readonly Mock<ILogger<CsvConfigurationParser>> _loggerMock;
    private readonly CsvConfigurationParser _parser;
    private readonly ITestService _testService = new TestService();

    public CsvConfigurationParserTests()
    {
        _loggerMock = new Mock<ILogger<CsvConfigurationParser>>();
        _parser = new CsvConfigurationParser(_loggerMock.Object);
    }

    [Fact]
    public async Task GetConfigurationRecord_CorrectCsvConfig_ReturnsConfiguration()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("CorrectCsvConfig.csv");

        var fileBytes = File.ReadAllBytes(testConfigFullPath);

        var result = await _parser.ParseAsync(fileBytes);

        Assert.NotNull(result);
        Assert.Equal("Конфигурация 2", result.Name);
        Assert.Equal("Описание Конфигурации 2", result.Description);
    }

    [Fact]
    public async Task GetConfigurationRecord_HalfFilledCsvConfig_ThrowsParserAlgorithmException()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("HalfFilledCsvConfig.csv");

        var fileBytes = await File.ReadAllBytesAsync(testConfigFullPath);

        var exception =
            await Assert.ThrowsAsync<ParserAlgorithmException>(
                async () => await _parser.ParseAsync(fileBytes));

        Assert.Equal(string.Format(ErrorMessages.CreatedConfigurationIsNotFilled, nameof(CsvConfigurationParser)), exception.Message);
    }

    [Fact]
    public async Task GetConfigurationRecord_NotFilledCsvConfig_ThrowsParserAlgorithmException()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("NotFilledCsvConfig.csv");

        var fileBytes = await File.ReadAllBytesAsync(testConfigFullPath);

        var exception = 
            await Assert.ThrowsAsync<ParserAlgorithmException>(
                async () => await _parser.ParseAsync(fileBytes));

        Assert.Equal(string.Format(ErrorMessages.CreatedConfigurationIsNull, nameof(CsvConfigurationParser)), exception.Message);
    }
}