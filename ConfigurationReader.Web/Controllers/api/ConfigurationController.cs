using System.Diagnostics;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Resources;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationReader.Web.Controllers.api;

/// <summary>
///     Контроллер по работе с конфигурациями
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConfigurationController : ControllerBase
{
    private readonly IConfigurationService _configurationService;
    private readonly ILogger<ConfigurationController> _logger;

    /// <inheritdoc />
    public ConfigurationController(IConfigurationService configurationService, ILogger<ConfigurationController> logger)
    {
        _configurationService = configurationService;
        _logger = logger;
    }

    /// <summary>
    ///     Получить все конфигурации с пути
    /// </summary>
    /// <param name="directoryPath">Путь до файлов</param>
    [HttpGet]
    [Route("GetConfigurationsFromDirectoryPath")]
    [ProducesResponseType(typeof(List<Configuration>), 200)]
    public async Task<IActionResult> GetConfigurationsFromDirectoryPath(string directoryPath)
    {
        _logger.LogInformation(string.Format(TracingMessages.MethodStarted,
            nameof(GetConfigurationsFromDirectoryPath), directoryPath));
        var stopWatch = new Stopwatch();

        try
        {
            stopWatch.Start();
            var configurations = await _configurationService.GetConfigurationsFromDirectoryPath(directoryPath);
            stopWatch.Stop();

            _logger.LogInformation(string.Format(TracingMessages.MethodFinished,
                nameof(GetConfigurationsFromDirectoryPath), stopWatch.Elapsed));

            return Ok(configurations);
        }
        catch (Exception e)
        {
            var message = string.Format(ErrorMessages.ProcessingPathsHasErrors, e.Message);

            _logger.LogError(message);

            return BadRequest(message);
        }
    }

    /// <summary>
    ///     Получить конфигурацию с путей файлов
    /// </summary>
    /// <param name="filesPaths"> Пути к файлам</param>
    [HttpGet]
    [Route("GetConfigurationFromFilesPaths")]
    [ProducesResponseType(typeof(List<Configuration>), 200)]
    public async Task<IActionResult> GetConfigurationFromFilesPaths(string[] filesPaths)
    {
        _logger.LogInformation(string.Format(TracingMessages.MethodStarted,
            nameof(GetConfigurationFromFilesPaths), string.Join("; ", filesPaths)));
        var stopWatch = new Stopwatch();

        try
        {
            stopWatch.Start();
            var configurations = await _configurationService.GetConfigurationFromFilesPaths(filesPaths);
            stopWatch.Stop();

            _logger.LogInformation(string.Format(TracingMessages.MethodFinished,
                nameof(GetConfigurationsFromDirectoryPath), stopWatch.Elapsed));

            return Ok(configurations);
        }
        catch (Exception e)
        {
            var message = string.Format(ErrorMessages.ProcessingPathsHasErrors, e.Message);

            _logger.LogError(message);

            return BadRequest(message);
        }
    }

    /// <summary>
    ///     Получить конфигурацию с пути файла
    /// </summary>
    /// <param name="filePath">Путь до файла</param>
    [HttpGet]
    [Route("GetConfigurationFromFilePath")]
    [ProducesResponseType(typeof(Configuration), 200)]
    public async Task<IActionResult> GetConfigurationFromFilePath(string filePath)
    {
        _logger.LogInformation(string.Format(TracingMessages.MethodStarted,
            nameof(GetConfigurationFromFilesPaths), filePath));
        var stopWatch = new Stopwatch();

        try
        {
            stopWatch.Start();
            var configuration = await _configurationService.GetConfigurationFromFilePathAsync(filePath);
            stopWatch.Stop();

            _logger.LogInformation(string.Format(TracingMessages.MethodFinished,
                nameof(GetConfigurationsFromDirectoryPath), stopWatch.Elapsed));

            return Ok(configuration);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);

            return BadRequest(e.Message);
        }
    }
}