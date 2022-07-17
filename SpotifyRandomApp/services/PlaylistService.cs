using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace SpotifyRandomApp.services;

public interface IPlaylistService
{
    Task<FullPlaylist> GetPlaylist(string playlistId);
    Task ReplaceTracks(string playlistId, IEnumerable<string> randomTrackIds);
}

public class PlaylistService : IPlaylistService
{
    private readonly ILogger<PlaylistService> _logger;

    public PlaylistService(ILoggerFactory loggerFactory){
        _logger = loggerFactory.CreateLogger<PlaylistService>();
    }

    public Task<FullPlaylist> GetPlaylist(string playlistId)
    {
        throw new NotImplementedException();
    }

    public Task ReplaceTracks(string playlistId, IEnumerable<string> randomTrackIds)
    {
        throw new NotImplementedException();
    }
}