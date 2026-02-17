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

    public IPage Page => _page;

    public async Task NavigateAsync(string query) => await _page.GotoAsync($"{_baseUrl}/search?q={Uri.EscapeDataString(query)}");
    public ILocator SearchResultsGrid => _page.Locator("[data-testid='search-results-grid']");
    public ILocator ProductCards => _page.Locator("[data-testid^='product-card-']");
    public ILocator NoResultsMessage => _page.Locator("[data-testid='empty-state']");
    public ILocator SearchQueryText => _page.Locator("[data-testid='search-results-query']");
    public ILocator SearchInput => _page.Locator("[data-testid='search-results-input']");
    public ILocator SearchHeading => _page.Locator("[data-testid='search-results-heading']");
}
