using CommunityToolkit.Mvvm.ComponentModel;

namespace TruthOrDrinkDemiBruls.ViewModels;

// In order to use ObservableProperty's we need to extends the ObservableObject class
public partial class WaitingPageViewModel : ObservableObject
{
    // The ObservableProperty is required in order for the view to know when a property has been updated
    [ObservableProperty]
    public string imageUrl;
}