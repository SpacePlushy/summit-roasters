using SummitRoasters.E2ETests.PageObjects;

namespace SummitRoasters.E2ETests;

public class ProductBrowsingTests
{
    private const string BaseUrl = "https://localhost:5001";

    // TODO: Set up Playwright browser in test fixture
    // private IPlaywright _playwright;
    // private IBrowser _browser;
    // private IPage _page;

    [Fact(Skip = "E2E tests require running application")]
    public async Task ProductListing_ShowsProducts()
    {
        // var page = ... // create page from browser
        // var listingPage = new ProductListingPage(page, BaseUrl);
        // await listingPage.NavigateAsync();
        // var count = await listingPage.ProductCards.CountAsync();
        // count.Should().BeGreaterThan(0);
        await Task.CompletedTask;
    }

    // TODO: CategoryFilter_FiltersProducts - selecting a category shows only matching products
    // TODO: SortBy_ReordersProducts - changing sort order reorders the product cards
    // TODO: Pagination_ShowsNextPage - clicking next page shows different products
    // TODO: ProductDetail_ShowsFullInfo - clicking a product card shows full product details
}
