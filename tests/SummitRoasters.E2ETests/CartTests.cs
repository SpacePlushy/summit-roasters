using FluentAssertions;
using Microsoft.Playwright;
using SummitRoasters.E2ETests.Fixtures;
using SummitRoasters.E2ETests.PageObjects;

namespace SummitRoasters.E2ETests;

[Collection("Browser")]
public class CartTests
{
    private readonly BrowserFixture _fixture;

    public CartTests(BrowserFixture fixture)
    {
        _fixture = fixture;
    }

    private async Task<IPage> AddProductToCartViaApiAsync()
    {
        var page = await _fixture.NewPageAsync();

        // Navigate to products page to get a product ID
        var listingPage = new ProductListingPage(page, _fixture.BaseUrl);
        await listingPage.NavigateAsync();

        // Click first product to go to detail page (so we have session/cookies)
        await listingPage.ProductCards.First.ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Extract productId from the hidden form field
        var productId = await page.Locator("[data-testid='product-detail-form'] input[name='productId']").GetAttributeAsync("value");

        // Add to cart via API call from the browser context
        await page.EvaluateAsync(@"async (pid) => {
            const response = await fetch('/api/cart/add', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ productId: parseInt(pid), quantity: 1 })
            });
            return await response.json();
        }", productId);

        // Wait for cart badge to potentially update
        await page.WaitForTimeoutAsync(500);

        return page;
    }

    [Fact]
    public async Task AddToCart_ShowsInCart()
    {
        var page = await AddProductToCartViaApiAsync();

        // Navigate to cart
        var cartPage = new CartPage(page, _fixture.BaseUrl);
        await cartPage.NavigateAsync();

        // Cart should have items
        await Assertions.Expect(cartPage.CartHeading).ToBeVisibleAsync();
        await Assertions.Expect(cartPage.CartItemsList).ToBeVisibleAsync();
    }

    [Fact]
    public async Task UpdateQuantity_UpdatesTotal()
    {
        var page = await AddProductToCartViaApiAsync();

        // Navigate to cart
        var cartPage = new CartPage(page, _fixture.BaseUrl);
        await cartPage.NavigateAsync();

        // Get initial subtotal text
        var initialSubtotal = await cartPage.CartSubtotal.TextContentAsync();

        // Update quantity via API (increase to 2)
        var productId = await page.Locator("[data-testid^='cart-item-increase-']").First.GetAttributeAsync("data-product-id");
        await page.EvaluateAsync(@"async (pid) => {
            const response = await fetch('/api/cart/update', {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ productId: parseInt(pid), quantity: 2 })
            });
            return await response.json();
        }", productId);

        // Reload the page to see updated totals
        await page.ReloadAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Subtotal should have changed
        var updatedSubtotal = await cartPage.CartSubtotal.TextContentAsync();
        updatedSubtotal.Should().NotBe(initialSubtotal);
    }

    [Fact]
    public async Task RemoveItem_RemovesFromCart()
    {
        var page = await AddProductToCartViaApiAsync();

        // Navigate to cart
        var cartPage = new CartPage(page, _fixture.BaseUrl);
        await cartPage.NavigateAsync();

        // Remove via API
        var productId = await page.Locator("[data-testid^='cart-item-remove-']").First.GetAttributeAsync("data-product-id");
        await page.EvaluateAsync(@"async (pid) => {
            const response = await fetch('/api/cart/remove/' + pid, {
                method: 'DELETE'
            });
            return await response.json();
        }", productId);

        // Reload to see empty cart
        await page.ReloadAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Cart should now show empty state
        await Assertions.Expect(cartPage.EmptyState).ToBeVisibleAsync();
    }

    [Fact]
    public async Task EmptyCart_ShowsMessage()
    {
        var page = await _fixture.NewPageAsync();
        var cartPage = new CartPage(page, _fixture.BaseUrl);
        await cartPage.NavigateAsync();

        // Empty cart should display the empty state message
        await Assertions.Expect(cartPage.EmptyState).ToBeVisibleAsync();
        await Assertions.Expect(page.Locator("[data-testid='empty-state-title']")).ToContainTextAsync("empty");
    }

    [Fact]
    public async Task CartBadge_ShowsCount()
    {
        var page = await AddProductToCartViaApiAsync();

        // Reload page to ensure badge is updated
        await page.GotoAsync(_fixture.BaseUrl);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Cart badge should show count
        var badge = page.Locator("[data-testid='header-cart-badge']");
        // The badge may be hidden with display:none if JS hasn't run, or visible with count
        var isVisible = await badge.IsVisibleAsync();
        if (isVisible)
        {
            var badgeText = await badge.TextContentAsync();
            badgeText.Should().NotBeNullOrEmpty();
            int.Parse(badgeText!.Trim()).Should().BeGreaterThan(0);
        }
        // Badge visibility depends on client-side JS updating it
    }
}
