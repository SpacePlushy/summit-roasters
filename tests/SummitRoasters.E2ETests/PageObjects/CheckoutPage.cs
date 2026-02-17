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

    public IPage Page => _page;

    public async Task NavigateAsync() => await _page.GotoAsync($"{_baseUrl}/checkout");
    public ILocator CheckoutHeading => _page.Locator("[data-testid='checkout-heading']");
    public ILocator CheckoutForm => _page.Locator("[data-testid='checkout-form']");
    public ILocator ShippingSection => _page.Locator("[data-testid='checkout-shipping']");
    public ILocator ShippingName => _page.Locator("[data-testid='checkout-shipping-name']");
    public ILocator ShippingPhone => _page.Locator("[data-testid='checkout-shipping-phone']");
    public ILocator ShippingStreet => _page.Locator("[data-testid='checkout-shipping-street']");
    public ILocator ShippingCity => _page.Locator("[data-testid='checkout-shipping-city']");
    public ILocator ShippingState => _page.Locator("[data-testid='checkout-shipping-state']");
    public ILocator ShippingZip => _page.Locator("[data-testid='checkout-shipping-zip']");
    public ILocator ReviewItems => _page.Locator("[data-testid='checkout-review-items']");
    public ILocator DiscountInput => _page.Locator("[data-testid='checkout-discount-input']");
    public ILocator ApplyDiscountButton => _page.Locator("[data-testid='checkout-discount-apply']");
    public ILocator PlaceOrderButton => _page.Locator("[data-testid='checkout-place-order']");
    public ILocator OrderSummary => _page.Locator("[data-testid='checkout-summary']");
    public ILocator Subtotal => _page.Locator("[data-testid='checkout-subtotal']");
    public ILocator Total => _page.Locator("[data-testid='checkout-total']");
    public ILocator DiscountAmount => _page.Locator("[data-testid='checkout-discount-amount']");

    public async Task FillShippingAsync(string name, string phone, string street, string city, string state, string zip)
    {
        await ShippingName.FillAsync(name);
        await ShippingPhone.FillAsync(phone);
        await ShippingStreet.FillAsync(street);
        await ShippingCity.FillAsync(city);
        await ShippingState.FillAsync(state);
        await ShippingZip.FillAsync(zip);
    }
}
