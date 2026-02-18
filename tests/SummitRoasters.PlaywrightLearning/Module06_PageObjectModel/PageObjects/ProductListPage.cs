namespace SummitRoasters.PlaywrightLearning.Module06_PageObjectModel.PageObjects;

using Microsoft.Playwright;

/// <summary>
/// EXERCISE PAGE OBJECT: ProductListPage
///
/// This page object is PARTIALLY complete. You need to:
/// 1. Add the missing locators (marked with TODO)
/// 2. Add the missing action methods (marked with TODO)
/// 3. Then go to Exercise_BuildPageObject.cs and use this page object in tests
///
/// REFERENCE: Look at AboutPage.cs above for the pattern to follow.
/// You can also peek at the existing E2E page objects for inspiration:
///   tests/SummitRoasters.E2ETests/PageObjects/ProductListingPage.cs
/// </summary>
public class ProductListPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public ProductListPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    // Expose the underlying page
    public IPage Page => _page;

    // ============ NAVIGATION (provided) ============

    public async Task NavigateAsync(string? category = null)
    {
        var url = $"{_baseUrl}/products";
        if (category != null)
            url += $"?category={category}";
        await _page.GotoAsync(url, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
    }

    // ============ LOCATORS — some provided, some for you ============

    /// <summary>The page heading ("Our Coffee" or similar)</summary>
    public ILocator Heading => _page.GetByTestId("products-heading");

    /// <summary>The product count text ("X products found")</summary>
    public ILocator ProductCount => _page.GetByTestId("products-count");

    // TODO: Add a locator for the products grid
    // HINT: data-testid="products-grid"
    // Example: public ILocator ProductsGrid => _page.GetByTestId("...");

    // TODO: Add a locator for ALL product cards in the grid
    // HINT: Use CSS selector [data-testid^='product-card-']
    // Example: public ILocator ProductCards => _page.Locator("...");

    // TODO: Add a locator for the sort dropdown
    // HINT: It's inside the filter sidebar:
    //   _page.Locator("[data-testid='products-filter-sidebar'] [data-testid='filter-sort-select']")

    // TODO: Add a locator for the Apply Filters button
    // HINT: data-testid="filter-apply"

    // ============ ACTIONS — some provided, some for you ============

    /// <summary>Get the number of visible product cards</summary>
    public async Task<int> GetProductCardCountAsync()
    {
        // TODO: Implement this method
        // HINT: Use your ProductCards locator with .CountAsync()
        // Example: return await ProductCards.CountAsync();
        throw new NotImplementedException("Implement GetProductCardCountAsync");
    }

    /// <summary>Click the first product card to navigate to its detail page</summary>
    public async Task ClickFirstProductAsync()
    {
        // TODO: Implement this method
        // STEPS:
        // 1. Click the first product card: await ProductCards.First.ClickAsync();
        // 2. Wait for load: await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        throw new NotImplementedException("Implement ClickFirstProductAsync");
    }

    /// <summary>Sort products by the given sort value (e.g., "price-asc")</summary>
    public async Task SortByAsync(string sortValue)
    {
        // TODO: Implement this method
        // STEPS:
        // 1. Select the option on SortSelect: await SortSelect.SelectOptionAsync(sortValue);
        // 2. Click ApplyFilters: await ApplyFilters.ClickAsync();
        // 3. Wait for reload: await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        throw new NotImplementedException("Implement SortByAsync");
    }
}
