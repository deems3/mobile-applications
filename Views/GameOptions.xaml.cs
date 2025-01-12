using System.Collections.ObjectModel;
using TruthOrDrinkDemiBruls.Client;
using TruthOrDrinkDemiBruls.Enums;
using QuestionIntensityEnum = TruthOrDrinkDemiBruls.Enums.QuestionIntensity;
using QuestionKindEnum = TruthOrDrinkDemiBruls.Enums.QuestionKind;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.ViewModels;

namespace TruthOrDrinkDemiBruls.Views;

// Query properties are needed in order to pass parameters to this view when navigating
// First parameter is the type of the property, the second parameter is the name of the parameter when navigating (which is also the name of the property on the class) (see line 44 for an example)
[QueryProperty(nameof(_Game), "Game")]
[QueryProperty(nameof(Themes), "Themes")]
[QueryProperty(nameof(QuestionKind), "QuestionKind")]
[QueryProperty(nameof(QuestionIntensity), "QuestionIntensity")]
[QueryProperty(nameof(QuestionAmount), "QuestionAmount")]
public partial class GameOptions : ContentPage
{
    public Game _Game
    {
        set
        {
            SetGame(value);
        }
    }

    public List<Theme> Themes
    {
        get => ViewModel.SelectedThemes.ToList();
        set
        {
            ViewModel.SelectedThemes = new ObservableCollection<Theme>(value ?? []);
        }
    }

    public QuestionKindEnum QuestionKind
    {
        get => ViewModel.QuestionKind;
        set
        {
            ViewModel.QuestionKind = value;
        }
    }
    public QuestionIntensityEnum QuestionIntensity
    {
        get => ViewModel.QuestionIntensity;
        set
        {
            ViewModel.QuestionIntensity = value;
        }
    }
    public int QuestionAmount
    {
        get => ViewModel.QuestionAmount;
        set
        {
            ViewModel.QuestionAmount = value;
        }
    }

    public GameOptionsViewModel ViewModel { get; private set; }

    public GameOptions()
    {
        ViewModel = new GameOptionsViewModel();

        InitializeComponent();

        BindingContext = ViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("==========================");
        Console.WriteLine($"Look this is ViewModel.Game.Code: {ViewModel.Game?.Code}");
        Console.WriteLine($"Look this is QuestionAmount: {QuestionAmount}");
        Console.WriteLine($"Look this is QuestionIntensity: {QuestionIntensity}");
        Console.WriteLine($"Look this is QuestionKind: {QuestionKind}");
        Console.WriteLine($"Look this is Themes.Count: {ViewModel.SelectedThemes.Count}");
        Console.WriteLine("==========================");
    }

    private async void GoToThemes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//Themes",
            new Dictionary<string, object>
            {
                { "Game", ViewModel.Game },
                { "InitialThemes", ViewModel.SelectedThemes.ToList() },
                { "QuestionAmount", QuestionAmount },
                { "QuestionKind", QuestionKind },
                { "QuestionIntensity", QuestionIntensity }
            }
        );
    }

    private async void GoToQuestions(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//Questions",
            new Dictionary<string, object>
            {
                { "Game", ViewModel.Game },
                { "Themes", ViewModel.SelectedThemes.ToList() },
                { "QuestionAmount", QuestionAmount },
                { "QuestionIntensity", QuestionIntensity },
                { "QuestionKind", QuestionKind switch {
                    QuestionKindEnum.Personalised => "Gepersonaliseerde vragen",
                    QuestionKindEnum.Generated => "Gegenereerde vragen",
                    QuestionKindEnum.Both => "Beide",
                    _ => string.Empty
                    }
                }
            }
        );
    }

    private async void GoToGameOverview(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "GameOverview",
            new Dictionary<string, object>
            {
                { "Game", ViewModel.Game },
                { "Themes", ViewModel.SelectedThemes.ToList() },
                { "QuestionAmount", QuestionAmount },
                { "QuestionIntensity", QuestionIntensity },
                { "QuestionKind", QuestionKind }
            }
        );
    }

    private async void GoToIntensity(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//Intensity",
            new Dictionary<string, object>
            {
                { "Game", ViewModel.Game },
                { "InitialThemes", ViewModel.SelectedThemes.ToList() },
                { "QuestionAmount", QuestionAmount },
                { "QuestionKind", QuestionKind },
                {
                    "QuestionIntensity",
                    QuestionIntensity switch
                    {
                        QuestionIntensityEnum.Easy => "Gemakkelijk",
                        QuestionIntensityEnum.Average => "Gemiddeld",
                        QuestionIntensityEnum.Challenging => "Moeilijk",
                        QuestionIntensityEnum.Daring => "Uitdagend",
                        QuestionIntensityEnum.Extreme => "Extreem",
                        _ => string.Empty
                    }
                }
            }
        );
    }

    private void SetGame(Game game)
    {
        ViewModel.Game = game;
    }
}