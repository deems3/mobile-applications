using TruthOrDrinkDemiBruls.Enums;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.Service;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(QuestionAmount), "QuestionAmount")]
[QueryProperty(nameof(QuestionKind), "QuestionKind")]
[QueryProperty(nameof(Themes), "Themes")]
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

    private GameService _gameService { get; }

    public Intensity(GameService gameService)
    {
        InitializeComponent();
        _gameService = gameService;
        BindingContext = this;
    }

    private void QuestionIntensitChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedQuestionIntensity = (string)e.CurrentSelection[0];
        _gameService.QuestionIntensity = SelectedQuestionIntensity switch
        {
            "Gemakkelijk" => QuestionIntensity.Easy,
            "Gemiddeld" => QuestionIntensity.Average,
            "Moeilijk" => QuestionIntensity.Challenging,
            "Uitdagend" => QuestionIntensity.Daring,
            "Extreem" => QuestionIntensity.Extreme,
            _ => QuestionIntensity.Unspecified
        };
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
                        "Moeilijk" => QuestionIntensity.Challenging,
                        "Uitdagend" => QuestionIntensity.Daring,
                        "Extreem" => QuestionIntensity.Extreme,
                        _ => QuestionIntensity.Unspecified
                    }
                },
            }
        );
    }
}