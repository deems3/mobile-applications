using System.ComponentModel.DataAnnotations.Schema;

namespace TruthOrDrinkDemiBruls.Responses;

public class GiphyImage
{
    // All of the giphy image URLS start with the same address
    private const string BaseUrl = "https://i.giphy.com";

    // Id of the image
    public required string Id { get; init; }

    // Return the base url combined with the image id, so we can use this later on in the view without having to construct the URL multiple times
    public string Url()
    {
        return $"{BaseUrl}/{Id}.gif";
    }
}
public class GiphyRandomResponse
{
    // Data is the property in the response object from Giphy, this contains all image data
    public required GiphyImage Data { get; set; }
}
