namespace SummitRoasters.PlaywrightLearning.Module05_WaitingAndAsync;

using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;
using SummitRoasters.PlaywrightLearning.Helpers;

/// <summary>
/// MODULE 5: WAITING AND ASYNC PATTERNS
/// =====================================
///
/// WHAT YOU'LL LEARN:
/// - Why Playwright's auto-waiting usually suffices
/// - WaitForLoadStateAsync()     — wait for page load states
/// - WaitForURLAsync()           — wait for URL to change
/// - WaitForTimeoutAsync()       — hard wait (AVOID in real tests)
/// - PageGotoOptions.WaitUntil   — control what "loaded" means
///
/// KEY CONCEPTS:
/// - LoadState.DOMContentLoaded : HTML parsed, DOM ready
/// - LoadState.Load             : All resources loaded (images, scripts)
/// - LoadState.NetworkIdle      : No network requests for 500ms
///
/// GOLDEN RULE: Prefer auto-waiting assertions over manual waits.
/// If you use WaitForTimeoutAsync a lot, there's usually a better way.
/// </summary>
[Collection("Learning")]
public class Lesson_WaitingPatterns
{
    private readonly LearningBrowserFixture _fixture;

    public Lesson_WaitingPatterns(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Example01_WaitForNavigation()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        // Click a link and wait for the URL to match a glob pattern
        await page.GetByTestId("header-nav-shop").ClickAsync();

        // The ** is a glob pattern matching any characters
        await page.WaitForURLAsync("**/products**");

        // Alternative: wait with a predicate function
        // await page.WaitForURLAsync(url => url.Contains("/products"));

        await Assertions.Expect(page.GetByTestId("products-heading")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Example02_WaitForNetworkIdle()
    {
        var page = await _fixture.NewPageAsync();

        // GotoAsync with WaitUntil option
        // NetworkIdle means ALL network requests have finished
        await page.GotoAsync($"{_fixture.BaseUrl}/products", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });

        // At this point, all async data loading is complete
        var cards = page.Locator("[data-testid^='product-card-']");
        var count = await cards.CountAsync();
        Assert.True(count > 0, "Products should be loaded after NetworkIdle");
    }

    [Fact]
    public async Task Example03_WaitForURLAfterFormSubmit()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/account/login");

        // Fill and submit login form
        await page.GetByTestId("login-email").FillAsync(TestCredentials.CustomerSarahEmail);
        await page.GetByTestId("login-password").FillAsync(TestCredentials.CustomerSarahPassword);
        await page.GetByTestId("login-submit").ClickAsync();

        // Wait for the URL to change AWAY from the login page
        // This is better than a fixed sleep because it resolves
        // as soon as the redirect happens
        await page.WaitForURLAsync(
            url => !url.Contains("/account/login"),
            new PageWaitForURLOptions { Timeout = 10000 });

        // Now we know the login succeeded
        await Assertions.Expect(page.GetByTestId("header-user-menu-toggle")).ToBeVisibleAsync();
    }
}
