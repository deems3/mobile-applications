using TruthOrDrinkDemiBruls.Responses;
using TruthOrDrinkDemiBruls.ViewModels;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(Image), "Image")]
public partial class WaitPage : ContentPage
{
    public string ImgUrl { get; set; }

    public WaitingPageViewModel ViewModel { get; set; }
    public GiphyImage Image
    {
        set
        {
            SetImageUrl(value);
        }
    }

	public WaitPage()
	{
        ViewModel = new();
		InitializeComponent();
        // Set the viewmodel to be the bindingcontext
        BindingContext = ViewModel;
	}

    private async void GoToNextQuestion(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//GameQuestions");
    }

    private async void GoToTheEnd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("TheEndPage");
    }

    private void SetImageUrl(GiphyImage image)
    {
        // Set the image url in the viewmodel
        ViewModel.ImageUrl = image.Url();
    }
}