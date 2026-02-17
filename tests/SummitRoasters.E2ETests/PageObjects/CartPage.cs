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

    public IPage Page => _page;

    public async Task NavigateAsync() => await _page.GotoAsync($"{_baseUrl}/cart");
    public ILocator CartItems => _page.Locator("[data-testid^='cart-item-'][data-testid$='-remove']").Locator("..");
    public ILocator CartItemsList => _page.Locator("[data-testid='cart-items-list']");
    public ILocator CartSubtotal => _page.Locator("[data-testid='cart-subtotal']");
    public ILocator CartTotal => _page.Locator("[data-testid='cart-total']");
    public ILocator CheckoutButton => _page.Locator("[data-testid='cart-checkout-button']");
    public ILocator EmptyState => _page.Locator("[data-testid='empty-state']");
    public ILocator CartHeading => _page.Locator("[data-testid='cart-heading']");
    public ILocator CartItemCount => _page.Locator("[data-testid='cart-item-count']");
    public ILocator CartBadge => _page.Locator("[data-testid='header-cart-badge']");
}
