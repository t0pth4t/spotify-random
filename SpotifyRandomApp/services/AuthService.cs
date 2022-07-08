using SpotifyAPI.Web;

namespace SpotifyRandomApp.services;

public class AuthService
{
    private readonly SpotifyClientConfig _spotifyClientConfig;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public AuthService(SpotifyClientConfig spotifyClientConfig, string? clientId, string? clientSecret)
    {
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
            Console.WriteLine($"Failed to get access token. {e}");
            throw;
        }

    }
}