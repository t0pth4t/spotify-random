using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace SpotifyRandomApp.services;

public interface IAuthService
{
    Task<string> GetToken();
}

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly SpotifyClientConfig _spotifyClientConfig;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public AuthService(ILoggerFactory loggerFactory, SpotifyClientConfig spotifyClientConfig, string? clientId, string? clientSecret)
    {
        _logger = loggerFactory.CreateLogger<AuthService>();
        _spotifyClientConfig = spotifyClientConfig;
        _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));

    }

    public async Task<string> GetToken()
    {
        try
        {
            var request = new ClientCredentialsRequest(_clientId, _clientSecret);
            var response = await new OAuthClient(_spotifyClientConfig).RequestToken(request);
            return response.AccessToken;
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to get access token. {e}");
            throw;
        }

    }
}