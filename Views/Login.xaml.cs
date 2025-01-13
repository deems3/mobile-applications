namespace TruthOrDrinkDemiBruls.Views;

public partial class Login : ContentPage
{
	public Login()
	{
        var hasLoggedInBefore = SecureStorage.GetAsync("truth_or_drink_authentication_code").GetAwaiter().GetResult();
        if (hasLoggedInBefore != null)
        {
            Shell.Current.GoToAsync("//MainPage").GetAwaiter().GetResult();
            return;
        }

        InitializeComponent();
	}

	private async void LoginUser(object sender, EventArgs e)
	{
		await SecureStorage.SetAsync("truth_or_drink_authentication_code", Guid.NewGuid().ToString());

		// the // betekent: er is geen backbutton en de hardware backbutton die gaat ook niet terug naar de vorige pagina.
		Vibration.Default.Vibrate(1000);
		await Shell.Current.GoToAsync("//MainPage");
    }
}