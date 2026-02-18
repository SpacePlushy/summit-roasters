namespace SummitRoasters.PlaywrightLearning.Module07_RealWorldWorkflows;

using FluentAssertions;
using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;
using SummitRoasters.PlaywrightLearning.Module07_RealWorldWorkflows.PageObjects;

/// <summary>
/// MODULE 7: REAL-WORLD WORKFLOWS
/// ===============================
///
/// WHAT YOU'LL LEARN:
/// - Multi-step test flows (login -> browse -> add to cart -> checkout)
/// - Test setup helpers (logging in before the real test begins)
/// - Negative testing (verifying what should NOT happen)
/// - Using page.EvaluateAsync to call APIs from the browser
///
/// REAL-WORLD CONTEXT:
/// At your job, most tests aren't single-action tests.
/// They test USER JOURNEYS — sequences of actions a real user would take.
/// These are the tests that catch the most bugs because they exercise
/// how features interact with each other.
/// </summary>
[Collection("Learning")]
public class Lesson_EndToEndFlows
{
    private readonly LearningBrowserFixture _fixture;

    public Lesson_EndToEndFlows(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Example01_BrowseAndAddToCart()
    {
        // This test simulates a user browsing products and adding one to cart
        var page = await _fixture.NewPageAsync();

        // STEP 1: Start on the products page
        await page.GotoAsync($"{_fixture.BaseUrl}/products");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // STEP 2: Click the first product to view details
        await page.Locator("[data-testid^='product-card-']").First.ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Capture the product name for later verification
        var productName = await page.GetByTestId("product-detail-name").TextContentAsync();
        productName.Should().NotBeNullOrEmpty("Product detail should show a name");

        // STEP 3: Add to cart via the JavaScript API
        // The "Add to Cart" button in this app triggers a fetch() call.
        // We can call the same API directly using page.EvaluateAsync.
        var productId = await page.Locator(
            "[data-testid='product-detail-form'] input[name='productId']")
            .GetAttributeAsync("value");

        await page.EvaluateAsync(@"async (pid) => {
            const response = await fetch('/api/cart/add', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ productId: parseInt(pid), quantity: 1 })
            });
            return await response.json();
        }", productId);

        // STEP 4: Navigate to cart and verify the item is there
        await page.GotoAsync($"{_fixture.BaseUrl}/cart");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Assertions.Expect(page.GetByTestId("cart-heading")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByTestId("cart-items-list")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Example02_ProtectedPageRedirectsToLogin()
    {
        // NEGATIVE TEST: Verify unauthenticated users can't access protected pages
        var page = await _fixture.NewPageAsync();

        // Try to access checkout without being logged in
        await page.GotoAsync($"{_fixture.BaseUrl}/checkout");

        // Should be redirected to login
        page.Url.ToLower().Should().Contain("/account/login",
            "Unauthenticated users should be redirected to login");

        // The login form should be visible
        await Assertions.Expect(page.GetByTestId("login-form")).ToBeVisibleAsync();
    }
}
