using System.Collections.ObjectModel;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.ViewModels;
public class GameOptionsViewModel
{
    public Game Game { get; set; }

    // ObservableCollection causes the view to be notified of updates to this collection
    public ObservableCollection<Theme> SelectedThemes { get; set; } = [];
}
