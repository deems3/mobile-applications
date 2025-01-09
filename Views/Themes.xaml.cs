using System.Collections.ObjectModel;
using TruthOrDrinkDemiBruls.Database;
using TruthOrDrinkDemiBruls.Enums;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(Game), "Game")]
[QueryProperty(nameof(List<Theme>), "InitialThemes")]
[QueryProperty(nameof(QuestionKind), "QuestionKind")]
[QueryProperty(nameof(QuestionIntensity), "QuestionIntensity")]
[QueryProperty(nameof(Int32), "QuestionAmount")]
public partial class Themes : ContentPage
{
    public Game Game { get; set; }

    public ICollection<Theme> InitialThemes { get; set; } = [];

    public ICollection<Theme> AvailableThemes { get; set; } = [];

    public List<Theme> SelectedThemes { get; set; }
    public QuestionKind QuestionKind { get; set; }
    public int QuestionAmount { get; set; }
    public QuestionIntensity QuestionIntensity { get; set; }

    public Themes(DatabaseContext context)
    {
        // Get all themes from the databse that has not already been selected
        AvailableThemes = context.Themes.ToList();
        InitializeComponent();

        BindingContext = this;
    }

    private void ThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedThemes = e.CurrentSelection as object as List<Theme>;
    }

    private async void GoToGameOptions(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//GameOptions",
            new Dictionary<string, object>
            {
                { "Game", Game },
                { "Themes", SelectedThemes },
                { "QuestionAmount", QuestionAmount },
                { "QuestionKind", QuestionKind },
                { "QuestionIntensity", QuestionIntensity }
            }
        );
    }
}