using Microsoft.Extensions.Logging;

namespace SpotifyRandomApp.services;

public interface IPlaylistService
{
}

public class PlaylistService : IPlaylistService
{
    private readonly ILogger<PlaylistService> _logger;

    public PlaylistService(ILoggerFactory loggerFactory){
        _logger = loggerFactory.CreateLogger<PlaylistService>();
    }
}