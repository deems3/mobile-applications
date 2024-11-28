namespace TruthOrDrinkDemiBruls.Views;

public partial class TheEndPage : ContentPage
{
	public TheEndPage()
	{
		InitializeComponent();
	}

    private async void GoToMain(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Home");
    }
}