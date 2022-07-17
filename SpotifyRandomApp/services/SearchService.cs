using System.Text.Json;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace SpotifyRandomApp.services;

public class SearchService
{
    private readonly ILogger<SearchService> _logger;
    private readonly SpotifyClientConfig _spotifyClientConfig;


    public SearchService(ILoggerFactory loggerFactory, SpotifyClientConfig config)
    {
        _logger = loggerFactory.CreateLogger<SearchService>();
        _spotifyClientConfig = config;
    }

    public async Task<IList<string>> RandomSearch(string accessToken)
    {
        var spotify = new SpotifyClient(_spotifyClientConfig.WithToken(accessToken));
        var searchRequest = new SearchRequest(SearchRequest.Types.Track, "artist:drake");
        var result = await spotify.Search.Item(searchRequest);
        var trackIds = new List<string>();
        foreach (var fullTrack in result?.Tracks.Items ?? Enumerable.Empty<FullTrack>())
        {
            _logger.LogInformation(JsonSerializer.Serialize(fullTrack, new JsonSerializerOptions
            {
                WriteIndented = true, MaxDepth = 10
            }));
            trackIds.Add(fullTrack.Id);
        }

        return trackIds;
    }

    internal string GenerateRandomSearchQuery()
    {
        return string.Empty;
    }
    
}