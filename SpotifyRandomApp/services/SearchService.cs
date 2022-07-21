using System.Text.Json;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace SpotifyRandomApp.services;

public interface ISearchService
{
    Task<IEnumerable<string>> GetRandomTracks(ISpotifyClient spotifyClient, IEnumerable<string> existingTrackUris);
}

public class SearchService : ISearchService
{
    private readonly ILogger<SearchService> _logger;
    private const int MaxTracks = 25;
    private const int TimeToSleepBetweenSearchesMs = 1_000;

    public SearchService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SearchService>();
    }

    internal async Task<List<FullTrack>> RandomSearch(ISpotifyClient spotifyClient, string query)
    {
        if (spotifyClient is null)
        {
            throw new ArgumentNullException(nameof(spotifyClient));
        }
        var searchRequest = new SearchRequest(SearchRequest.Types.Track, query);
        var result = await spotifyClient.Search.Item(searchRequest);
        return result.Tracks.Items ?? new List<FullTrack>();
    }

    public async Task<IEnumerable<string>> GetRandomTracks(ISpotifyClient spotifyClient, IEnumerable<string> existingTrackUris)
    {
        var results = new List<string>();
        do
        {
            var randomQuery = GenerateRandomSearchQuery();
            _logger.LogDebug($"Random search query: {randomQuery}");
            var randomTracks = await RandomSearch(spotifyClient, randomQuery);
            var randomTrack = randomTracks
                .OrderByDescending(track => track.Popularity)
                .FirstOrDefault(track => !existingTrackUris.Contains(track.Uri) && !results.Contains(track.Uri));
            if(randomTrack is not null)
            {
                _logger.LogDebug($"Found track {randomTrack.Name} Uri {randomTrack.Uri}");
                results.Add(randomTrack.Uri);
            }else
            {
                _logger.LogDebug($"No track found for query {randomQuery}");
            }
            Thread.Sleep(TimeToSleepBetweenSearchesMs);
        } while (results.Count < MaxTracks);

        return results;
    }

    internal string GenerateRandomSearchQuery()
    {
        var randomYear = GetRandomYear();
        var randomGenre = GetRandomGenre();
        return $"year:{randomYear} genre:{randomGenre}";
    }
    
    internal int GetRandomYear() => new Random().Next(1920, DateTime.Now.Year);

    internal string GetRandomGenre() => SpotifyGenres.Genres[new Random().Next(SpotifyGenres.Genres.Count)];
    

}