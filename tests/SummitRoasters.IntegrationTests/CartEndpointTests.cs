using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace SummitRoasters.IntegrationTests;

public class CartEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CartEndpointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCart_ReturnsSuccessAndHtml()
    {
        // Arrange - use a fresh client for session isolation
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/cart");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddToCart_ReturnsUpdatedCount()
    {
        // Arrange - fresh client for session isolation
        var client = _factory.CreateClient();
        var payload = new { productId = 1, quantity = 2, weight = "12 oz", grind = "Whole Bean" };

        // Act
        var response = await client.PostAsJsonAsync("/api/cart/add", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<CartApiResponse>();
        body!.ItemCount.Should().Be(2);
        body.Subtotal.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateCartQuantity_ReturnsUpdatedCart()
    {
        // Arrange - fresh client, add an item first
        var client = _factory.CreateClient();
        var addPayload = new { productId = 1, quantity = 1, weight = "12 oz", grind = "Whole Bean" };
        var addResponse = await client.PostAsJsonAsync("/api/cart/add", addPayload);
        addResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act - update quantity to 3
        var updatePayload = new { productId = 1, quantity = 3 };
        var response = await client.PutAsJsonAsync("/api/cart/update", updatePayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<CartApiResponse>();
        body!.ItemCount.Should().Be(3);
    }

    [Fact]
    public async Task RemoveFromCart_ReturnsEmptyCart()
    {
        // Arrange - fresh client, add an item first
        var client = _factory.CreateClient();
        var addPayload = new { productId = 1, quantity = 1, weight = "12 oz", grind = "Whole Bean" };
        var addResponse = await client.PostAsJsonAsync("/api/cart/add", addPayload);
        addResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act - remove the item
        var response = await client.DeleteAsync("/api/cart/remove/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<CartApiResponse>();
        body!.ItemCount.Should().Be(0);
    }

    private record CartApiResponse(int ItemCount, decimal Subtotal);
}
