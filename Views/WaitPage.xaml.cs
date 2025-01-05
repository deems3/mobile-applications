using TruthOrDrinkDemiBruls.Responses;

namespace TruthOrDrinkDemiBruls.Views;

[QueryProperty(nameof(Image), "Image")]
public partial class WaitPage : ContentPage
{
    public string ImgUrl { get; private set; }
    public GiphyImage Image
    {
        set
        {
            SetImageUrl(value);
        }
    }

	public WaitPage()
	{
		InitializeComponent();
        BindingContext = this;
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
        ImgUrl = image.Url();
    }
}