using FluentAssertions;
using Microsoft.Playwright;
using SummitRoasters.E2ETests.Fixtures;
using SummitRoasters.E2ETests.PageObjects;

namespace SummitRoasters.E2ETests;

[Collection("Browser")]
public class SmokeTests
{
    private readonly BrowserFixture _fixture;

    public SmokeTests(BrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Homepage_Loads_Successfully()
    {
        var page = await _fixture.NewPageAsync();
        var homePage = new HomePage(page, _fixture.BaseUrl);
        await homePage.NavigateAsync();

        await Assertions.Expect(page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("Summit Roasters"));
        await Assertions.Expect(homePage.HeroSection).ToBeVisibleAsync();
        await Assertions.Expect(homePage.HeroHeading).ToContainTextAsync("Summit Roasters");
    }

    [Fact]
    public async Task NavigationLinks_LeadToCorrectPages()
    {
        var page = await _fixture.NewPageAsync();
        var homePage = new HomePage(page, _fixture.BaseUrl);
        await homePage.NavigateAsync();

        // Click Shop nav link
        await page.Locator("[data-testid='header-nav-shop']").ClickAsync();
        await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/products"));
        await Assertions.Expect(page.Locator("[data-testid='products-heading']")).ToBeVisibleAsync();

        // Click About nav link
        await page.Locator("[data-testid='header-nav-about']").ClickAsync();
        await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/Home/About"));
        await Assertions.Expect(page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("About"));
    }

    [Fact]
    public async Task Footer_IsVisibleOnAllPages()
    {
        var page = await _fixture.NewPageAsync();
        var homePage = new HomePage(page, _fixture.BaseUrl);

        // Check footer on homepage
        await homePage.NavigateAsync();
        await Assertions.Expect(homePage.Footer).ToBeVisibleAsync();

        // Check footer on products page
        await page.GotoAsync($"{_fixture.BaseUrl}/products");
        await Assertions.Expect(page.Locator("[data-testid='footer']")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task MobileMenu_TogglesOnSmallScreens()
    {
        var page = await _fixture.NewPageAsync();
        // Set mobile viewport
        await page.SetViewportSizeAsync(375, 667);

        var homePage = new HomePage(page, _fixture.BaseUrl);
        await homePage.NavigateAsync();

        // Mobile menu toggle should be visible
        await Assertions.Expect(homePage.MobileMenuToggle).ToBeVisibleAsync();

        // Open mobile menu
        await homePage.MobileMenuToggle.ClickAsync();
        await Assertions.Expect(homePage.MobileMenu).ToBeVisibleAsync();

        // Close mobile menu
        await homePage.MobileMenuClose.ClickAsync();
        await page.WaitForTimeoutAsync(500);
    }
}
