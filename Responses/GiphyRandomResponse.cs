using System.ComponentModel.DataAnnotations.Schema;

namespace TruthOrDrinkDemiBruls.Responses;

public class GiphyImage
{
    private const string BaseUrl = "https://i.giphy.com";
    public required string Id { get; init; }

    public string Url()
    {
        return $"{BaseUrl}/{Id}.gif";
    }
}
public class GiphyRandomResponse
{
    public required GiphyImage Data { get; set; }
}
