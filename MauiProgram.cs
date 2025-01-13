using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using System.Reflection;
using TruthOrDrinkDemiBruls.Client;
using TruthOrDrinkDemiBruls.Config;
using TruthOrDrinkDemiBruls.Database;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.Service;
using TruthOrDrinkDemiBruls.ViewModels;
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
                .UseLocalNotification()
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


            // Register the pages to the ServiceCollection
            builder.Services.AddSingleton<GameService>();
            builder.Services.AddSingleton<WaitPage>();
            builder.Services.AddSingleton<WaitingPageViewModel>();
            builder.Services.AddSingleton<Lobby>();
            builder.Services.AddSingleton<LobbyViewModel>();
            builder.Services.AddSingleton<Intensity>();
            builder.Services.AddSingleton<GameOptions>();
            builder.Services.AddSingleton<GameOptionsViewModel>();
            builder.Services.AddSingleton<GameViewModel>();
            builder.Services.AddSingleton<Questions>();
            builder.Services.AddSingleton<Themes>();
            builder.Services.AddSingleton<GameOverview>();


            // Register the HttpClient as singleton in the service collection so we can inject it via the constructor in the code base
            builder.Services.AddSingleton<HttpClient>();
            // Register the GiphyClient to the service collection so we can inject it via the constructor in the code base
            builder.Services.AddSingleton<GiphyClient>();
            builder.Services.AddDbContext<DatabaseContext>();
            var dbContext = new DatabaseContext();
            dbContext.Dispose();

            // Inject all pages that require the database

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var host = builder.Build();

            using var scope = host.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var pendingMigrations = db.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                db.Database.Migrate();
            }

            db.Database.EnsureCreated();

            if (!db.Themes.Any())
            {
                SeedThemes(db);
                db.SaveChanges();
            }

            if (!db.Questions.Any())
            {
                SeedQuestions(db);
                db.SaveChanges();
            }

            return host;
        }

        private static void SeedQuestions(DatabaseContext db)
        {
            SeedRomanceQuestions(db);
            SeedAdventureQuestions(db);
            SeedSecretsQuestions(db);
            SeedHobbiesQuestions(db);
            SeedFamilyQuestions(db);
            SeedChoicesQuestions(db);
            SeedFoodQuestions(db);
        }

        // Eten & drank
        private static void SeedFoodQuestions(DatabaseContext db)
        {
            var theme = db.Themes.First(x => x.Id == 7);
            db.Questions.AddRange(
                // Easy
                new Question { Description = "Wat is je favoriete ontbijt?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Hoe drink jij je koffie het liefst?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat eet je meestal als snack?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete soort pizza?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een gerecht gegeten dat je niet lekker vond, maar het toch geprobeerd hebt?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Eet je vaak groenten? Waarom wel of niet?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete drankje bij de lunch?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Houd je van zoet of zout eten?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Hoe vaak ga je uit eten?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete fastfood?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Welke soort soep eet je het liefst?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Ben je een fan van desserts? Zo ja, welk dessert is je favoriet?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete fruit?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Welke soort kaas vind jij het lekkerst?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Kook je vaak thuis? Wat maak je dan meestal?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Houd je van sushi?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete ijsje?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Eet je liever gezond of lekker? Waarom?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete maaltijd van de dag?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Hou je van koken voor anderen? Wat maak je dan?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },

                // Average
                new Question { Description = "Wat is de vreemdste combinatie van eten die je ooit hebt geprobeerd?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Als je maar één soort eten zou mogen eten voor de rest van je leven, wat zou het dan zijn?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Eet je wel eens iets dat anderen raar vinden?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het duurste gerecht dat je ooit hebt gegeten?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is de meest exotische smaak die je hebt geproefd?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Als je je leven lang één gerecht moet koken, wat zou je dan kiezen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Welke ingrediënten mogen absoluut niet ontbreken in je koelkast?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je wel eens een gerecht laten staan omdat het er niet goed uitzag?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is je geheime truc in de keuken?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Maak je zelf wel eens je eigen pasta?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete gerecht uit de Italiaanse keuken?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het vreemdste gerecht dat je ooit hebt gezien, maar nooit hebt geprobeerd?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het lekkerste gerecht dat je ooit hebt gegeten op vakantie?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Eet je wel eens volgens een dieet of voedingsplan?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is jouw favoriete manier om aardappelen te bereiden?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Ben je ooit een keer op een foodtruckfestival geweest? Wat vond je ervan?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Als je een onbekend gerecht zou moeten beschrijven aan iemand, hoe zou je het doen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste misser die je ooit hebt gemaakt in de keuken?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Eet je vaak vegetarisch of veganistisch?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },

                // Challenging
                new Question { Description = "Wat is het meest avontuurlijke gerecht dat je hebt geprobeerd, maar nooit meer zou willen eten?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de vreemdste voedseltrend die je ooit hebt gevolgd?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Als je jezelf zou moeten uitdagen om iets ongebruikelijks te eten, wat zou dat dan zijn?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Eet je wel eens in een Michelin-sterrenrestaurant? Wat is je ervaring?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Ben je bereid om insecten te eten? Waarom wel of niet?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het zwaarste gerecht dat je ooit hebt gegeten?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Eet je wel eens ‘extreme’ voeding zoals bijvoorbeeld extreem pittige gerechten?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het gevaarlijkste gerecht dat je ooit hebt geproefd?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een gerecht gegeten dat je lichamelijk ongemak bezorgde?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je een gerecht voorgeschoteld krijgt dat je absoluut niet lekker vindt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een exotisch vlees gegeten? Wat vond je ervan?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Welke ongewone gerechten eet je graag en zou je anderen aanraden?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je wel eens iets gegeten dat je niet goed begreep, maar wel nieuwsgierig was?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de meest gedurfde smaakcombinatie die je ooit hebt geprobeerd?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de meest onverwachte maaltijd die je ooit hebt gegeten op vakantie?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je denken van een dieet gebaseerd op alleen rauwe voeding?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Zou je ooit overwegen om een plantaardig dieet volledig te omarmen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is je ervaring met het eten van fermented food, zoals kimchi?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },

                // Daring
                new Question { Description = "Zou je ooit een vleesgerecht eten dat is bereid met een ander dier dan een koe, varken of kip?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is de gevaarlijkste smaak die je zou willen proberen, zelfs als het je niet lekker lijkt?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Eet je wel eens levende dieren als delicatessen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is de meest exotische maaltijd die je ooit hebt gegeten onder extreme omstandigheden?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Eet je ooit vlees of voedsel dat je lichaam niet volledig kan verteren?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je wordt uitgedaagd om iets extreem smaakvols te eten, maar het blijkt heel ongezond?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste voedselmisdaad die je ooit hebt gepleegd?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Zou je ooit proberen om je eigen voedsel in extreme omstandigheden te verkrijgen, zoals in de natuur?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is de meest onhygiënische maaltijd die je ooit hebt gegeten, maar waarvan je vond dat het de moeite waard was?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Zou je ooit een gerecht eten met ingrediënten waarvan je niet zeker weet waar ze vandaan komen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Als je moest kiezen tussen een gerecht met extreme kruiden of extreem ongewone ingrediënten, welke zou je dan kiezen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is de vreemdste uitdaging die je ooit hebt aangenomen als het om eten ging?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is de stoutste eetervaring die je ooit hebt gehad?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je denken als je wordt uitgedaagd om een gerecht te bereiden dat gevaarlijk of onethisch wordt beschouwd?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Zou je ooit overwegen om je dieet volledig te veranderen voor een uitdaging, bijvoorbeeld naar alleen rauw voedsel?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Zou je ooit deelnemen aan een eetwedstrijd waarbij extreem pittig eten betrokken is?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },

                // Extreme
                new Question { Description = "Zou je ooit overwegen om levende octopus of andere dieren in hun levende vorm te eten?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest gedurfde dat je zou eten, zelfs als het je gezondheid in gevaar zou kunnen brengen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Eet je weleens voedsel dat afkomstig is van een verboden of controversieel dier?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste extreme eetervaring die je ooit hebt gehad?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Als je wordt uitgedaagd om een maaltijd te eten met ingrediënten die direct schadelijk voor je gezondheid kunnen zijn, zou je dat doen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je wordt uitgedaagd om een gerecht te bereiden dat gevaarlijk of onethisch wordt beschouwd?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest extreme dat je zou eten als je niet wist waar het vandaan kwam?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Zou je ooit overwegen om je voedselinname te beperken tot alleen ongebruikelijke of niet-standaard voeding?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste extreme eetervaring die je ooit hebt gehad?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is de stoutste eetervaring die je ooit hebt gehad?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Zou je ooit een maaltijd eten die opzettelijk ontworpen is om je extreem te verrassen, zelfs als het je ongemakkelijk maakt?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het vreemdste gerecht dat je ooit hebt gegeten, zelfs als het schadelijk of gevaarlijk was?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je een gerecht voorgeschoteld krijgt dat je absoluut niet lekker vindt?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Zou je ooit overwegen om voedsel te eten dat via onorthodoxe methoden is verkregen, zoals van een onbekende bron?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest extreme gerecht dat je ooit hebt bereid, zelfs als het een risico voor je gezondheid betekende?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } }
            );
        }

        // Levenskeuzes en beslissingen
        private static void SeedChoicesQuestions(DatabaseContext db)
        {
            var theme = db.Themes.First(x => x.Id == 6);
            db.Questions.AddRange(
                // Easy
                new Question { Description = "Zou je liever altijd met een groep mensen zijn of liever alleen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je een dag niets hoeft te doen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is belangrijker voor jou: geld of vrije tijd?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Verkies je een rustige wandeling in het park of een drukke dag winkelen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je liever een boek lezen of een film kijken?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat voor soort vakantie zou je het liefst willen: avontuurlijk of ontspannen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Verkies je thuis koken of uit eten gaan?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je liever doen in je vrije tijd: iets creatiefs of iets sportiefs?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Kies je voor een georganiseerde dag of een spontaan avontuur?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat vind je prettiger: veel mensen om je heen of een rustige omgeving?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je liever je favoriete hobby elke dag doen of afwisselen met nieuwe activiteiten?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is belangrijker voor jou: comfort of avontuur?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Verkies je vakantie in de natuur of in een stad?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je liever een hobby hebben die veel tijd kost of eentje die snel te leren is?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat doe je liever in je vrije tijd: iets alleen of samen met anderen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je liever een vaste routine hebben of elke dag iets anders doen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je liever verhuizen naar een andere stad of een ander land?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is voor jou belangrijker: zekerheid of avontuur?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Verkies je een rustige ochtend of een productieve start van de dag?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je liever altijd binnen of altijd buiten zijn?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },

                // Average
                new Question { Description = "Hoe zou je reageren als je plotseling je baan zou verliezen?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je een kans krijgt om in een ander land te wonen?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is belangrijker voor jou in een relatie: communicatie of fysieke aantrekkingskracht?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je kiezen: een carrière in de stad of een rustig leven op het platteland?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je liever een hoge functie hebben met veel verantwoordelijkheid of een rustige baan?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je kiezen: een stabiel maar saai leven of een leven vol avontuur?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je ontdekt dat een vriend iets belangrijks voor je heeft verborgen?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Als je maar één ding in je leven kon veranderen, wat zou dat dan zijn?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je liever rijk zijn maar ongelukkig, of arm maar gelukkig?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je liever een jaar reizen of een jaar thuis blijven en je eigen bedrijf starten?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is belangrijker in het leven: het maken van herinneringen of het bereiken van doelen?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je onterecht beschuldigd wordt van iets ernstigs?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Kies je voor een veilige maar saaie keuze of voor risico en avontuur?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is voor jou belangrijker in een carrière: passie of salaris?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je ineens wereldberoemd wordt?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je in een situatie zou zitten waarin je niet kunt winnen?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je liever de waarheid weten, zelfs als het pijn doet, of liever in onzekerheid blijven?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je kiezen: je dromen volgen of een veilige keuze maken voor de toekomst?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je reageren als iemand je levensstijl afwijst?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },

                // Challenging
                new Question { Description = "Wat zou je doen als je een belangrijke keuze moet maken die je leven compleet verandert?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Als je een relatie zou moeten beëindigen, hoe zou je dat doen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je een ongeluk meemaakt en alles moet herstarten?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je je voelen als je belangrijke mensen in je leven moet loslaten?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je ooit bereid zijn om alles op te geven voor een nieuwe kans, zelfs als dat betekent dat je moet verhuizen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je ontdekt dat je alles hebt bereikt wat je wilde, maar het niet voldoening geeft?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je het gevoel hebt dat je geen controle meer hebt over je leven?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je je vrienden verlaten voor een carrièrekans die alles verandert?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je omgaan met het verliezen van je identiteit door omstandigheden?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je niet meer de persoon kunt zijn die je altijd was?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je kiezen: je dromen laten varen om een andere te helpen, of doorgaan met je eigen pad?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Als je je oude leven zou kunnen herstarten, wat zou je anders doen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je weet dat je maar een korte tijd te leven hebt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je ooit een drastic change in je leven overwegen als alles je niet gelukkig maakt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je een geliefde moet loslaten voor altijd?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je jezelf kunnen vergeven als je iemand ernstig gekwetst hebt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },

                // Daring
                new Question { Description = "Wat zou je doen als je het risico zou moeten nemen om al je bezittingen te verliezen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je ooit naar een land reizen zonder enige voorbereiding of kennis van de cultuur?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je je comfortzone volledig moet verlaten voor een groter doel?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je je droom kunnen najagen, zelfs als het betekent dat je alles moet opgeven?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je in een situatie komt waar er geen uitweg lijkt te zijn?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je je leven riskeren om iets te doen dat je altijd al hebt gewild?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je de kans kreeg om te ontsnappen uit je huidige leven en iets totaal anders te beginnen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je je grootste angst zou moeten confronteren?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je in een extreme situatie jezelf in gevaar brengen voor een hoger doel?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je moet kiezen tussen je principes en de druk van de samenleving?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },

                // Extreme
                new Question { Description = "Wat zou je doen als je geconfronteerd werd met een levensbedreigende situatie?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je ooit bereid zijn om alles op te geven, inclusief je eigen veiligheid, om te vechten voor gerechtigheid?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je gedwongen werd om een levensveranderende beslissing te nemen zonder enige garantie?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je jezelf opofferen voor het welzijn van anderen, zelfs als het betekent dat je leven in gevaar komt?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je moest kiezen tussen jezelf en je familie in een gevaarlijke situatie?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je voor je overtuigingen sterven als het nodig zou zijn?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je je leven moet riskeren voor iets groters dan jezelf?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je ooit bereid zijn om jezelf te vernietigen voor het welzijn van anderen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je in een extreme situatie in staat zijn om je eigen principes op te geven?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je geconfronteerd werd met een extreem dilemma, zonder mogelijkheid van terugkeer?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } }
            );
        }

        // Familie en vriendschap
        private static void SeedFamilyQuestions(DatabaseContext db)
        {
            var theme = db.Themes.First(x => x.Id == 5);
            db.Questions.AddRange(
                // Easy Questions
                new Question { Description = "Wat is je favoriete herinnering met je familie?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is een traditie in jouw familie die je altijd hebt gevolgd?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wie binnen je vriendenkring zou je het eerst bellen bij problemen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is het leukste wat je ooit hebt gedaan met je vrienden?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is jouw favoriete gezelschapsspel om met familie te spelen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Hoe zou je je relatie met je beste vriend(in) omschrijven?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is een eigenschap die je waardeert in je vrienden?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is het mooiste cadeau dat je ooit van je familie hebt gekregen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Hoe vaak breng je tijd door met je ouders?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is de beste vakantie die je ooit met je vrienden hebt gehad?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete familiebijeenkomst?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wie binnen je vriendenkring maakt je altijd aan het lachen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is een gemeenschappelijke eigenschap tussen jou en je vrienden?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Hoe zou je je ideale familiediner beschrijven?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is het mooiste moment dat je met je broer(s) of zus(sen) hebt gedeeld?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat vind je het leukste aan je beste vriend(in)?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Welke hobby doe je het liefst samen met een vriend?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat voor soort vakanties geniet je het meest met je familie?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat was het laatste moment waarop je je familie echt waardeerde?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Welke persoon in je familie ben je het meest mee verbonden?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },

                // Average Questions
                new Question { Description = "Wanneer was de laatste keer dat je iets belangrijks deelde met je vrienden?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een familiedynamiek gehad die je zou willen veranderen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Hoe zou je de rol van vrienden in je leven beschrijven?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je ouders een beslissing nemen die je niet begrijpt?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Welke rol speelt vriendschap in jouw persoonlijke ontwikkeling?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat was een moment in je leven waarop je familie je absoluut niet begreep?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een vriend verloren door een misverstand? Wat gebeurde er?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest verrassende dat je over je familie hebt geleerd?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is jouw definitie van echte vriendschap?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Als je moest kiezen, zou je dan liever tijd doorbrengen met je vrienden of je familie?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wanneer was de laatste keer dat je je kwetsbaar opstelde tegenover je familie?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat zou je veranderen aan je vriendschappen als je de kans had?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat voor soort familiegebeurtenis maakt je altijd het gelukkigst?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je moet kiezen tussen een familielid en een vriend?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Hoe denk je dat de vriendschappen die je hebt, je leven op lange termijn beïnvloeden?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je een vriend nodig had, maar hij of zij het niet voor je doet?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het moeilijkste besluit dat je ooit hebt moeten nemen voor je familie?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },

                // Challenging Questions
                new Question { Description = "Wat is het moeilijkste gesprek dat je ooit met je familie hebt gehad?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een geheim voor je beste vriend(in) gehad? Wat was het?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als een vriend je trouw beloofd, maar het breekt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Hoe reageer je als een familielid iets doet wat je niet kunt accepteren?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat was het moeilijkste moment in je relatie met je ouders?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je beste vriend(in) je niet meer vertrouwt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit moeten kiezen tussen je familie en je vrienden? Wat was het resultaat?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat was het zwaarste conflict dat je met een vriend hebt meegemaakt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Hoe zou je omgaan met een familielid die een beslissing neemt die je niet kunt steunen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Hoeveel zou je bereid zijn te geven voor een vriend die in nood verkeert?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je ontdekt dat je familie je altijd al heeft misleid?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je een vriend ontdekt die je vertrouwen heeft geschaad?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het moeilijkste dat je ooit voor een vriend hebt gedaan?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je een familielid in gevaar ziet, maar ze willen niet geholpen worden?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste opoffering die je ooit voor je familie hebt gemaakt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wanneer heb je je voor het laatst afgekeerd van een vriend om een reden die ze niet begrijpen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het belangrijkste dat je hebt geleerd van conflicten met familie?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },

                // Daring Questions
                new Question { Description = "Zou je bereid zijn om een familielid te confronteren over iets dat ze altijd hebben verborgen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als een vriend je bedriegt?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit tegen je familie gelogen over iets belangrijks?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Zou je bereid zijn je beste vriend(in) te vergeven na verraad?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je ontdekt dat je familie je altijd heeft misleid?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Zou je ooit afstand nemen van je familie als dat je zou helpen jezelf te redden?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je je vrienden volledig in de steek laat in een tijd van nood?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Zou je kunnen vergeven en vergeten als een familielid je in het verleden ernstig heeft gekwetst?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Zou je het kunnen verdragen om je eigen familie te betrappen op een grote leugen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Als je moest kiezen tussen een vriend of je familie, wie zou je kiezen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },

                // Extreme Questions
                new Question { Description = "Zou je ooit een vriend kunnen verraden om je eigen leven te redden?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Zou je ooit je familie kunnen verlaten als dat het enige was wat je geluk zou brengen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je ontdekt dat een familielid een misdrijf heeft gepleegd?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Zou je bereid zijn een leven te sacrificeren voor een vriend of familielid?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste geheim dat je voor je vrienden hebt verborgen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Zou je bereid zijn je familie volledig te ontkennen als ze je niet steunen in een belangrijke beslissing?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je het gevoel hebt dat je vrienden je leven bepalen in plaats van jijzelf?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit je eigen familie pijn gedaan om iemand anders te helpen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als een vriend je vraagt om een groot risico te nemen dat je familie in gevaar zou brengen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Zou je je eigen leven riskeren om iemand in je vriendenkring te redden?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } }
            );
        }

        // Hobby's & interesses
        private static void SeedHobbiesQuestions(DatabaseContext db)
        {
            var theme = db.Themes.First(x => x.Id == 4);

            db.Questions.AddRange(
                // Easy intensity
                new Question { Description = "Wat is je favoriete manier om je vrije tijd door te brengen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Welke hobby zou je willen leren als je tijd had?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete seizoen en waarom?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Heb je een favoriete tv-show? Waarom spreekt deze je aan?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is de laatste film die je hebt gezien en vond je die goed?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Als je een dag niets zou moeten doen, wat zou je dan doen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Heb je een favoriete sport? Waarom vind je het leuk?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete manier om te ontspannen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iets nieuws geprobeerd, zoals een hobby of sport? Wat was het?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Ben je ooit naar een muziekfestival geweest? Hoe was die ervaring?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete genre muziek?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Als je elke week een nieuwe hobby zou kiezen, welke zou dat zijn?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Ben je een ochtend- of avondmens?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is de beste vakantie die je ooit hebt gehad?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete gerecht om te koken?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is een hobby die je in je jeugd had, maar nu niet meer beoefent?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is de laatste activiteit die je hebt gedaan om te ontspannen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Lees je boeken? Zo ja, welk boek raad je aan?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat zou je willen kunnen doen als hobby, maar is het niet mogelijk?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },

                // Average intensity
                new Question { Description = "Wat was de grootste uitdaging die je hebt gehad in je hobby's?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iets gedaan wat je helemaal niet leuk vond, maar waarvan je nu denkt dat het een goede ervaring was?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat zou je willen verbeteren aan een van je hobby's?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een hobby gehad die je nu niet meer beoefent? Wat is er veranderd?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit geprobeerd om je hobby om te zetten in een carrière?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest avontuurlijke dat je ooit hebt gedaan?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete ding om te doen met vrienden in je vrije tijd?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een hobby ontdekt waar je totaal geen ervaring mee had? Hoe ging dat?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Welke hobby zou je willen delen met anderen? Waarom?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is iets waar je gepassioneerd over bent, maar waarvan anderen misschien niet begrijpen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste les die je hebt geleerd van je hobby?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Als je één hobby zou moeten kiezen voor de rest van je leven, welke zou dat zijn?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een hobby uitgeoefend die je niet verwachtte te bevallen, maar die je toch leuk vond?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je opeens alle tijd van de wereld had om je hobby’s te volgen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is een hobby die je graag zou willen delen met je kinderen of kleinkinderen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Als je een hobby zou moeten inruilen voor een andere, welke zou je kiezen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat maakt een hobby voor jou echt de moeite waard?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest verrassende dat je hebt geleerd van je hobby?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },

                // Challenging intensity
                new Question { Description = "Wat is het moeilijkste dat je ooit hebt gedaan voor een hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een hobby opgegeven omdat het te moeilijk was? Waarom?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je volledig zou falen in je hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste persoonlijke uitdaging die je hebt overwonnen door middel van je hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een grote tegenslag gehad tijdens je hobby? Hoe ben je ermee omgegaan?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste mislukking die je hebt ervaren in je hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Ben je ooit zo gefrustreerd geweest over een hobby dat je bijna wilde stoppen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je zeggen tegen iemand die je hobby niet begrijpt of niet waardeert?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest ambitieuze doel dat je hebt gesteld voor je hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat heb je geleerd van je grootste uitdaging in je hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Als je je hobby moest aanpassen om meer succes te behalen, wat zou je dan veranderen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Hoe zou je reageren als je moet concurreren met iemand die veel beter is in je hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is je grootste onzekerheid als het gaat om je hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Als je alleen maar een hobby mocht kiezen, zou je dan voor iets avontuurlijks of iets rustig kiezen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je geen tijd meer had voor je hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest complexe aspect van je hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit de moed verloren om een hobby voort te zetten? Hoe vond je weer motivatie?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het moeilijkste dat je hebt bereikt in je hobby?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },

                // Daring intensity
                new Question { Description = "Wat is het riskantste dat je hebt gedaan in je hobby?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Ben je ooit gewond geraakt door een hobby? Wat gebeurde er?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je hobby je leven in gevaar zou brengen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest gewaagde dat je hebt geprobeerd in je hobby?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste uitdaging die je ooit bent aangegaan in je hobby, ondanks dat het gevaarlijk leek?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is je grootste avontuur geweest buiten je comfortzone in je hobby?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Als je een extreme versie van je hobby zou kunnen ervaren, wat zou dat zijn?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest opwindende dat je hebt gedaan in je hobby?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iets in je hobby gedaan dat mensen echt als 'waanzinnig' beschouwden?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het gevaarlijkste dat je hebt gedaan in de naam van je hobby?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Als je nooit meer risico zou mogen nemen in je hobby, zou je dat kunnen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het meeste vertrouwen dat je hebt opgebouwd door een gewaagde hobby?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het spannendste avontuur dat je hebt ervaren door een hobby?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest gedurfde dat je hebt gedaan voor je hobby?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een gevaarlijke situatie ervaren door je hobby? Hoe heb je het opgelost?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },

                // Extreme intensity
                new Question { Description = "Wat is het meest gevaarlijke avontuur dat je hebt ondernomen in de naam van je hobby?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit je leven risicoloos gezet voor je hobby? Waarom?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het uiterste gegaan in je hobby, en wat zou je doen als het fout zou gaan?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iets gedaan voor je hobby dat je later als extreem gevaarlijk beschouwde?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Als je niet bang zou zijn, wat zou je dan voor je hobby doen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest ondenkbare dat je hebt gedaan voor je hobby?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je hobby je leven zou kosten?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste persoonlijke test die je hebt ondergaan door je hobby?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest extreme avontuur dat je ooit hebt beleefd?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je hobby je in een gevaarlijke situatie bracht?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } }
            );
        }

        // Persoonlijke geheimen
        private static void SeedSecretsQuestions(DatabaseContext db)
        {
            var theme = db.Themes.First(x => x.Id == 3);

            db.Questions.AddRange(
                // Easy Intensity
                new Question { Description = "Wat is jouw favoriete film die je altijd kijkt wanneer je je niet goed voelt?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest gênante dat je ooit hebt gedaan?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Welke gewoonte heb je die je liever niet zou willen delen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is de laatste keer dat je je echt gelukkig voelde?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wie is de eerste persoon waar je naartoe gaat voor advies?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iets gestolen? Zo ja, wat?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste geheim dat je voor jezelf bewaart?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je nooit zou willen dat anderen over jou weten?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is een ongemakkelijke situatie waar je ooit in terecht bent gekomen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest romantische dat je ooit hebt gedaan?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete manier om je stress te verlichten?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Als je één ding aan jezelf zou kunnen veranderen, wat zou dat dan zijn?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat was de grootste misstap die je ooit hebt gemaakt op je werk?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iets gedaan dat je ouders echt zou teleurstellen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je slechtste gewoonte die je nooit aan anderen zou willen toegeven?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is de meest gekke beslissing die je ooit hebt genomen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete geheime plek om naartoe te gaan?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je een dag lang iemand anders zou kunnen zijn?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest ongebruikelijke dat je ooit hebt verzameld?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je grootste angst?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },

                // Average Intensity
                new Question { Description = "Wat is een ervaring uit je verleden die je nog steeds achtervolgt?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iemand bedrogen? Waarom?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste geheim dat je hebt gehouden voor je beste vriend(in)?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je absoluut niet kunt vergeven, zelfs niet voor jezelf?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Is er iemand in je leven die je liever niet had ontmoet?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste leugen die je ooit hebt verteld?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest privé dat je ooit op social media hebt gedeeld?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je wist dat niemand het zou ontdekken?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat was de grootste persoonlijke fout die je ooit hebt gemaakt?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iets gedaan dat je later hebt geweten dat het schadelijk was voor anderen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je altijd hebt willen zeggen, maar nooit hebt durven zeggen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is een geheim dat je altijd hebt bewaakt uit angst voor de gevolgen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iets gedaan uit wraak? Wat was dat?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je niet wilt dat je partner ooit ontdekt over jou?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste misverstanden die mensen over jou hebben?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is je meest geheime wens die je nooit met iemand zou delen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je hebt gedaan om jezelf uit een benarde situatie te redden?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je zeker wist dat je geen consequenties zou ondervinden?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },

                // Challenging Intensity
                new Question { Description = "Wat is iets dat je niet durft te vertellen, zelfs niet aan je beste vriend?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iemand in je leven volledig vertrouwen verloren? Wat gebeurde er?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het moeilijkste wat je ooit hebt moeten vergeven?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is een beslissing die je hebt genomen die je nooit had moeten nemen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een vriend(in) verloren door een geheim dat je hebt bewaard?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je partner je een geheim vertelt waarvan je weet dat het een dealbreaker is?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste angst die je hebt voor je toekomst?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is een geheim waar je zelf moeite mee hebt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest vernederende wat je ooit hebt meegemaakt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iemand verraden voor eigen gewin?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je wist dat je nooit meer veroordeeld zou worden voor iets dat je hebt gedaan?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste les die je hebt geleerd van een persoonlijke fout?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je niemand ooit zou kunnen vergeven?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de ergste beslissing die je ooit hebt genomen in je persoonlijke relaties?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste geheim dat je hebt over je familie?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je hebt verborgen uit schaamte, maar dat je nu zou willen delen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je absoluut niet kunt vergeten uit je verleden?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het moeilijkste dat je ooit hebt moeten loslaten?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },

                // Daring Intensity
                new Question { Description = "Wat is het gevaarlijkste dat je ooit hebt gedaan?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste risico dat je ooit hebt genomen in je leven?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je hebt gedaan waar je je diep voor schaamt?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iets gedaan uit pure rebellie? Wat was het?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je onzichtbaar zou kunnen zijn voor een dag?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste geheim dat je hebt over je liefdesleven?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is een geheim dat je zou delen, maar alleen als niemand zou weten dat het van jou komt?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest verbazingwekkende wat je ooit hebt meegemaakt?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit iets gedaan uit woede waar je spijt van hebt?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste avontuur dat je ooit hebt ondernomen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest gedurfde dat je ooit hebt gezegd?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is een geheim dat je nooit zou delen, zelfs niet met de persoon die het het meest verdient?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste avontuur dat je ooit hebt meegemaakt?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste geheim dat je hebt over je vrienden?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },

                // Extreme Intensity
                new Question { Description = "Wat is het meest extreme wat je ooit hebt gedaan?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste geheim dat je ooit hebt bewaard, zelfs voor jezelf?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het ergste wat je ooit hebt gedaan zonder het terug te nemen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je de rest van je leven nooit meer ergens veilig zou kunnen zijn?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je zou doen als je absoluut geen angst had voor de gevolgen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is je grootste onvervulde verlangen dat je nooit zou kunnen toelaten om uit te komen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste geheim dat je hebt dat je je ouders zou schokken?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste geheim dat je hebt over je carrièresucces?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is iets dat je hebt verborgen voor alle mensen in je leven?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest intense geheim dat je hebt over je gezondheid?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } }
            );
        }

        // Reizen & Avontuur
        private static void SeedAdventureQuestions(DatabaseContext db)
        {
            var theme = db.Themes.First(x => x.Id == 2);

            db.Questions.AddRange(
                // Easy intensity questions
                new Question { Description = "Wat is je favoriete vakantiebestemming en waarom?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je ideale manier om te reizen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Welke stad zou je graag nog eens willen bezoeken?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat was je leukste reiservaring tot nu toe?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Ben je liever in een hotel of kampeer je liever?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is het eerste wat je doet wanneer je op vakantie bent?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is de beste souvenir die je ooit hebt meegenomen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Welke cultuur wil je graag beter leren kennen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Heb je een favoriete reisblogger of influencer?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Hoe plan je normaal je reizen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete tijd van het jaar om te reizen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is een bestemming die je absoluut nog wilt bezoeken?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is het gekste dat je ooit hebt meegemaakt tijdens een reis?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Welke sport of activiteit zou je willen proberen op vakantie?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Welke plek heeft je het meeste verrast?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat neem je altijd mee als je op reis gaat?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Reis je liever met vrienden of solo?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is je favoriete manier om een nieuw land te verkennen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste fout die je ooit hebt gemaakt tijdens een reis?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },
                new Question { Description = "Als je de wereld zou kunnen rondreizen, welk vervoermiddel zou je dan kiezen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new[] { theme } },

                // Average intensity questions
                new Question { Description = "Wat is een land dat je nooit zou willen bezoeken, en waarom?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest avontuurlijke dat je ooit hebt gedaan tijdens een reis?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit in een ander land gewoond? Wat was dat voor ervaring?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Welke lokale gerechten heb je geproefd die je niet snel zult vergeten?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste risico dat je ooit hebt genomen tijdens een reis?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Hoe ga je om met cultuurshock wanneer je naar een nieuw land reist?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest unieke hotel waar je ooit hebt geslapen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Welke ervaring zou je aanraden aan reizigers die van avontuur houden?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is de gekste reisplanning die je ooit hebt gehad?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je plotseling strandt op een onbekend eiland?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit angst gehad tijdens een avontuurlijke activiteit?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is de langste reis die je ooit hebt gemaakt?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is een land waarvan je de geschiedenis altijd al hebt willen leren?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Hoe belangrijk is duurzaamheid voor jou tijdens het reizen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is je mening over backpacken versus luxe reizen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een onbekende taal geleerd tijdens je reizen?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het leukste dat je hebt gedaan op een onverwachte bestemming?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is een avontuur dat je nog wilt beleven, maar waarvoor je niet genoeg moed hebt?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },
                new Question { Description = "Wat is het mooiste natuurlijke wonder dat je hebt gezien?", Intensity = Enums.QuestionIntensity.Average, Themes = new[] { theme } },

                // Challenging intensity questions
                new Question { Description = "Wat is het grootste gevaar dat je ooit hebt ervaren tijdens een avontuur?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit in een land gereisd waar je geen enkele taal sprak? Hoe ging dat?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de meest fysieke uitdaging die je hebt overwonnen tijdens een reis?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je jezelf in een gevaarlijke situatie bevindt tijdens een reis?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is je grootste avontuur geweest dat je nog steeds aan anderen vertelt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit overwogen om een risico te nemen dat anderen niet zouden durven?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je geen manier had om terug naar huis te komen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat was de moeilijkste beslissing die je moest nemen tijdens een reis?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is de gevaarlijkste activiteit die je hebt geprobeerd?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een avontuur gehad dat je angst aanjoeg, maar waarvan je achteraf trots was?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest uitdagende land om te verkennen volgens jou?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Hoe heb je je voorbereid op een reis die veel fysieke inspanning vergde?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat was je grootste angst toen je solo op reis ging?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je in een vreemd land zonder geld vast kwam te zitten?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat was de grootste cultuurshock die je hebt ervaren tijdens een reis?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Heb je ooit een avontuur in de natuur meegemaakt waar je je leven op het spel zette?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest gevaarlijke wat je hebt gedaan voor avontuur?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Welke reiservaring zou je als levensveranderend beschrijven?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je midden in de nacht verdwaalt in een onbekende stad?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new[] { theme } },

                // Daring intensity questions
                new Question { Description = "Wat zou je doen als je het risico had om een natuurgeweld te overleven?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je zonder voorbereiding een berg beklimt?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je midden in de oceaan schipbreuk lijdt?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Hoe zou je reageren als je in een vreemd land zonder geld zou vast komen te zitten?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest gevaarlijke avontuur dat je zou willen proberen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je gedwongen werd om een zeldzaam avontuur alleen te ondernemen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest ondenkbare avontuur waar je jezelf in zou kunnen bevinden?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Hoe zou je jezelf voorbereiden op een avontuur dat je fysieke grenzen test?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Zou je een avontuur aangaan waarbij je de hulp van anderen niet kunt vragen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je voor een onmogelijke keuze staat tijdens een avontuur?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het grootste avontuur dat je zou doen als je geen angst had?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest risicovolle dat je zou doen als je weet dat het je leven zou veranderen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Welke uitdaging zou je aangaan om je angsten te overwinnen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je gedwongen werd om een onbekend terrein te verkennen zonder voorbereiding?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je in de natuur zou overleven zonder enige technologie?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je vast zou zitten in een onbekend land zonder enig begrip van de cultuur?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je tijdens een avontuur in de regenwouden wordt geconfronteerd met wilde dieren?", Intensity = Enums.QuestionIntensity.Daring, Themes = new[] { theme } },

                // Extreme intensity questions
                new Question { Description = "Wat zou je doen als je alleen in een onbekend land terechtkomt zonder enige middelen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het gevaarlijkste avontuur dat je zou aangaan om jezelf te testen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je tijdens een avontuur plotseling zwaargewond raakt?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Zou je een avontuur aangaan dat je leven zou kunnen kosten?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je in de diepste oceaan terechtkomt zonder hulpmiddelen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je voor een levensbedreigende situatie komt te staan zonder enige hulp?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je geen andere optie hebt dan een doodsangst avontuur aan te gaan?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Zou je een avontuur aangaan dat je psychisch of fysiek zou kunnen breken?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je in een noodsituatie terechtkomt zonder middelen om te overleven?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je wordt geconfronteerd met de ultieme uitdaging, zonder enige garanties?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is de grootste fysieke of mentale uitdaging die je ooit hebt overwonnen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je op een onbewoond eiland terechtkomt zonder enig hulpmiddel?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat zou je doen als je midden in een storm op zee vast komt te zitten?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Zou je een avontuur aangaan waarbij je moet overleven zonder enige hulp?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } },
                new Question { Description = "Wat is het meest extreme dat je ooit hebt gedaan voor avontuur?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new[] { theme } }
            );
        }

        private static void SeedRomanceQuestions(DatabaseContext db)
        {
            var theme = db.Themes.First(x => x.Id == 1);
            db.Questions.AddRange(
                // Easy Intensity
                new Question { Description = "Waar was jouw eerste date?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is jouw favoriete romantische film?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is voor jou een perfecte avond uit?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou jij je ideale partner omschrijven?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat vind je het leukste om samen te doen met je partner?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is jouw favoriete romantische locatie?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is het leukste dat je ooit voor je partner hebt gedaan?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou jij doen als je partner een heel verrassend cadeau voor je heeft?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is jouw idee van een perfecte date?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat was het leukste moment tijdens je laatste vakantie samen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is jouw favoriete manier om samen tijd door te brengen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat vind jij belangrijk in een vriendschap?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is een leuke verrassing die je ooit voor je partner hebt geregeld?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Welke romantische plek zou je graag samen willen bezoeken?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is het eerste dat je opvalt aan iemand?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou jij doen als je een boek over jullie liefde zou schrijven?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is je favoriete manier om je liefde te tonen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is voor jou een romantische verrassing?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is je favoriete herinnering van je laatste vakantie?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is jouw favoriete manier om je partner te verrassen?", Intensity = Enums.QuestionIntensity.Easy, Themes = new List<Theme> { theme } },

                // Medium Intensity
                new Question { Description = "Wat was het mooiste cadeau dat je ooit voor iemand hebt gekocht?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat was je meest romantische ervaring?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is jouw idee van een perfecte vakantie met je partner?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat vind jij belangrijk in een relatie?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner je niet zou steunen in een belangrijk project?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je reageren als je partner je zou teleurstellen?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou jij doen als je partner iets heel onverwachts voor je doet?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner je niet goed begrijpt?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je je partner niet meer vertrouwt?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat betekent liefde voor jou in één zin?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je jouw relatie willen zien in vijf jaar?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe belangrijk is communicatie voor jou in een relatie?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is voor jou het beste kenmerk van een gezonde relatie?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat was een moeilijk moment in jouw relatie en hoe ben je ermee omgegaan?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat denk je dat een langetermijnrelatie sterk maakt?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je je partner vergeven als ze iets heel erg verkeerds doen?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner in een andere stad zou moeten werken?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is de belangrijkste les die je hebt geleerd over liefde?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je omgaan met jaloezie in een relatie?", Intensity = Enums.QuestionIntensity.Average, Themes = new List<Theme> { theme } },

                // Challenging Intensity
                new Question { Description = "Zou je ooit een langeafstandrelatie aangaan? Waarom wel of niet?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner een geheime romantische relatie had?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner je niet zou steunen in je carrière?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat betekent het voor jou om 'voor altijd' samen te zijn?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner een andere levensstijl had dan jij?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je je partner kunnen vergeven voor iets dat je diep gekwetst heeft?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je reageren als je partner je niet zou begrijpen tijdens een moeilijke tijd?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner te veel vrijheid neemt in de relatie?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe belangrijk is onafhankelijkheid in een relatie voor jou?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner zijn/haar tijd niet goed zou verdelen tussen jou en vrienden?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je je partner steunen als ze een grote verandering in hun leven doormaakt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je je partner niet meer aantrekkelijk vindt?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat is jouw definitie van trouw in een relatie?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je in een open relatie willen zijn? Waarom wel of niet?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat betekent het voor jou om je partner volledig te vertrouwen?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner te veel zou eisen van je tijd?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner een geheim met je had?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je omgaan met onenigheid over belangrijke levenskeuzes?", Intensity = Enums.QuestionIntensity.Challenging, Themes = new List<Theme> { theme } },

                // Daring Intensity
                new Question { Description = "Wat is je grootste romantische avontuur dat je ooit zou willen beleven?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner jou ten huwelijk zou vragen op de meest onverwachte manier?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner ineens een open relatie zou voorstellen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner een extreme verandering in hun uiterlijk zou ondergaan?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je ooit een verre reis maken voor je partner zonder dat het praktisch zou zijn?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner een gigantisch risico zou nemen voor jullie toekomst?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je jezelf kunnen voorstellen in een situatie van jaloezie en wat zou je doen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner een grote geheimen onthult?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner je zou vragen om je levensstijl radicaal te veranderen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner een extreme carrièrekeuze maakt die jullie relatie zou kunnen beïnvloeden?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je reageren als je partner een zware en levensveranderende beslissing moet maken?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner zou willen verhuizen naar een ander land?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je een partner willen die heel veel risico neemt in het leven?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner je op een avontuur vraagt dat je nooit zou verwachten?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner voor een maand verdwijnt en je geen contact kunt opnemen?", Intensity = Enums.QuestionIntensity.Daring, Themes = new List<Theme> { theme } },

                // Extreme Intensity
                new Question { Description = "Wat is jouw grootste angstdroom als het gaat om relaties?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je reageren als je partner ineens met een ander persoon zou willen trouwen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner een geheim had dat hun gevoelens voor jou totaal zou veranderen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner zou aangeven dat ze je niet meer willen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je ontdekt dat je partner je bedroog voor jaren?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je je partner kunnen vergeven voor een grote geheimen die ze jarenlang verborgen hielden?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Hoe zou je omgaan met een partner die je vertrouwen volledig schendt?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner je zou willen verlaten voor iemand anders?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner je volledig onterecht beschuldigt?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner je zou vragen om iets onethisch of immoreel te doen?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner plotseling een radicale verandering in levenskeuzes zou maken?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Zou je kunnen omgaan met een situatie waarin je partner meer aandacht zou besteden aan hun carrière dan aan jullie relatie?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner alles zou verliezen en je zou moeten kiezen om samen verder te gaan?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } },
                new Question { Description = "Wat zou je doen als je partner plotseling een heel ander leven wil leiden?", Intensity = Enums.QuestionIntensity.Extreme, Themes = new List<Theme> { theme } }
            );
        }

        private static void SeedThemes(DatabaseContext db)
        {
            db.Themes.AddRange(
                new Theme
                {
                    Id = 1,
                    Name = "Romantiek & relaties"
                },
                new Theme
                {
                    Id = 2,
                    Name = "Reizen & avontuur"
                },
                new Theme
                {
                    Id = 3,
                    Name = "Persoonlijke geheimen"
                },
                new Theme
                {
                    Id = 4,
                    Name = "Hobby's & interesses"
                },
                new Theme
                {
                    Id = 5,
                    Name = "Familie & vriendschap"
                },
                new Theme
                {
                    Id = 6,
                    Name = "Levenskeuzes & beslissingen"
                },
                new Theme
                {
                    Id = 7,
                    Name = "Eten & drank"
                }
            );
        }
    }
}
