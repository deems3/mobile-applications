using TruthOrDrinkDemiBruls.Views;

namespace TruthOrDrinkDemiBruls
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Login", typeof(Login));
            Routing.RegisterRoute("Home", typeof(MainPage));
            Routing.RegisterRoute("Lobby", typeof(Lobby));
            Routing.RegisterRoute("Oefenen", typeof(Oefenen));
            Routing.RegisterRoute("GameRules", typeof(GameRules));
            Routing.RegisterRoute("CustomQuestions", typeof(CustomQuestions));
            Routing.RegisterRoute("GameOptions", typeof(GameOptions));
            Routing.RegisterRoute("Themes", typeof(Themes));
            Routing.RegisterRoute("Questions", typeof(Questions));
            Routing.RegisterRoute("GameOverview", typeof(GameOverview));
            Routing.RegisterRoute("Intensity", typeof(Intensity));
            Routing.RegisterRoute("GameQuestions", typeof(GameQuestions));
            Routing.RegisterRoute("WaitPage", typeof(WaitPage));
            Routing.RegisterRoute("TheEndPage", typeof(TheEndPage));
        }
    }
}
