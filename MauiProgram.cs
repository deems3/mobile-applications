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

            var executingAssembly = Assembly.GetExecutingAssembly();
            using var requiredAppsettings = executingAssembly.GetManifestResourceStream("TruthOrDrinkDemiBruls.appsettings.json")!;
            using var localAppsettings = executingAssembly.GetManifestResourceStream("TruthOrDrinkDemiBruls.appsettings.Local.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(requiredAppsettings);

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
            builder.Services.AddSingleton<GiphyConfig>(_ =>
            {
                var config = builder.Configuration.GetRequiredSection("Giphy");
                return new GiphyConfig { ApiKey = config["ApiKey"]! };
            });


            builder.Services.AddTransient<HttpClient>(_ => new());
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
