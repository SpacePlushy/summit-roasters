namespace SummitRoasters.E2ETests.PageObjects;

using Microsoft.Playwright;

public class CheckoutPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public CheckoutPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    public async Task NavigateAsync() => await _page.GotoAsync($"{_baseUrl}/checkout");
    public ILocator ShippingForm => _page.Locator("[data-testid='shipping-form']");
    public ILocator OrderSummary => _page.Locator("[data-testid='order-summary']");
    public ILocator PlaceOrderButton => _page.Locator("[data-testid='place-order-button']");
    public ILocator DiscountCodeInput => _page.Locator("[data-testid='discount-code-input']");
    public ILocator ApplyDiscountButton => _page.Locator("[data-testid='apply-discount-button']");
}
