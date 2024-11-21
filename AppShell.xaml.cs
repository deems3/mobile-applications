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
            Routing.RegisterRoute("Oefenen", typeof(Oefenen));
        }
    }
}
