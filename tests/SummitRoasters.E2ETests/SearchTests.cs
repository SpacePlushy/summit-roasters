using FluentAssertions;
using Microsoft.Playwright;
using SummitRoasters.E2ETests.Fixtures;
using SummitRoasters.E2ETests.PageObjects;

namespace SummitRoasters.E2ETests;

[Collection("Browser")]
public class SearchTests
{
    private readonly BrowserFixture _fixture;

    public SearchTests(BrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Search_ReturnsMatchingProducts()
    {
        var page = await _fixture.NewPageAsync();
        var searchPage = new SearchResultsPage(page, _fixture.BaseUrl);

        // Search for "Ethiopian" - likely a common coffee term in seed data
        await searchPage.NavigateAsync("Ethiopian");

        await Assertions.Expect(searchPage.SearchHeading).ToBeVisibleAsync();
        await Assertions.Expect(searchPage.SearchQueryText).ToContainTextAsync("Ethiopian");

        var count = await searchPage.ProductCards.CountAsync();
        count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Search_NoResults_ShowsMessage()
    {
        var page = await _fixture.NewPageAsync();
        var searchPage = new SearchResultsPage(page, _fixture.BaseUrl);

        // Search for something that won't match any products
        await searchPage.NavigateAsync("xyznonexistentproduct123");

        await Assertions.Expect(searchPage.SearchHeading).ToBeVisibleAsync();
        await Assertions.Expect(searchPage.NoResultsMessage).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Search_ViaHeaderInput_NavigatesToResults()
    {
        var page = await _fixture.NewPageAsync();
        var homePage = new HomePage(page, _fixture.BaseUrl);
        await homePage.NavigateAsync();

        // The header search is inside a form with method="get" that submits to Search/Index
        // Fill the input and press Enter (submit the form)
        await homePage.HeaderSearchInput.ClickAsync();
        await homePage.HeaderSearchInput.FillAsync("coffee");

        // Use page.RunAndWaitForNavigationAsync to catch the navigation from form submit
        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await homePage.HeaderSearchInput.PressAsync("Enter");
        }, new PageRunAndWaitForNavigationOptions { Timeout = 10000 });

        // Should navigate to search results page
        page.Url.Should().Contain("/Search");
        await Assertions.Expect(page.Locator("[data-testid='search-results-heading']")).ToBeVisibleAsync();
    }
}
