using TruthOrDrinkDemiBruls.Database;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(Game), "Game")]
[QueryProperty(nameof(List<Theme>), "InitialThemes")]
public partial class Themes : ContentPage
{
	public Game Game { get; set; }

	public ICollection<Theme> InitialThemes { get; set; } = [];

	public ICollection<Theme> AvailableTheme { get; set; } = [];

	public Themes(DatabaseContext context)
	{
		AvailableTheme = context.Themes.Where(t => !InitialThemes.Contains(t)).ToList();
		InitializeComponent();
	}
}