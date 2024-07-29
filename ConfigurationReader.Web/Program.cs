using ConfigurationReader.Infrastructure.Services;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using System.Reflection;
using ConfigurationReader.Infrastructure.Factories;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Parsers;

namespace ConfigurationReader.Web
{
    /// <summary>
    /// ¬ходна€ точка проекта, регистраци€ зависимостей
    /// </summary>
    public class Program
    {
        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            ConfigureServices(builder.Services);
            ConfigureFactories(builder.Services);
            ConfigureParsers(builder.Services);

            var app = builder.Build();

            app.UseSwagger(); 
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureParsers(IServiceCollection builderServices)
        {
            builderServices.AddTransient<XmlConfigurationParser>();
            builderServices.AddTransient<CsvConfigurationParser>();
        }

        private static void ConfigureFactories(IServiceCollection builderServices)
        {
            builderServices.AddSingleton<IConfigurationParserFactory, ConfigurationParserFactory>();
        }

        private static void ConfigureServices(IServiceCollection builderServices)
        {
            builderServices.AddScoped<IFileService, FileService>();
            builderServices.AddScoped<IConfigurationService, ConfigurationService>();
        }
    }
}
