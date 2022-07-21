using Microsoft.Extensions.Logging;
using Moq;
using SpotifyAPI.Web;
using SpotifyRandomApp.services;

namespace SpotifyRandomApp.Tests;

public class RandomPlaylistCreatorTests
{
    [Fact]
    public async Task CreateRandomPlaylist__DoesNotThrowException()
    {
        // Arrange
        ILoggerFactory doesntDoMuch = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory();
        var playlistServiceMock = new Mock<IPlaylistService>();
        var authServiceMock = new Mock<IAuthService>();
        // var mockSpotifyClient = new Mock<SpotifyClient>();
        // authServiceMock.Setup(x => x.GetAuthenticatedClient()).ReturnsAsync(mockSpotifyClient.Object);
        var searchServiceMock = new Mock<ISearchService>();
        var config = SpotifyClientConfig.CreateDefault();
        var randomPlaylistCreator = new RandomPlaylistCreator(doesntDoMuch,config,authServiceMock.Object,searchServiceMock.Object,playlistServiceMock.Object);
        //Act
        await Assert.ThrowsAsync<ArgumentNullException>(() => randomPlaylistCreator.CreateRandomPlaylist(null));
    }
}