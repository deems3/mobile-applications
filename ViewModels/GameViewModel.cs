using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TruthOrDrinkDemiBruls.Enums;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.ViewModels;

[QueryProperty(nameof(Game), "Game")]
[QueryProperty(nameof(QuestionAmount), "QuestionAmount")]
[QueryProperty(nameof(QuestionKind), "QuestionKind")]
[QueryProperty(nameof(QuestionIntensity), "QuestionIntensity")]
[QueryProperty(nameof(Themes), "Themes")]
public partial class GameViewModel : ObservableObject
{
    public Game Game { get; set; }
    public int QuestionAmount { get; set; }
    public QuestionKind QuestionKind { get; set; }
    public QuestionIntensity QuestionIntensity { get; set; }
    public List<Theme> Themes { get; set; } = [];
}
