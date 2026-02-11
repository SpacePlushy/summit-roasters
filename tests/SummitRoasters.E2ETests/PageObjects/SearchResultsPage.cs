namespace SummitRoasters.E2ETests.PageObjects;

using Microsoft.Playwright;

public class SearchResultsPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public SearchResultsPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    public async Task NavigateAsync(string query) => await _page.GotoAsync($"{_baseUrl}/search?q={Uri.EscapeDataString(query)}");
    public ILocator SearchResults => _page.Locator("[data-testid='search-results']");
    public ILocator ProductCards => _page.Locator("[data-testid='product-card']");
    public ILocator NoResultsMessage => _page.Locator("[data-testid='no-results']");
    public ILocator SearchQuery => _page.Locator("[data-testid='search-query']");
}
