using System.Collections.ObjectModel;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.ViewModels;
public class GameOptionsViewModel
{
    public Game Game { get; set; }
    public ObservableCollection<Theme> SelectedThemes { get; set; } = [];
}
