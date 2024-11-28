namespace TruthOrDrinkDemiBruls.Views;

public partial class GameQuestions : ContentPage
{
	public GameQuestions()
	{
		InitializeComponent();
	}

    private async void GoToWaitPage(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("WaitPage");
    }
}