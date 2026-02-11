namespace SummitRoasters.E2ETests.PageObjects;

using Microsoft.Playwright;

public class ProductDetailPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public ProductDetailPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    public async Task NavigateAsync(string slug) => await _page.GotoAsync($"{_baseUrl}/products/{slug}");
    public ILocator ProductName => _page.Locator("[data-testid='product-name']");
    public ILocator ProductPrice => _page.Locator("[data-testid='product-price']");
    public ILocator AddToCartButton => _page.Locator("[data-testid='add-to-cart-button']");
    public ILocator WeightSelector => _page.Locator("[data-testid='weight-selector']");
    public ILocator GrindSelector => _page.Locator("[data-testid='grind-selector']");
    public ILocator QuantityInput => _page.Locator("[data-testid='quantity-input']");
    public ILocator ReviewsSection => _page.Locator("[data-testid='reviews-section']");
    public ILocator ReviewCards => _page.Locator("[data-testid='review-card']");
}
