using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using ConfigurationReader.Infrastructure.Exceptions;

namespace ConfigurationReader.Web;

/// <summary>
/// Глобальная обработка исключений
/// </summary>
public class GlobalExceptionsFilter : IAsyncExceptionFilter
{
    private readonly ILogger<GlobalExceptionsFilter> _logger;

    public GlobalExceptionsFilter(ILogger<GlobalExceptionsFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// При получении исключений
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task OnExceptionAsync(ExceptionContext context)
    {
        _logger.LogCritical(context.Exception, "Ошибка: {Message}", context.Exception.Message);

        context.Result = context.Exception switch
        {
            PathException pathException => new NotFoundObjectResult(pathException.Message),
            _ => new ObjectResult("Внутренняя ошибка сервера: " + context.Exception.Message)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };

        context.ExceptionHandled = true;

        return Task.CompletedTask;
    }
}