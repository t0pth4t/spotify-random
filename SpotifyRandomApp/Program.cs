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
    var authService = new AuthService(config, clientId, clientSecret);
    var accessToken = await authService.GetToken().ConfigureAwait(false);
    var spotify = new SpotifyClient(config.WithToken(accessToken));
    var result = await spotify.Search.Item(new SearchRequest(SearchRequest.Types.Track, ""));
}
catch (Exception e)
{
    logger.LogCritical($"Failed to execute. Exception: {e.Message}");
}
