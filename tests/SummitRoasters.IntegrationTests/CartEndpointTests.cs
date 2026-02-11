using System.Net;
using FluentAssertions;

namespace SummitRoasters.IntegrationTests;

public class CartEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CartEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCart_ReturnsSuccessAndHtml()
    {
        // Act
        var response = await _client.GetAsync("/cart");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // TODO: AddToCart_ReturnsUpdatedCount
    // TODO: UpdateCartQuantity
    // TODO: RemoveFromCart
}
