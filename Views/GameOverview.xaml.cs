using System.ComponentModel;
using TruthOrDrinkDemiBruls.Enums;
using QuestionKindEnum = TruthOrDrinkDemiBruls.Enums.QuestionKind;
using QuestionIntensityEnum = TruthOrDrinkDemiBruls.Enums.QuestionIntensity;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.ViewModels;
using TruthOrDrinkDemiBruls.Service;
using Plugin.LocalNotification;

namespace TruthOrDrinkDemiBruls.Views;

public partial class GameOverview : ContentPage
{
    public string QuestionKindString => QuestionKind switch
    {
        QuestionKindEnum.Personalised => "Gepersonaliseerde vragen",
        QuestionKindEnum.Generated => "Gegenereerde vragen",
        QuestionKindEnum.Both => "Beide",
        _ => QuestionKind.ToString()
    };
    public QuestionKindEnum QuestionKind { get; set; }
    public string QuestionIntensityString => QuestionIntensity switch
    {
        QuestionIntensityEnum.Easy => "Gemakkelijk",
        QuestionIntensityEnum.Average => "Gemiddeld",
        QuestionIntensityEnum.Challenging => "Moeilijk",
        QuestionIntensityEnum.Daring => "Uitdagend",
        QuestionIntensityEnum.Extreme => "Extreem",
        _ => string.Empty
    };
    public QuestionIntensityEnum QuestionIntensity { get; set; }
    public int QuestionAmount { get; set; }
    public Game _Game { get; set; }
    public List<Theme> Themes { get; set; } = [];
    private GameViewModel ViewModel { get; set; }

    private GameService _gameService { get; }

    public GameOverview(GameViewModel viewModel, GameService gameService)
    {
        InitializeComponent();
        _gameService = gameService;
        QuestionAmount = _gameService.QuestionAmount;
        QuestionIntensity = _gameService.QuestionIntensity;
        QuestionKind = _gameService.QuestionKind;
        _Game = _gameService.Game!;
        Themes = _gameService.Themes;

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        // TODO: trigger changes
        base.OnAppearing();
        QuestionAmount = _gameService.QuestionAmount;
        QuestionIntensity = _gameService.QuestionIntensity;
        QuestionKind = _gameService.QuestionKind;
        _Game = _gameService.Game!;
        Themes = _gameService.Themes;
        BindingContext = this;
    }

    private async void StartGame(object sender, EventArgs e)
    {
        // TODO: make call to ChatGpt for the game questions, create some type of ViewModel that contains the question amount etc, so we'll only have to pass one object in the game loop.
        _gameService.StartGame();

        var request = new NotificationRequest
        {
            NotificationId = 1000,
            Title = "Have fun!",
            Subtitle = "Please enjoy the game!",
            Description = "",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now.AddSeconds(1)
            },
        };

        await LocalNotificationCenter.Current.Show(request);


        await Shell.Current.GoToAsync("//GameQuestions", new Dictionary<string, object>
        {
            { "Player", _gameService.PlayerToAnswer! },
            { "Question", _gameService.QuestionToAnswer!.Question }
        });

    }
}