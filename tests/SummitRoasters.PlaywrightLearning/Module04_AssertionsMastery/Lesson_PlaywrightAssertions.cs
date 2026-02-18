namespace SummitRoasters.PlaywrightLearning.Module04_AssertionsMastery;

using FluentAssertions;
using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;

/// <summary>
/// MODULE 4: ASSERTIONS MASTERY
/// ============================
///
/// WHAT YOU'LL LEARN:
/// - ToBeVisibleAsync()       — is the element on screen?
/// - ToContainTextAsync()     — does the element contain specific text?
/// - ToHaveTextAsync()        — does the element have EXACTLY this text?
/// - ToHaveAttributeAsync()   — does the element have a specific attribute?
/// - ToHaveValueAsync()       — what value does an input field have?
/// - ToHaveCountAsync()       — how many elements match?
/// - ToBeEnabledAsync()       — is the element enabled/disabled?
/// - Not (negation)           — expect something to NOT be true
///
/// TWO ASSERTION LIBRARIES:
/// 1. Playwright Assertions:  Assertions.Expect(locator).To...()
///    - Auto-retrying! Waits up to 5 seconds for the condition to become true
///    - ALWAYS prefer these for UI assertions
///
/// 2. FluentAssertions:  value.Should().Be(...)
///    - For asserting plain values (strings, numbers, booleans)
///    - Does NOT auto-retry — use only for values already extracted
/// </summary>
[Collection("Learning")]
public class Lesson_PlaywrightAssertions
{
    private readonly LearningBrowserFixture _fixture;

    public Lesson_PlaywrightAssertions(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Example01_VisibilityAssertions()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        // Assert element IS visible
        await Assertions.Expect(page.GetByTestId("hero-section")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByTestId("header")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByTestId("footer")).ToBeVisibleAsync();

        // Assert element is NOT visible (negation with .Not)
        // The auth links should be visible when logged out,
        // but the user dropdown should not be
        await Assertions.Expect(page.GetByTestId("header-auth-links")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Example02_TextAssertions()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        // ToContainTextAsync — partial match (most commonly used)
        await Assertions.Expect(page.GetByTestId("hero-heading"))
            .ToContainTextAsync("Summit");

        // ToHaveTextAsync with Regex — flexible matching
        await Assertions.Expect(page.GetByTestId("hero-heading"))
            .ToHaveTextAsync(new System.Text.RegularExpressions.Regex("Summit Roasters"));
    }

    [Fact]
    public async Task Example03_CountAssertions()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/products");

        // Count the product cards
        var productCards = page.Locator("[data-testid^='product-card-']");

        // Use CountAsync to get the count, then FluentAssertions
        var count = await productCards.CountAsync();
        count.Should().BeGreaterThan(0, "Product listing should show products");
    }

    [Fact]
    public async Task Example04_AttributeAndValueAssertions()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/account/login");

        var emailInput = page.GetByTestId("login-email");

        // Check that the input has the correct type attribute
        await Assertions.Expect(emailInput).ToHaveAttributeAsync("type", "email");

        // Fill a value and assert it was set correctly
        await emailInput.FillAsync("test@test.com");
        await Assertions.Expect(emailInput).ToHaveValueAsync("test@test.com");
    }
}
