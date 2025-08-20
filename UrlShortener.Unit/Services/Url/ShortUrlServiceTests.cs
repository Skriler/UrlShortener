using Moq;
using UrlShortener.Data.Repositories;
using UrlShortener.Exceptions;
using UrlShortener.Models.DTOs.Url;
using UrlShortener.Models.Entities;
using UrlShortener.Services.Url.Core;
using UrlShortener.Services.Url.Generators;

namespace UrlShortener.Unit.Services.Url;

[Trait("Category", "Services")]
[Trait("Component", "Url")]
[Trait("SubComponent", "ShortUrlService")]
public class ShortUrlServiceTests
{
    private readonly Mock<IShortUrlRepository> shortUrlRepositoryMock;
    private readonly Mock<IShortCodeGenerator> shortCodeGeneratorMock;
    private readonly ShortUrlService service;

    public ShortUrlServiceTests()
    {
        shortUrlRepositoryMock = new Mock<IShortUrlRepository>();
        shortCodeGeneratorMock = new Mock<IShortCodeGenerator>();
        service = new ShortUrlService(shortUrlRepositoryMock.Object, shortCodeGeneratorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ReturnsShortUrlDto()
    {
        // Arrange
        var dto = new CreateUrlDto { OriginalUrl = "https://example.com" };
        const string userId = "user123";
        const string shortCode = "abc123";
        var shortUrl = new ShortUrl
        {
            Id = 1,
            OriginalUrl = dto.OriginalUrl,
            ShortCode = shortCode,
            CreatedBy = new ApplicationUser { UserName = "testuser" }
        };

        shortUrlRepositoryMock
            .Setup(x => x.OriginalUrlExistsAsync(dto.OriginalUrl))
            .ReturnsAsync(false);

        shortCodeGeneratorMock
            .Setup(x => x.GenerateAsync(dto.OriginalUrl))
            .ReturnsAsync(shortCode);

        shortUrlRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ShortUrl>()))
            .ReturnsAsync(shortUrl);

        // Act
        var result = await service.CreateAsync(dto, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(shortUrl.Id, result.Id);
        Assert.Equal(shortUrl.OriginalUrl, result.OriginalUrl);
        Assert.Equal(shortUrl.ShortCode, result.ShortCode);
    }

    [Fact]
    public async Task CreateAsync_WhenUrlExists_ThrowsUrlAlreadyExistsException()
    {
        // Arrange
        var dto = new CreateUrlDto { OriginalUrl = "https://example.com" };
        const string userId = "user123";

        shortUrlRepositoryMock
            .Setup(x => x.OriginalUrlExistsAsync(dto.OriginalUrl))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<UrlAlreadyExistsException>(() => service.CreateAsync(dto, userId));
    }

    [Fact]
    public async Task DeleteAsync_WhenUserNotOwnerAndNotAdmin_ThrowsUnauthorizedUrlAccessException()
    {
        // Arrange
        const long id = 1L;
        const string userId = "user123";
        var shortUrl = new ShortUrl
        {
            Id = id,
            CreatedById = "otheruser"
        };

        shortUrlRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(shortUrl);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedUrlAccessException>(() => service.DeleteAsync(id, userId, false));
    }

    [Fact]
    public async Task DeleteAsync_WhenUrlNotFound_ThrowsUrlNotFoundException()
    {
        // Arrange
        const long id = 1L;
        const string userId = "user123";

        shortUrlRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((ShortUrl)null!);

        // Act & Assert
        await Assert.ThrowsAsync<UrlNotFoundException>(() => service.DeleteAsync(id, userId, false));
    }
}
