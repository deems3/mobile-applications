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
[QueryProperty(nameof(Game), "Game")]
[QueryProperty(nameof(List<Theme>), "Themes")]
[QueryProperty(nameof(QuestionKindEnum), "QuestionKind")]
[QueryProperty(nameof(QuestionIntensityEnum), "QuestionIntensity")]
[QueryProperty(nameof(Int32), "QuestionAmount")]
public partial class GameOptions : ContentPage
{
    public Game _Game
    {
        set
        {
            SetGame(value);
        }
    }

    public ICollection<Theme> Themes
    {
        get => ViewModel.SelectedThemes;
        set
        {
            SetThemes(value);
        }
    }

    public QuestionKindEnum QuestionKind { 
        get => ViewModel.QuestionKind; 
        set
        {
            ViewModel.QuestionKind = value;
        }
    }
    public QuestionIntensityEnum QuestionIntensity { get; set; }
    public int QuestionAmount { get; set; }
    public Game Game { get; set; }

    public GameOptionsViewModel ViewModel { get; private set; }

    public GameOptions()
    {
        ViewModel = new GameOptionsViewModel();

        InitializeComponent();

        BindingContext = ViewModel;
    }

    private async void GoToThemes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//Themes",
            new Dictionary<string, object>
            {
                { "Game", ViewModel.Game },
                { "InitialThemes", ViewModel.SelectedThemes },
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
                { "Themes", ViewModel.SelectedThemes },
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
                { "InitialThemes", ViewModel.SelectedThemes },
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

    private void SetThemes(ICollection<Theme> themes)
    {
        // If no themes are provided when navigating to this view, set the selected themes to be an empty ObservableCollection
        if (themes.Count == 0)
        {
            ViewModel.SelectedThemes = new ObservableCollection<Theme>();
            return;
        }

        ViewModel.SelectedThemes = new ObservableCollection<Theme>(themes);
    }
}