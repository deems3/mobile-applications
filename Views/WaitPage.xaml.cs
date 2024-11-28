namespace TruthOrDrinkDemiBruls.Views;

public partial class WaitPage : ContentPage
{
	public WaitPage()
	{
		InitializeComponent();
	}


    private async void GoToNextQuestion(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//GameQuestions");
    }

    private async void GoToTheEnd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("TheEndPage");
    }
}