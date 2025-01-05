using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using TruthOrDrinkDemiBruls.Config;
using TruthOrDrinkDemiBruls.Responses;

namespace TruthOrDrinkDemiBruls.Client;

public class GiphyClient(HttpClient httpClient, GiphyConfig giphyConfig)
{
    private const string BaseUrl = "https://api.giphy.com/v1/gifs";
    private const string SearchEndpoint = "random";

    /// <summary>
    /// Search for gifs on Giphy
    /// </summary>
    /// <param name="searchParamm"></param>
    /// <param name="rating">Can either be 'g', 'pg', 'pg-13' or 'r'</param>
    /// <param name="amount">Amount of gifs to return from the API</param>
    /// <returns></returns>
    public async Task<GiphyRandomResponse> Random(string searchParamm, string rating = "pg", int amount = 1)
    {
        var response = await httpClient.GetAsync($"{BaseUrl}/{SearchEndpoint}?tag={searchParamm}&rating={rating}&api_key={GetApiKey()}");

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonConvert.DeserializeObject<GiphyRandomResponse>(content);

        if (parsed is null)
        {
            throw new Exception($"Could not parse response {content}");
        }

        return parsed;
    }

    private string GetApiKey()
    {
        return giphyConfig.ApiKey;
    }
}
