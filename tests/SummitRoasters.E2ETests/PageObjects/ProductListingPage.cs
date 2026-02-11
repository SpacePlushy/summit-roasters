namespace SummitRoasters.E2ETests.PageObjects;

using Microsoft.Playwright;

public class ProductListingPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public ProductListingPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    public async Task NavigateAsync(string? category = null)
    {
        var url = $"{_baseUrl}/products";
        if (category != null) url += $"?category={category}";
        await _page.GotoAsync(url);
    }

    public ILocator ProductCards => _page.Locator("[data-testid='product-card']");
    public ILocator FilterSidebar => _page.Locator("[data-testid='filter-sidebar']");
    public ILocator SortSelect => _page.Locator("[data-testid='sort-select']");
    public ILocator Pagination => _page.Locator("[data-testid='pagination']");
    public ILocator ActiveFilters => _page.Locator("[data-testid='active-filters']");
    public ILocator ProductCount => _page.Locator("[data-testid='product-count']");
}
