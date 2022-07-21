using Microsoft.Extensions.Logging;
using SpotifyRandomApp.services;

namespace SpotifyRandomApp.Tests;

public class SearchServiceTests
{
    [Fact]
    public void GenerateRandomSearchQuery__ReturnsNonEmptyString()
    {
        // Arrange
        ILoggerFactory doesntDoMuch = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory();

        var searchService = new SearchService(doesntDoMuch);

        // Act
        var result = searchService.GenerateRandomSearchQuery();

        // Assert
        Assert.NotEmpty(result);
    }
}