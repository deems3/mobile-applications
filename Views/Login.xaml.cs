namespace TruthOrDrinkDemiBruls.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

	private async void LoginUser(object sender, EventArgs e)
	{
		// the // betekent: er is geen backbutton en de hardware backbutton die gaat ook niet terug naar de vorige pagina.
		Vibration.Default.Vibrate(200);
		await Shell.Current.GoToAsync("//MainPage");
    }


}