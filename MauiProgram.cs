using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TruthOrDrinkDemiBruls.Database;

namespace TruthOrDrinkDemiBruls
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

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
