using SpotifyAPI.Web;
using SpotifyRandomApp.services;

var config = SpotifyClientConfig.CreateDefault();
var clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
var clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET");
var authService = new AuthService(config, clientId, clientSecret);
var accessToken = await authService.GetToken().ConfigureAwait(false);
var spotify = new SpotifyClient(config.WithToken(accessToken));
var result = await spotify.Search.Item(new SearchRequest(SearchRequest.Types.Track,""));