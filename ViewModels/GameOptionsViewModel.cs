using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TruthOrDrinkDemiBruls.Enums;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.ViewModels;
public class GameOptionsViewModel : ObservableObject
{
    public Game Game { get; set; }

    // ObservableCollection causes the view to be notified of updates to this collection
    public ObservableCollection<Theme> SelectedThemes { get; set; } = [];

    public QuestionKind QuestionKind { get; set; }
    public QuestionIntensity QuestionIntensity { get; set; }
    public int QuestionAmount { get; set; }
}
