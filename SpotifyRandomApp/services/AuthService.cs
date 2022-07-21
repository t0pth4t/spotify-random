using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;

namespace SpotifyRandomApp.services;

public interface IAuthService
{
    Task<string> GetToken();
    Task<ISpotifyClient> GetAuthenticatedClient();
    Task GenerateCredentials();
}

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly SpotifyClientConfig _spotifyClientConfig;
    private readonly string _clientId;
    private readonly string _clientSecret;

    
    private const string CredentialsPath = "credentials.json";


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

    public async Task<ISpotifyClient> GetAuthenticatedClient()
    {
        if (!File.Exists(CredentialsPath))
        {
            throw new Exception("Client has not been authenticated. Run the application with 'GENERATE_CREDENTIALS' set to TRUE first to generate credentials.");
        }

        var json = await File.ReadAllTextAsync(CredentialsPath);
        var token = JsonConvert.DeserializeObject<PKCETokenResponse>(json);
        if(token is null)
        {
            throw new Exception("Failed to deserialize token");
        }
        var authenticator = new PKCEAuthenticator(_clientId, token);
        authenticator.TokenRefreshed += (sender, tokenResponse) => File.WriteAllText(CredentialsPath, JsonConvert.SerializeObject(tokenResponse));

        var config = SpotifyClientConfig.CreateDefault()
            .WithAuthenticator(authenticator);
        return new SpotifyClient(config);
    }

    public async Task GenerateCredentials()
    {
        var (verifier, challenge) = PKCEUtil.GenerateCodes();
        var server = new EmbedIOAuthServer(new Uri("http://localhost:5234/callback"), 5234);

        await server.Start();
        server.AuthorizationCodeReceived += async (sender, response) =>
        {
            await server.Stop();
            var token = await new OAuthClient().RequestToken(
                new PKCETokenRequest(_clientId, response.Code, server.BaseUri, verifier)
            );

            await File.WriteAllTextAsync(CredentialsPath, JsonConvert.SerializeObject(token));
        };

        var request = new LoginRequest(server.BaseUri, _clientId, LoginRequest.ResponseType.Code)
        {
            CodeChallenge = challenge,
            CodeChallengeMethod = "S256",
            Scope = new List<string> { "playlist-modify-public playlist-modify-private" }
        };

        var uri = request.ToUri();
        try
        {
            BrowserUtil.Open(uri);
        }
        catch (Exception)
        {
            Console.WriteLine("Unable to open URL, manually open: {0}", uri);
        }
        Console.WriteLine("After authenticating, press any key to continue...");
        _ = Console.ReadKey();
    }
}