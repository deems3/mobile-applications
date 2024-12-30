using QRCoder;
using SQLiteBrowser;
using TruthOrDrinkDemiBruls.Database;
using TruthOrDrinkDemiBruls.Models;
namespace TruthOrDrinkDemiBruls.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void CreateGame(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Lobby");
        }

        private async void ReadRules(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("GameRules");
        }

        private async void GoToCustomQuestions(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("CustomQuestions");
        }

        private async void OpenDatabaseBrowser(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DatabaseBrowserPage(Helper.GetDbPath("truth_or_drink_demi.db")));
        }
    }
}
