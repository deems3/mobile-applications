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
        // Get an image from Giphy
        var img = await giphy.Random("telling truth");

        // Provide the image as a QueryProperty to the WaitPage view
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