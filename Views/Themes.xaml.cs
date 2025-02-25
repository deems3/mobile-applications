using System.Collections.ObjectModel;
using TruthOrDrinkDemiBruls.Database;
using TruthOrDrinkDemiBruls.Enums;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.Service;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(Game), "Game")]
[QueryProperty(nameof(InitialThemes), "InitialThemes")]
[QueryProperty(nameof(QuestionKind), "QuestionKind")]
[QueryProperty(nameof(QuestionIntensity), "QuestionIntensity")]
[QueryProperty(nameof(QuestionAmount), "QuestionAmount")]
public partial class Themes : ContentPage
{
    public Game Game { get; set; }

    public List<Theme> InitialThemes { get; set; } = [];

    public List<Theme> AvailableThemes { get; set; } = [];

    public List<Theme> SelectedThemes { get; set; }
    public QuestionKind QuestionKind { get; set; }
    public int QuestionAmount { get; set; }
    public QuestionIntensity QuestionIntensity { get; set; }
    private GameService GameService { get; }

    public Themes(DatabaseContext context, GameService gameService)
    {
        // Get all themes from the databse that has not already been selected
        GameService = gameService;
        AvailableThemes = context.Themes.ToList();
        InitializeComponent();

        BindingContext = this;
    }

    private void ThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedThemes = e.CurrentSelection.Cast<Theme>().ToList();
        GameService.Themes = SelectedThemes;
    }
 private async void GoToGameOptions(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//GameOptions",
            new Dictionary<string, object>
            {
                { "Game", Game },
                { "Themes", SelectedThemes.ToList() },
                { "QuestionAmount", QuestionAmount },
                { "QuestionKind", QuestionKind },
                { "QuestionIntensity", QuestionIntensity }
            }
        );
    }
}