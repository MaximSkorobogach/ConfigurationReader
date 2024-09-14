using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace ConfigurationReader.Web.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var queryString = GetQueryString(context);
            var requestBody = await GetRequestBodyAsync(context);

            var startLogString = BuildStartLogString(context, queryString, requestBody);
            _logger.LogInformation(startLogString);

            await _next(context);

            stopwatch.Stop();
            var endLogString = BuildEndLogString(context, stopwatch, queryString, requestBody);
            _logger.LogInformation(endLogString);

        }

        private string GetQueryString(HttpContext context)
        {
            return context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
        }

        private async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get || context.Request.ContentLength == 0)
                return string.Empty;

            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            return body;
        }

        private string BuildStartLogString(HttpContext context, string queryString, string requestBody)
        {
            var stringBuilder = new StringBuilder($"Начало обработки запроса: {context.Request.Path}");

            AddParametersToLogString(queryString, requestBody, stringBuilder);

            return stringBuilder.ToString();
        }

        private string BuildEndLogString(HttpContext context, Stopwatch stopwatch, string queryString, string requestBody)
        {
            var stringBuilder = new StringBuilder($"Обработка запроса: {context.Request.Path}");

            AddParametersToLogString(queryString, requestBody, stringBuilder);

            stringBuilder.Append($", завершена за {stopwatch.ElapsedMilliseconds} мс");

            return stringBuilder.ToString();
        }

        private static void AddParametersToLogString(string queryString, string requestBody, StringBuilder stringBuilder)
        {
            if(!string.IsNullOrWhiteSpace(queryString))
                stringBuilder.Append($", QueryString: {queryString}");

            if (!string.IsNullOrWhiteSpace(requestBody))
                stringBuilder.Append($", RequestBody: {requestBody}");
        }
    }
}
