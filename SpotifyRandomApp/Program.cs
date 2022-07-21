using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using SpotifyRandomApp.services;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
        .AddConsole();
});
ILogger logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("Example log message");
try
{

    var config = SpotifyClientConfig.CreateDefault();
    var clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
    var clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET");
    var playlistId = Environment.GetEnvironmentVariable("PLAYLIST_ID");
    var shouldGenerateCredentials = Environment.GetEnvironmentVariable("GENERATE_CREDENTIALS");
    var authService = new AuthService(loggerFactory, config, clientId, clientSecret);
    if(shouldGenerateCredentials is not null && shouldGenerateCredentials.Equals("TRUE",StringComparison.InvariantCultureIgnoreCase))
    {
        await authService.GenerateCredentials().ConfigureAwait(false);
    }

    var searchService = new SearchService(loggerFactory);
    var playlistService = new PlaylistService(loggerFactory);
    var randomPlaylistCreator = new RandomPlaylistCreator(loggerFactory, config, authService, searchService,playlistService);

    await randomPlaylistCreator.CreateRandomPlaylist(playlistId);
}
catch (Exception e)
{
    logger.LogCritical($"Failed to execute. Exception: {e.Message}");
}
