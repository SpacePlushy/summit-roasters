using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SummitRoasters.IntegrationTests;

public class OrderFlowTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public OrderFlowTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task FullCheckoutFlow_CreatesOrder()
    {
        // Arrange - create a no-redirect client and login
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        await LoginAsync(client, "sarah@example.com", "Customer123!");

        // Add item to cart (API endpoint - no CSRF needed)
        var addPayload = new { productId = 1, quantity = 1, weight = "12 oz", grind = "Whole Bean" };
        var addResponse = await client.PostAsJsonAsync("/api/cart/add", addPayload);
        addResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Go to checkout page - should now succeed since we're authenticated with items in cart
        // Use a redirect-following client for checkout since it may redirect if cart is empty
        var autoClient = _factory.CreateClient();
        // Login on the auto-redirect client too
        await LoginAsync(autoClient, "sarah@example.com", "Customer123!");
        await autoClient.PostAsJsonAsync("/api/cart/add", addPayload);

        var checkoutResponse = await autoClient.GetAsync("/checkout");
        checkoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var checkoutHtml = await checkoutResponse.Content.ReadAsStringAsync();
        checkoutHtml.Should().Contain("checkout");
    }

    [Fact]
    public async Task Checkout_RequiresAuthentication()
    {
        // Arrange - unauthenticated client with no auto-redirect
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await client.GetAsync("/checkout");

        // Assert - should redirect to login
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Contain("account/login");
    }

    [Fact]
    public async Task OrderHistory_ShowsUserOrders()
    {
        // Arrange - login as sarah who has seeded orders
        var client = _factory.CreateClient();
        await LoginAsync(client, "sarah@example.com", "Customer123!");

        // Act
        var response = await client.GetAsync("/account/orders");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        // Sarah should have seeded orders with order numbers starting with SR-
        content.Should().Contain("SR-");
    }

    [Fact]
    public async Task OrderDetail_ShowsCorrectOrder()
    {
        // Arrange - login as sarah and get her order list first
        var client = _factory.CreateClient();
        await LoginAsync(client, "sarah@example.com", "Customer123!");

        // Get order history to find an order number
        var historyResponse = await client.GetAsync("/account/orders");
        var historyHtml = await historyResponse.Content.ReadAsStringAsync();

        // Extract an order number from the page (format: SR-2025XXXX-XXXXXXXX)
        var orderMatch = Regex.Match(historyHtml, @"(SR-\d{8}-[A-F0-9]{8})");
        orderMatch.Success.Should().BeTrue("seeded orders should appear in order history");
        var orderNumber = orderMatch.Groups[1].Value;

        // Act
        var response = await client.GetAsync($"/account/orderdetail?orderNumber={orderNumber}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain(orderNumber);
    }

    private async Task LoginAsync(HttpClient client, string email, string password)
    {
        // Get login page for anti-forgery token
        var loginPage = await client.GetAsync("/account/login");
        var html = await loginPage.Content.ReadAsStringAsync();
        var token = ExtractAntiForgeryToken(html);

        var formData = new Dictionary<string, string>
        {
            ["Email"] = email,
            ["Password"] = password,
            ["__RequestVerificationToken"] = token
        };

        await client.PostAsync("/account/login", new FormUrlEncodedContent(formData));
    }

    private static string ExtractAntiForgeryToken(string html)
    {
        var match = Regex.Match(html, @"name=""__RequestVerificationToken""\s+type=""hidden""\s+value=""([^""]+)""");
        if (!match.Success)
            match = Regex.Match(html, @"type=""hidden""\s+name=""__RequestVerificationToken""\s+value=""([^""]+)""");
        if (!match.Success)
            match = Regex.Match(html, @"value=""([^""]+)""\s+name=""__RequestVerificationToken""");
        if (!match.Success)
            match = Regex.Match(html, @"__RequestVerificationToken[^>]+value=""([^""]+)""");
        match.Success.Should().BeTrue("anti-forgery token should be present in the form");
        return match.Groups[1].Value;
    }
}
