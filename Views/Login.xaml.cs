namespace TruthOrDrinkDemiBruls.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

	private async void LoginUser(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync("//MainPage");
    }


}