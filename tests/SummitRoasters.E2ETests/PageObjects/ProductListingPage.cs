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

    public IPage Page => _page;

    public async Task NavigateAsync(string? category = null)
    {
        var url = $"{_baseUrl}/products";
        if (category != null) url += $"?category={category}";
        await _page.GotoAsync(url);
    }

    public ILocator ProductCards => _page.Locator("[data-testid='products-grid'] > [data-testid^='product-card-']");
    public ILocator ProductsGrid => _page.Locator("[data-testid='products-grid']");
    public ILocator FilterSidebar => _page.Locator("[data-testid='filter-sidebar']");
    public ILocator SortSelect => _page.Locator("[data-testid='products-filter-sidebar'] [data-testid='filter-sort-select']");
    public ILocator FilterApply => _page.Locator("[data-testid='products-filter-sidebar'] [data-testid='filter-apply']");
    public ILocator Pagination => _page.Locator("[data-testid='pagination']");
    public ILocator PaginationNext => _page.Locator("[data-testid='pagination-next']");
    public ILocator ProductCount => _page.Locator("[data-testid='products-count']");
    public ILocator ProductsHeading => _page.Locator("[data-testid='products-heading']");
}
