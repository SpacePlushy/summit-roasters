using System.Net;
using FluentAssertions;

namespace SummitRoasters.IntegrationTests;

public class ProductEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_ReturnsSuccessAndHtml()
    {
        // Act
        var response = await _client.GetAsync("/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("products");
    }

    [Fact]
    public async Task GetProducts_WithCategoryFilter_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/products?category=SingleOrigin");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // TODO: GetProductDetail_ReturnsSuccess
    // TODO: GetProducts_WithSortParameter
}
