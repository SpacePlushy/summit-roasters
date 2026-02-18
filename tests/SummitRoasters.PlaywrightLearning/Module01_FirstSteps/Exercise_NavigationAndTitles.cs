namespace SummitRoasters.PlaywrightLearning.Module01_FirstSteps;

using System.Text.RegularExpressions;
using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;

/// <summary>
/// MODULE 1 EXERCISES
/// ==================
///
/// Complete each exercise by replacing the TODO comments with working code.
/// Look at the Lesson file for examples of the patterns you need.
///
/// TIPS:
/// - Every test follows the ARRANGE / ACT / ASSERT pattern
/// - Use _fixture.NewPageAsync() to get a fresh page
/// - Use _fixture.BaseUrl as the root URL
/// - Run with: dotnet test --filter "Exercise_NavigationAndTitles"
/// </summary>
[Collection("Learning")]
public class Exercise_NavigationAndTitles
{
    private readonly LearningBrowserFixture _fixture;

    public Exercise_NavigationAndTitles(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Exercise01_NavigateToRegisterPage()
    {
        // GOAL: Navigate to the registration page and verify the URL
        //
        // STEPS:
        // 1. Create a new page with _fixture.NewPageAsync()
        // 2. Navigate to {BaseUrl}/account/register
        // 3. Assert the URL contains "/account/register"
        //
        // EXPECTED: Test passes (green) when the URL assertion succeeds

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/account/register");

        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex("/account/register"));

    }

    [Fact]
    public async Task Exercise02_NavigateToCart()
    {
        // GOAL: Navigate to the shopping cart page and verify the title
        //
        // HINT: The cart page is at /cart
        // HINT: The page title should contain "Cart" (use Regex)
        //
        // EXPECTED: Test passes when title contains "Cart"

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();

        await page.GotoAsync($"{_fixture.BaseUrl}/cart");

        await Assertions.Expect(page).ToHaveTitleAsync(
            new System.Text.RegularExpressions.Regex("Cart"));

    }

    [Fact]
    public async Task Exercise03_NavigateToAboutPage()
    {
        // GOAL: Navigate to the about page and check both title and URL
        //
        // HINT: The about page is at /Home/About
        // HINT: Check that URL contains "About" AND title contains "About"
        //
        // EXPECTED: Both assertions pass

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();

        await page.GotoAsync($"{_fixture.BaseUrl}/Home/About");
        
        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex("About")
        );
        await Assertions.Expect(page).ToHaveTitleAsync(
            new System.Text.RegularExpressions.Regex("About")
        );
    }

    [Fact]
    public async Task Exercise04_NavigateToSearchWithQueryString()
    {
        // GOAL: Navigate to a search page with a query parameter
        //
        // HINT: The search URL with a query looks like: /search?q=Ethiopian
        // HINT: After navigation, verify the URL contains "q=Ethiopian"
        //
        // WHY THIS MATTERS: Real-world apps use query strings constantly.
        // Verifying query parameters are preserved tests that routing works.
        //
        // EXPECTED: URL contains the search query parameter

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();

        await page.GotoAsync($"{_fixture.BaseUrl}/search?q=Ethiopian");

        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex("q=Ethiopian"));

    }
}
