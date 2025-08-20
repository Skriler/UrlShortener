using Moq;
using UrlShortener.Data.Repositories;
using UrlShortener.Exceptions;
using UrlShortener.Services.Url.Generators;

namespace UrlShortener.Unit.Services.Url;

[Trait("Category", "Services")]
[Trait("Component", "Url")]
[Trait("SubComponent", "ShortCodeGenerator")]
public class ShortCodeGeneratorTests
{
    private const string ValidCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int ExpectedLength = 6;
    private const int MaxAttempts = 100;

    private readonly Mock<IShortUrlRepository> shortUrlRepositoryMock;
    private readonly ShortCodeGenerator generator;

    public ShortCodeGeneratorTests()
    {
        shortUrlRepositoryMock = new Mock<IShortUrlRepository>();
        generator = new ShortCodeGenerator(shortUrlRepositoryMock.Object);
    }

    [Theory]
    [InlineData("https://example.com")]
    [InlineData("https://google.com/search?q=test")]
    [InlineData("https://very-long-domain-name.com/very/long/path/to/resource")]
    public async Task GenerateAsync_WithDifferentUrls_AlwaysReturnsValidCode(string originalUrl)
    {
        // Arrange
        shortUrlRepositoryMock
            .Setup(r => r.ShortCodeExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await generator.GenerateAsync(originalUrl);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ExpectedLength, result.Length);
        Assert.True(IsValidCharacterSet(result), "Generated code contains invalid characters");
    }

    [Fact]
    public async Task GenerateAsync_WhenCodeExists_RetriesUntilUniqueFound()
    {
        // Arrange
        const string originalUrl = "https://example.com";
        var callCount = 0;

        shortUrlRepositoryMock
            .Setup(r => r.ShortCodeExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(() => ++callCount <= 2);

        // Act
        var result = await generator.GenerateAsync(originalUrl);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ExpectedLength, result.Length);
        Assert.True(IsValidCharacterSet(result), "Generated code contains invalid characters");
        shortUrlRepositoryMock.Verify(r => r.ShortCodeExistsAsync(It.IsAny<string>()), Times.Exactly(3));
    }

    [Fact]
    public async Task GenerateAsync_WhenMaxAttemptsReached_ThrowsShortCodeGenerationException()
    {
        // Arrange
        const string originalUrl = "https://example.com";

        shortUrlRepositoryMock
            .Setup(r => r.ShortCodeExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        var exception = await Assert.ThrowsAsync<ShortCodeGenerationException>(
            () => generator.GenerateAsync(originalUrl));

        // Assert
        Assert.Equal(originalUrl, exception.OriginalUrl);
        Assert.Equal(MaxAttempts, exception.Attempts);
        shortUrlRepositoryMock.Verify(r => r.ShortCodeExistsAsync(It.IsAny<string>()), Times.Exactly(MaxAttempts));
    }

    private static bool IsValidCharacterSet(string code)
    {
        return code.All(ValidCharacters.Contains);
    }
}
