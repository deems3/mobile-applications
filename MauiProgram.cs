using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TruthOrDrinkDemiBruls.Client;
using TruthOrDrinkDemiBruls.Config;
using TruthOrDrinkDemiBruls.Database;
using TruthOrDrinkDemiBruls.Views;

namespace TruthOrDrinkDemiBruls
{
    public static class MauiProgram
    {
        public static void Main(string[] args)
        {
            // Placeholder to satisfy the runtime.
        }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Because paths work differently when deploying to android or windows, we need to get the appsettings file(s) directly from the assembly
            var executingAssembly = Assembly.GetExecutingAssembly();
            using var requiredAppsettings = executingAssembly.GetManifestResourceStream("TruthOrDrinkDemiBruls.appsettings.json")!;
            using var localAppsettings = executingAssembly.GetManifestResourceStream("TruthOrDrinkDemiBruls.appsettings.Local.json");

            // Add the required appsettings file to the configuration builder
            var config = new ConfigurationBuilder()
                        .AddJsonStream(requiredAppsettings);

            // If the local appsettings are not null, add them as well
            if (localAppsettings != null)
            {
                config.AddJsonStream(localAppsettings);
            }

            var builtConfig = config.Build();

            builder.Configuration.AddConfiguration(builtConfig);

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Config
            // Build the GiphyConfig, so the API key can be accessed through a config object in the code base so we won't have to keep calling the config directly and check if the config is present etc.
            builder.Services.AddSingleton<GiphyConfig>(_ =>
            {
                var config = builder.Configuration.GetRequiredSection("Giphy");
                return new GiphyConfig { ApiKey = config["ApiKey"]! };
            });

            // Register the HttpClient as singleton in the service collection so we can inject it via the constructor in the code base
            builder.Services.AddSingleton<HttpClient>();
            // Register the GiphyClient to the service collection so we can inject it via the constructor in the code base
            builder.Services.AddSingleton<GiphyClient>();
            builder.Services.AddDbContext<DatabaseContext>();
            var dbContext = new DatabaseContext();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();

            // Inject all pages that require the database

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
