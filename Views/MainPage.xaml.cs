using QRCoder;
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
    }
}
