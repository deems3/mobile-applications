using System.ComponentModel;
using TruthOrDrinkDemiBruls.Enums;
using QuestionKindEnum = TruthOrDrinkDemiBruls.Enums.QuestionKind;
using QuestionIntensityEnum = TruthOrDrinkDemiBruls.Enums.QuestionIntensity;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.ViewModels;

namespace TruthOrDrinkDemiBruls.Views;

//[QueryProperty(nameof(_Game), "Game")]
//[QueryProperty(nameof(Themes), "Themes")]
//[QueryProperty(nameof(QuestionAmount), "QuestionAmount")]
//[QueryProperty(nameof(QuestionKind), "QuestionKind")]
//[QueryProperty(nameof(QuestionIntensity), "QuestionIntensity")]
public partial class GameOverview : ContentPage
{
    public string QuestionKindString => QuestionKind switch
    {
        QuestionKindEnum.Personalised => "Gepersonaliseerde vragen",
        QuestionKindEnum.Generated => "Gegenereerde vragen",
        QuestionKindEnum.Both => "Beide",
        _ => ""
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


    public GameOverview(GameViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("OVERVIEW ==========================");
        Console.WriteLine($"Look this is ViewModel.Game.Code: {_Game?.Code}");
        Console.WriteLine($"Look this is QuestionAmount: {QuestionAmount}");
        Console.WriteLine($"Look this is QuestionIntensity: {QuestionIntensity}");
        Console.WriteLine($"Look this is QuestionKind: {QuestionKind}");
        Console.WriteLine($"Look this is Themes.Count: {Themes.Count}");

        Console.WriteLine("==========================");
        Console.WriteLine($"Look this is ViewModel.Game.Code: {ViewModel.Game.Code}");
        Console.WriteLine($"Look this is QuestionAmount: {ViewModel.QuestionAmount}");
        Console.WriteLine($"Look this is QuestionIntensity: {ViewModel.QuestionIntensity}");
        Console.WriteLine($"Look this is QuestionKind: {ViewModel.QuestionKind}");
        Console.WriteLine($"Look this is Themes.Count: {ViewModel.Themes.Count()}");
        Console.WriteLine("==========================");
    }

    private async void StartGame(object sender, EventArgs e)
    {
        Console.WriteLine("OVERVIEW START GAME==========================");
        Console.WriteLine($"Look this is ViewModel.Game.Code: {_Game?.Code}");
        Console.WriteLine($"Look this is QuestionAmount: {QuestionAmount}");
        Console.WriteLine($"Look this is QuestionIntensity: {QuestionIntensity}");
        Console.WriteLine($"Look this is QuestionKind: {QuestionKind}");
        Console.WriteLine($"Look this is Themes.Count: {Themes.Count}");
        Console.WriteLine("==========================");
        // TODO: make call to ChatGpt for the game questions, create some type of ViewModel that contains the question amount etc, so we'll only have to pass one object in the game loop.
        return;
        await Shell.Current.GoToAsync("//GameQuestions", new Dictionary<string, object>
        {
            { "Game", _Game }
        });
    }
}