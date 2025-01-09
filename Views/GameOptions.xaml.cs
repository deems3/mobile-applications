using System.Collections.ObjectModel;
using TruthOrDrinkDemiBruls.Client;
using TruthOrDrinkDemiBruls.Enums;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.ViewModels;

namespace TruthOrDrinkDemiBruls.Views;

// Query properties are needed in order to pass parameters to this view when navigating
// First parameter is the name of the property on this class, the second parameter is the name of the parameter when navigating (see line 44 for an example)
[QueryProperty(nameof(Game), "Game")]
[QueryProperty(nameof(List<Theme>), "Themes")]
[QueryProperty(nameof(QuestionKind), "QuestionKind")]
[QueryProperty(nameof(QuestionIntensity), "QuestionIntensity")]
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
        set
        {
            SetThemes(value);
        }
    }

    public QuestionKind QuestionKind { get; set; }
    public QuestionIntensity QuestionIntensity { get; set; }
    public int QuestionAmount { get; set; } = 4;
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
                    QuestionKind.Personalised => "Gepersonaliseerde vragen",
                    QuestionKind.Generated => "Gegenereerde vragen",
                    QuestionKind.Both => "Beide",
                        _ => string.Empty
                    }
                }
            }
        );
    }

    private async void GoToGameOverview(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("GameOverview");
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
                        QuestionIntensity.Easy => "Gemakkelijk",
                        QuestionIntensity.Average => "Gemiddeld",
                        QuestionIntensity.Challenging => "Moeilijk",
                        QuestionIntensity.Daring => "Uitdagend",
                        QuestionIntensity.Extreme => "Extreem",
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