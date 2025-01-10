using QRCoder;
using SQLiteBrowser;
using TruthOrDrinkDemiBruls.Database;
namespace TruthOrDrinkDemiBruls.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
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

        // Action to open the database browser
        private async void OpenDatabaseBrowser(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DatabaseBrowserPage(Helper.GetDbPath("truth_or_drink_demi.db")));
        }

        private void ToggleGyroscope()
        {
            if (Gyroscope.Default.IsSupported)
            {
                if (!Gyroscope.Default.IsMonitoring)
                {
                    // Turn on gyroscope
                    Gyroscope.Default.ReadingChanged += Gyroscope_ReadingChanged;
                    Gyroscope.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    // Turn off gyroscope
                    Gyroscope.Default.Stop();
                    Gyroscope.Default.ReadingChanged -= Gyroscope_ReadingChanged;
                }
            }
        }

        private void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            // Update UI Label with gyroscope state
            GyroscopeLabel.TextColor = Colors.Green;
            GyroscopeLabel.Text = $"Gyroscope: {e.Reading}";
        }

        private void ToggleMagnetometer(object sender, EventArgs e)
        {
            ToggleGyroscope();
        }
    }
}
