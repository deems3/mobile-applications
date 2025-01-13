using TruthOrDrinkDemiBruls.Responses;
using TruthOrDrinkDemiBruls.Service;
using TruthOrDrinkDemiBruls.ViewModels;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(Image), "Image")]
public partial class WaitPage : ContentPage
{
    public string ImgUrl { get; set; }

    public WaitingPageViewModel ViewModel { get; set; }
    public GiphyImage Image
    {
        set
        {
            SetImageUrl(value);
        }
    }

    private GameService _gameService;

    public WaitPage(GameService gameService)
    {
        ViewModel = new();
        _gameService = gameService;
        InitializeComponent();
        // Set the viewmodel to be the bindingcontext
        BindingContext = ViewModel;
    }

    private async void GoToNextQuestion(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//GameQuestions",
            new Dictionary<string, object>
            {
                {"Player", _gameService.PlayerToAnswer!},
                {"Question", _gameService.QuestionToAnswer!.Question }
            }
            );
    }

    private async void GoToTheEnd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("TheEndPage");
    }

    private void SetImageUrl(GiphyImage image)
    {
        // Set the image url in the viewmodel
        ViewModel.ImageUrl = image.Url();
    }
}