using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace SpotifyRandomApp.services;

public interface IPlaylistService
{
    Task<FullPlaylist?> GetPlaylist(ISpotifyClient spotifyClient, string playlistId);
    Task ReplaceTracks(ISpotifyClient spotifyClient, string playlistId, IEnumerable<string> randomTrackIds);
}

public class PlaylistService : IPlaylistService
{
    private readonly ILogger<PlaylistService> _logger;

    public PlaylistService(ILoggerFactory loggerFactory){
        _logger = loggerFactory.CreateLogger<PlaylistService>();
    }

    public async Task<FullPlaylist?> GetPlaylist(ISpotifyClient spotifyClient, string playlistId)
    {
        try
        {
            return await spotifyClient.Playlists.Get(playlistId).ConfigureAwait(false);    
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting playlist");
            throw;
        }
    }

    public async Task ReplaceTracks(ISpotifyClient spotifyClient, string playlistId, IEnumerable<string> randomTrackIds)
    {
        try
        {
            var replaceItemsRequest = new PlaylistReplaceItemsRequest(randomTrackIds.ToList());
            await spotifyClient.Playlists.ReplaceItems(playlistId, replaceItemsRequest).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error replacing tracks");
            throw;
        }
    }
}