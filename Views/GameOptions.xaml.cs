namespace TruthOrDrinkDemiBruls.Views;

public partial class GameOptions : ContentPage
{
	public GameOptions()
	{
		InitializeComponent();
	}

    private async void GoToThemes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Themes");
    }

    private async void GoToQuestions(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Questions");
    }

    private async void GoToGameOverview(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("GameOverview");
    }

    private async void GoToIntensity(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Intensity");
    }
}