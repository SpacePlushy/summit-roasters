using System.Net;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SummitRoasters.IntegrationTests;

public class AuthenticationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AuthenticationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task LoginPage_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/account/login");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ProtectedPage_RedirectsToLogin()
    {
        // Arrange
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await client.GetAsync("/checkout");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Contain("account/login");
    }

    [Fact]
    public async Task Register_CreatesNewUser()
    {
        // Arrange - use a no-redirect client so we can inspect the 302
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Get the registration page to extract anti-forgery token
        var getResponse = await client.GetAsync("/account/register");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var html = await getResponse.Content.ReadAsStringAsync();
        var token = ExtractAntiForgeryToken(html);

        // Act
        var formData = new Dictionary<string, string>
        {
            ["FirstName"] = "Test",
            ["LastName"] = "User",
            ["Email"] = $"testuser_{Guid.NewGuid():N}@example.com",
            ["Password"] = "TestPass123!",
            ["ConfirmPassword"] = "TestPass123!",
            ["__RequestVerificationToken"] = token
        };
        var content = new FormUrlEncodedContent(formData);

        var response = await client.PostAsync("/account/register", content);

        // Assert - successful registration redirects to home
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Be("/");
    }

    [Fact]
    public async Task Login_WithValidCredentials_Succeeds()
    {
        // Arrange - use a no-redirect client
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Get the login page to extract anti-forgery token
        var getResponse = await client.GetAsync("/account/login");
        var html = await getResponse.Content.ReadAsStringAsync();
        var token = ExtractAntiForgeryToken(html);

        // Act - login with seeded user
        var formData = new Dictionary<string, string>
        {
            ["Email"] = "sarah@example.com",
            ["Password"] = "Customer123!",
            ["__RequestVerificationToken"] = token
        };
        var content = new FormUrlEncodedContent(formData);

        var response = await client.PostAsync("/account/login", content);

        // Assert - successful login redirects to home
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Be("/");
    }

    [Fact]
    public async Task Logout_RedirectsToHome()
    {
        // Arrange - first login
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Login first
        var loginPage = await client.GetAsync("/account/login");
        var loginHtml = await loginPage.Content.ReadAsStringAsync();
        var loginToken = ExtractAntiForgeryToken(loginHtml);

        var loginForm = new Dictionary<string, string>
        {
            ["Email"] = "sarah@example.com",
            ["Password"] = "Customer123!",
            ["__RequestVerificationToken"] = loginToken
        };
        await client.PostAsync("/account/login", new FormUrlEncodedContent(loginForm));

        // Now get a page that has the logout anti-forgery token
        // After login, get the home page which should have a logout form
        var homePage = await client.GetAsync("/");
        var homeHtml = await homePage.Content.ReadAsStringAsync();
        var logoutToken = ExtractAntiForgeryToken(homeHtml);

        // Act - logout
        var logoutForm = new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = logoutToken
        };
        var response = await client.PostAsync("/account/logout", new FormUrlEncodedContent(logoutForm));

        // Assert - logout redirects to home
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Be("/");
    }

    private static string ExtractAntiForgeryToken(string html)
    {
        var match = Regex.Match(html, @"name=""__RequestVerificationToken""\s+type=""hidden""\s+value=""([^""]+)""");
        if (!match.Success)
        {
            // Try alternate attribute order
            match = Regex.Match(html, @"type=""hidden""\s+name=""__RequestVerificationToken""\s+value=""([^""]+)""");
        }
        if (!match.Success)
        {
            // Try with value first
            match = Regex.Match(html, @"value=""([^""]+)""\s+name=""__RequestVerificationToken""");
        }
        if (!match.Success)
        {
            // Most flexible: just find the token value near the name
            match = Regex.Match(html, @"__RequestVerificationToken[^>]+value=""([^""]+)""");
        }
        match.Success.Should().BeTrue("anti-forgery token should be present in the form");
        return match.Groups[1].Value;
    }
}
