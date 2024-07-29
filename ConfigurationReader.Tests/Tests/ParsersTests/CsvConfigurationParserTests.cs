using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Parsers;
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
    public void GetConfigurationRecord_CorrectCsvConfig_ReturnsConfiguration()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("CorrectCsvConfig.csv");

        var fileBytes = File.ReadAllBytes(testConfigFullPath);

        var result = _parser.Parse(fileBytes);

        Assert.NotNull(result);
        Assert.Equal("Конфигурация 2", result.Name);
        Assert.Equal("Описание Конфигурации 2", result.Description);
    }

    [Fact]
    public void GetConfigurationRecord_HalfFilledCsvConfig_ThrowsParserAlgorithmException()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("HalfFilledCsvConfig.csv");

        var fileBytes = File.ReadAllBytes(testConfigFullPath);

        var exception = Assert.Throws<ParserAlgorithmException>(() => _parser.Parse(fileBytes));

        Assert.Equal(string.Format(AllConsts.Errors.CreatedConfigurationIsNotFilled, nameof(CsvConfigurationParser)), exception.Message);
    }

    [Fact]
    public void GetConfigurationRecord_NotFilledCsvConfig_ThrowsParserAlgorithmException()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("NotFilledCsvConfig.csv");

        var fileBytes = File.ReadAllBytes(testConfigFullPath);

        var exception = Assert.Throws<ParserAlgorithmException>(() => _parser.Parse(fileBytes));

        Assert.Equal(string.Format(AllConsts.Errors.CreatedConfigurationIsNull, nameof(CsvConfigurationParser)), exception.Message);
    }
}