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

            var app = builder.Build();

            app.UseSwagger(); 
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureFactories(IServiceCollection builderServices)
        {
            builderServices.AddTransient<IConfigurationParserFactory, ConfigurationParserFactory>();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IFileService, FileService>();
            serviceCollection.AddTransient<IConfigurationService, ConfigurationService>();
            serviceCollection.AddTransient<XmlConfigurationParser>();
            serviceCollection.AddTransient<CsvConfigurationParser>();
        }
    }
}
