using TruthOrDrinkDemiBruls.Client;

namespace TruthOrDrinkDemiBruls.Views;

public partial class GameQuestions : ContentPage
{
    private GiphyClient giphy;
    public GameQuestions(GiphyClient giphyClient)
    {
        giphy = giphyClient;
        InitializeComponent();
    }

    private async void GoToWaitPageTruth(object sender, EventArgs e)
    {
        var img = await giphy.Random("telling truth");
        await Shell.Current.GoToAsync("WaitPage", new Dictionary<string, object>
        {
            { "Image", img.Data }
        });
    }
    private async void GoToWaitPageDrink(object sender, EventArgs e)
    {
        var img = await giphy.Random("drinking");

        await Shell.Current.GoToAsync("WaitPage", new Dictionary<string, object>
        {
            { "Image", img.Data }
        });
    }
}