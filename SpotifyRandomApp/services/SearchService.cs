using System.Text.Json;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace SpotifyRandomApp.services;

public interface ISearchService
{
    Task<List<FullTrack>> RandomSearch(ISpotifyClient spotifyClient);
    Task<IEnumerable<string>> GetRandomTracks(IEnumerable<string> existingTrackIds);
}

public class SearchService : ISearchService
{
    private readonly ILogger<SearchService> _logger;
    public SearchService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SearchService>();
    }

    public async Task<List<FullTrack>> RandomSearch(ISpotifyClient spotifyClient)
    {
        if (spotifyClient is null)
        {
            throw new ArgumentNullException(nameof(spotifyClient));
        }
        var searchRequest = new SearchRequest(SearchRequest.Types.Track, "artist:drake");
        var result = await spotifyClient.Search.Item(searchRequest);
        return result.Tracks.Items ?? new List<FullTrack>();
    }

    public Task<IEnumerable<string>> GetRandomTracks(IEnumerable<string> existingTrackIds)
    {
        throw new NotImplementedException();
    }

    internal string GenerateRandomSearchQuery()
    {
        return string.Empty;
    }
    
}