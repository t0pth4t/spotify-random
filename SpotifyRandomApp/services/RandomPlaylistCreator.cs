using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace SpotifyRandomApp.services;

public class RandomPlaylistCreator
{
    private readonly ILogger<RandomPlaylistCreator> _logger;
    private readonly SpotifyClientConfig _spotifyClientConfig;
    private readonly IAuthService _authService;
    private readonly ISearchService _searchService;
    private readonly IPlaylistService _playlistService;

    public RandomPlaylistCreator(ILoggerFactory loggerFactory, SpotifyClientConfig config, IAuthService authService, ISearchService searchService, IPlaylistService playlistService)
    {
        _logger = loggerFactory.CreateLogger<RandomPlaylistCreator>();
        _authService = authService;
        _searchService = searchService;
        _playlistService = playlistService;
        _spotifyClientConfig = config;
    }


    public async Task CreateRandomPlaylist()
    {
        var accessToken = await _authService.GetToken().ConfigureAwait(false);
        var spotify = new SpotifyClient(_spotifyClientConfig.WithToken(accessToken));
        

    }
}