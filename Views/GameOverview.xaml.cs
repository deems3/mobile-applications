namespace TruthOrDrinkDemiBruls.Views;

public partial class GameOverview : ContentPage
{
	public GameOverview()
	{
		InitializeComponent();
	}

    private async void StartGame(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("//GameQuestions");
    }
}