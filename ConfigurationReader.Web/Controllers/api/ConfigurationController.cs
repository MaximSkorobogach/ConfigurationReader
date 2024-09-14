using System.Diagnostics;
using System.Net;
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
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ConfigurationController : ControllerBase
{
    private readonly IConfigurationService _configurationService;

    /// <inheritdoc />
    public ConfigurationController(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    /// <summary>
    ///     Получить все конфигурации с пути
    /// </summary>
    /// <param name="directoryPath">Путь до файлов</param>
    [HttpGet]
    [Route("GetConfigurationsFromDirectoryPath")]
    [ProducesResponseType(typeof(List<Configuration>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Configuration>>> GetConfigurationsFromDirectoryPath(string directoryPath)
    {
        var configurations = await _configurationService.GetConfigurationsFromDirectoryPath(directoryPath);
        return Ok(configurations);
    }

    /// <summary>
    ///     Получить конфигурацию с путей файлов
    /// </summary>
    /// <param name="filesPaths"> Пути к файлам</param>
    [HttpGet]
    [Route("GetConfigurationFromFilesPaths")]
    [ProducesResponseType(typeof(List<Configuration>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Configuration>>> GetConfigurationFromFilesPaths([FromQuery]string[] filesPaths)
    {
        var configurations = await _configurationService.GetConfigurationFromFilesPaths(filesPaths);
        return Ok(configurations);
    }

    /// <summary>
    ///     Получить конфигурацию с пути файла
    /// </summary>
    /// <param name="filePath">Путь до файла</param>
    [HttpGet]
    [Route("GetConfigurationFromFilePath")]
    [ProducesResponseType(typeof(Configuration), StatusCodes.Status200OK)]
    public async Task<ActionResult<Configuration>> GetConfigurationFromFilePath(string filePath)
    {
        var configuration = await _configurationService.GetConfigurationFromFilePathAsync(filePath);
        return Ok(configuration);
    }
}