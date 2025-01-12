using System.ComponentModel;
using TruthOrDrinkDemiBruls.Enums;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.Service;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(QuestionAmount), "QuestionAmount")]
[QueryProperty(nameof(_QuestionKind), "QuestionKind")]
[QueryProperty(nameof(Themes), "Themes")]
[QueryProperty(nameof(_Game), "Game")]
[QueryProperty(nameof(QuestionIntensity), "QuestionIntensity")]
public partial class Questions : ContentPage, INotifyPropertyChanged
{
    private int questionAmount = 4;
    public int QuestionAmount
    {
        get => questionAmount;
        set
        {
            if (questionAmount != value)
            {
                questionAmount = value;
                OnPropertyChanged(nameof(QuestionAmount));
            }
        }
    }

    public List<Theme> Themes { get; set; }
    public Game _Game { get; set; }
    public QuestionIntensity QuestionIntensity { get; set; }

    public List<string> QuestionKinds { get; } = new List<string>
    {
        "Gepersonaliseerde vragen",
        "Gegenereerde vragen",
        "Beide"
    };

    private string questionKind = string.Empty;
    public string _QuestionKind
    {
        get => questionKind;
        set
        {
            if (questionKind != value)
            {
                questionKind = value;
                OnPropertyChanged(nameof(_QuestionKind));
            }
        }
    }

    private GameService _gameService { get; }

    public Questions(GameService gameService)
    {
        InitializeComponent();
        _gameService = gameService;
        BindingContext = this;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void QuestionAmountChanged(object sender, ValueChangedEventArgs e)
    {
        QuestionAmount = (int)e.NewValue;
        _gameService.QuestionAmount = QuestionAmount;
    }

    private void QuestionKindChanged(object sender, SelectionChangedEventArgs e)
    {
        _QuestionKind = (string)e.CurrentSelection[0];
        _gameService.QuestionKind = _QuestionKind switch
        {
            "Gepersonaliseerde vragen" => QuestionKind.Personalised,
            "Gegenereerde vragen" => QuestionKind.Generated,
            "Beide" => QuestionKind.Both,
            _ => QuestionKind.Unspecified
        };
    }

    private async void GoToGameOptions(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//GameOptions",
            new Dictionary<string, object>
            {
                { "Game", _Game },
                { "Themes", Themes },
                { "QuestionAmount", QuestionAmount },
                { "QuestionIntensity", QuestionIntensity },
                { "QuestionKind", _QuestionKind switch {
                        "Gepersonaliseerde vragen" => QuestionKind.Personalised,
                        "Gegenereerde vragen" => QuestionKind.Generated,
                        "Beide" => QuestionKind.Both,
                        _ => QuestionKind.Unspecified
                    }
                }
            }
        );
    }
}