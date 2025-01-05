using System.Collections.ObjectModel;
using TruthOrDrinkDemiBruls.Client;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.ViewModels;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(Game), "Game")]
[QueryProperty(nameof(ICollection<Theme>), "Themes")]
public partial class GameOptions : ContentPage
{
    public Game Game
    {
        set
        {
            SetGame(value);
        }
    }

    public ICollection<Theme> Themes {
        set
        {
            SetThemes(value);
        }
    }

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
            "Themes",
            new Dictionary<string, object>
            {
                { "Game", ViewModel.Game },
                { "InitialThemes", ViewModel.SelectedThemes }
            }
        );
    }

    private async void GoToQuestions(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Questions");
    }

    private async void GoToGameOverview(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("GameOverview");
    }

    private async void GoToIntensity(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Intensity");
    }

    private void SetGame(Game game)
    {
        ViewModel.Game = game;
    }

    private void SetThemes(ICollection<Theme> themes)
    {
        if (themes.Count == 0)
        {
            ViewModel.SelectedThemes = [];
            return;
        }

        ViewModel.SelectedThemes = new ObservableCollection<Theme>(themes);
    }
}