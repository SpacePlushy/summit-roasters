namespace SummitRoasters.PlaywrightLearning.Module02_FindingElements;

using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;

/// <summary>
/// MODULE 2: FINDING ELEMENTS
/// ==========================
///
/// WHAT YOU'LL LEARN:
/// - GetByTestId()     — find elements by data-testid attribute (PREFERRED at work)
/// - Locator()         — find elements by CSS selector
/// - GetByRole()       — find elements by ARIA role (button, link, heading)
/// - GetByText()       — find elements by visible text content
/// - Chaining locators — page.Locator("parent").Locator("child")
///
/// LOCATOR PRIORITY (best to worst):
/// 1. data-testid  — most reliable, won't break when CSS/text changes
/// 2. Role-based   — accessible, semantic, resilient to visual changes
/// 3. Text-based   — simple but fragile (text changes = broken tests)
/// 4. CSS selectors — powerful but couples tests to implementation details
///
/// All interactive elements in Summit Roasters have data-testid attributes.
/// This is a best practice you'll see at your job too.
/// </summary>
[Collection("Learning")]
public class Lesson_LocatorStrategies
{
    private readonly LearningBrowserFixture _fixture;

    public Lesson_LocatorStrategies(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Example01_FindByTestId()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        // GetByTestId finds elements with data-testid="hero-heading"
        // This is the most reliable locator strategy
        var heroHeading = page.GetByTestId("hero-heading");

        await Assertions.Expect(heroHeading).ToBeVisibleAsync();
        await Assertions.Expect(heroHeading).ToContainTextAsync("Summit Roasters");
    }

    [Fact]
    public async Task Example02_FindByCssSelector()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        // Locator() accepts any CSS selector
        // Bracket notation finds elements where an attribute equals a value
        var footer = page.Locator("[data-testid='footer']");
        await Assertions.Expect(footer).ToBeVisibleAsync();

        // You can also use standard CSS selectors
        var allLinks = page.Locator("a[href]");
        var linkCount = await allLinks.CountAsync();
        Assert.True(linkCount > 5, $"Expected many links, found {linkCount}");
    }

    [Fact]
    public async Task Example03_FindByRole()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/account/login");

        // GetByRole finds elements by their ARIA role
        // A <button> element has role "button"
        // The Name option filters by the button's accessible name
        var loginButton = page.GetByRole(AriaRole.Button, new() { Name = "Sign In" });
        await Assertions.Expect(loginButton).ToBeVisibleAsync();

        // Find all heading elements on the page
        var headings = page.GetByRole(AriaRole.Heading);
        var headingCount = await headings.CountAsync();
        Assert.True(headingCount >= 1, "Login page should have at least one heading");
    }

    [Fact]
    public async Task Example04_ChainingLocators()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        // CHAINING: Find an element inside another element
        // First find the header, then find elements inside it
        var header = page.GetByTestId("header");
        var cartLink = header.GetByTestId("header-cart-link");
        await Assertions.Expect(cartLink).ToBeVisibleAsync();

        // Another example: find the search input inside the header
        var searchInput = header.GetByTestId("header-search-input");
        await Assertions.Expect(searchInput).ToBeVisibleAsync();
    }
}
