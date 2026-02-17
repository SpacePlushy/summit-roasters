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

    public IPage Page => _page;
    public async Task NavigateAsync(string slug) => await _page.GotoAsync($"{_baseUrl}/products/{slug}");
    public ILocator ProductName => _page.Locator("[data-testid='product-detail-name']");
    public ILocator ProductPrice => _page.Locator("[data-testid='product-detail-price']");
    public ILocator ProductDescription => _page.Locator("[data-testid='product-detail-description']");
    public ILocator ProductCategory => _page.Locator("[data-testid='product-detail-category']");
    public ILocator AddToCartButton => _page.Locator("[data-testid='add-to-cart-button']");
    public ILocator WeightOptions => _page.Locator("[data-testid='product-detail-weight-options']");
    public ILocator GrindSelect => _page.Locator("[data-testid='grind-select']");
    public ILocator QuantityInput => _page.Locator("[data-testid='quantity-input']");
    public ILocator ReviewsTab => _page.Locator("[data-testid='product-tab-reviews']");
    public ILocator ReviewsContent => _page.Locator("[data-testid='product-tab-content-reviews']");
    public ILocator ReviewForm => _page.Locator("[data-testid='review-form']");
    public ILocator ReviewFormRating => _page.Locator("[data-testid='review-form-rating']");
    public ILocator ReviewFormTitle => _page.Locator("[data-testid='review-form-title']");
    public ILocator ReviewFormBody => _page.Locator("[data-testid='review-form-body']");
    public ILocator ReviewFormSubmit => _page.Locator("[data-testid='review-form-submit']");
    public ILocator ReviewsList => _page.Locator("[data-testid='reviews-list']");
    public ILocator StockStatus => _page.Locator("[data-testid='product-detail-stock']");
}
