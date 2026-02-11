namespace SummitRoasters.E2ETests.PageObjects;

using Microsoft.Playwright;

public class CartPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public CartPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    public async Task NavigateAsync() => await _page.GotoAsync($"{_baseUrl}/cart");
    public ILocator CartItems => _page.Locator("[data-testid='cart-item']");
    public ILocator CartSubtotal => _page.Locator("[data-testid='cart-subtotal']");
    public ILocator CheckoutButton => _page.Locator("[data-testid='checkout-button']");
    public ILocator EmptyCartMessage => _page.Locator("[data-testid='empty-cart']");
    public ILocator QuantityInputs => _page.Locator("[data-testid='cart-item-quantity']");
    public ILocator RemoveButtons => _page.Locator("[data-testid='cart-item-remove']");
}
