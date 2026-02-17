using FluentAssertions;
using Moq;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Services;

namespace SummitRoasters.UnitTests.Services;

public class SearchServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly SearchService _searchService;

    public SearchServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _searchService = new SearchService(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Search_ReturnsResults()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Summit Blend" },
            new Product { Id = 2, Name = "Summit Dark" }
        };
        _productRepositoryMock
            .Setup(r => r.SearchAsync("Summit", 20))
            .ReturnsAsync(products);

        // Act
        var result = await _searchService.SearchAsync("Summit");

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Summit Blend");
    }

    [Fact]
    public async Task Search_ReturnsEmpty_WhenBlankQuery()
    {
        // Act
        var result = await _searchService.SearchAsync("   ");

        // Assert
        result.Should().BeEmpty();
        _productRepositoryMock.Verify(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Autocomplete_ReturnsResults()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Summit Blend" }
        };
        _productRepositoryMock
            .Setup(r => r.SearchAsync("Su", 5))
            .ReturnsAsync(products);

        // Act
        var result = await _searchService.AutocompleteAsync("Su");

        // Assert
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Summit Blend");
    }

    [Fact]
    public async Task Autocomplete_ReturnsEmpty_WhenQueryTooShort()
    {
        // Act
        var result = await _searchService.AutocompleteAsync("S");

        // Assert
        result.Should().BeEmpty();
        _productRepositoryMock.Verify(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }
}
