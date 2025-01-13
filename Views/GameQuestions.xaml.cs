using TruthOrDrinkDemiBruls.Client;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.Service;
using TruthOrDrinkDemiBruls.ViewModels;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(Player), "Player")]
[QueryProperty(nameof(Question), "Question")]
public partial class GameQuestions : ContentPage
{
    private GiphyClient giphy;
    private GameService _gameService;
    public Player Player
    {
        set
        {
            ViewModel.Player = value;
        }
    }

    public Question Question
    {
        set
        {
            ViewModel.Question = value;
        }
    }

    public GameQuestionViewModel ViewModel;

    public GameQuestions(GiphyClient giphyClient, GameService gameService)
    {
        giphy = giphyClient;
        _gameService = gameService;
        ViewModel = new();
        InitializeComponent();

        BindingContext = ViewModel;
    }

    private async void GoToWaitPageTruth(object sender, EventArgs e)
    {
        AnswerQuestionAndNext();

        if (_gameService.QuestionToAnswer == null)
        {
            await Shell.Current.GoToAsync("TheEndPage");
            return;
        }
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
        AnswerQuestionAndNext();

        if (_gameService.QuestionToAnswer == null)
        {
            await Shell.Current.GoToAsync("TheEndPage");
            return;
        }

        var img = await giphy.Random("drinking");

        await Shell.Current.GoToAsync("WaitPage", new Dictionary<string, object>
        {
            { "Image", img.Data }
        });
    }

    private void AnswerQuestionAndNext()
    {
        _gameService.AnswerQuestion();
        _gameService.SetPlayerToAnswer();
        _gameService.SetNextQuestionToAnswer();
    }
}