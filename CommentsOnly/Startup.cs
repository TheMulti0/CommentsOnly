using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommentsOnly
{
    public class Startup
    {
        private static Task Main(string[] args)
        {
            return new HostBuilder()
                .ConfigureAppConfiguration(ConfigureConfiguration)
                .ConfigureLogging(builder => builder.AddConsole())
                .ConfigureServices(ConfigureServices)
                .RunConsoleAsync();
        }

        private static void ConfigureConfiguration(IConfigurationBuilder builder)
        {
            string environmentName =
                Environment.GetEnvironmentVariable("ENVIRONMENT");

            const string fileName = "appsettings";
            const string fileType = "json";

            string basePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                Environment.GetEnvironmentVariable("CONFIG_DIRECTORY") ?? string.Empty);

            builder
                .SetBasePath(basePath)
                .AddJsonFile($"{fileName}.{fileType}", false)
                .AddJsonFile($"{fileName}.{environmentName}.{fileType}", true) // Overrides default appsettings.json
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables();
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services
                .AddTelegramBotClient()
                .AddSingleton<IUpdatesHandler, UpdatesHandler>()
                .AddSingleton<IMessagesHandler, StartCommand>()
                .AddHostedService<TelegramBotService>();
        }
    }
}