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
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
        _playlistService = playlistService ?? throw new ArgumentNullException(nameof(playlistService));
        _spotifyClientConfig = config ?? throw new ArgumentNullException(nameof(config));
    }


    public async Task CreateRandomPlaylist(string? playlistId)
    {
        if (string.IsNullOrWhiteSpace(playlistId))
        {
            throw new ArgumentNullException(nameof(playlistId));
        }
        var accessToken = await _authService.GetToken().ConfigureAwait(false);
        ISpotifyClient spotify = new SpotifyClient(_spotifyClientConfig.WithToken(accessToken));
        //get original playlist
        var originalPlaylist = await _playlistService.GetPlaylist(spotify,playlistId).ConfigureAwait(false);
        if (originalPlaylist is null)
        {
            _logger.LogError($"Playlist not found for playlist id: {playlistId}. Unable to proceed");
            return;
        }
        var tracks = GetTracksFromPlaylist(originalPlaylist);
        //kick off search for random tracks using playlist so no-dupes
        var randomTracks = await _searchService.GetRandomTracks(spotify, tracks.Select(x => x.Id)).ConfigureAwait(false);
        //replace random tracks to playlist
        await _playlistService.ReplaceTracks(spotify, playlistId, randomTracks).ConfigureAwait(false);
    }

    internal IEnumerable<FullTrack> GetTracksFromPlaylist(FullPlaylist playlist)
    {
        var tracks = new List<FullTrack>();

        foreach (var item in playlist?.Tracks?.Items ?? new List<PlaylistTrack<IPlayableItem>>())
        {
            if (item.Track is FullTrack track)
            {
                tracks.Add(track);
            }
        }

        return tracks;
    }
}