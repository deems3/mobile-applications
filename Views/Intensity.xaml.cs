using TruthOrDrinkDemiBruls.Enums;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(Int32), "QuestionAmount")]
[QueryProperty(nameof(QuestionKind), "QuestionKind")]
[QueryProperty(nameof(List<Theme>), "Themes")]
[QueryProperty(nameof(Game), "Game")]
[QueryProperty(nameof(SelectedQuestionIntensity), "SelectedQuestionIntensity")]
public partial class Intensity : ContentPage
{
	public int QuestionAmount { get; set; }
	public QuestionKind QuestionKind { get; set; }
	public List<Theme> Themes { get; set; }
	public Game Game { get; set; }

	public string SelectedQuestionIntensity { get; set; }
	public List<string> AvailableQuestionIntensities { get; } = [
        "Gemakkelijk",
        "Gemiddeld",
        "Moeilijk",
        "Uitdagend",
        "Extreem"
    ];

	public Intensity()
	{
		InitializeComponent();
		BindingContext = this;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine($"Look at the {Bla}!");
    }

    private void QuestionIntensitChanged(object sender, SelectionChangedEventArgs e)
    {
		SelectedQuestionIntensity = (string)e.CurrentSelection[0];
    }

    private async void GoToGameOptions(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//GameOptions",
            new Dictionary<string, object>
            {
                { "Game", Game },
                { "Themes", Themes },
                { "QuestionAmount", QuestionAmount },
                { "QuestionKind", QuestionKind },
                { "QuestionIntensity", SelectedQuestionIntensity switch {
                        "Gemakkelijk" => QuestionIntensity.Easy,
                        "Gemiddeld" => QuestionIntensity.Average,
                        "Moeijlijk" => QuestionIntensity.Challenging,
                        "Uitdagend" => QuestionIntensity.Daring,
                        "Extreem" => QuestionIntensity.Extreme,
                        _ => QuestionIntensity.Unspecified
                    }
                },
            }
        );
    }
}