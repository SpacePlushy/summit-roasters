namespace SummitRoasters.PlaywrightLearning.Module01_FirstSteps;

using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;

/// <summary>
/// MODULE 1: FIRST STEPS
/// ====================
///
/// Welcome! These are your first Playwright tests.
///
/// WHAT YOU'LL LEARN:
/// - How a Playwright test is structured
/// - GotoAsync()         — navigating to a URL
/// - ToHaveTitleAsync()  — checking the page title
/// - ToHaveURLAsync()    — checking the current URL
/// - Regex patterns in assertions
///
/// HOW TO RUN THESE TESTS:
///   dotnet test tests/SummitRoasters.PlaywrightLearning --filter "Module01"
///
/// HOW TO RUN IN HEADED MODE (see the browser):
///   HEADED=1 dotnet test tests/SummitRoasters.PlaywrightLearning --filter "Module01"
///
/// HOW TO RUN WITH SLOW MOTION:
///   HEADED=1 SLOW_MO=500 dotnet test tests/SummitRoasters.PlaywrightLearning --filter "Module01"
///
/// BEFORE YOU START:
///   Make sure the app is running: dotnet run --project src/SummitRoasters.Web
///   The app runs at http://localhost:5273
/// </summary>
[Collection("Learning")]
public class Lesson_NavigationAndTitles
{
    private readonly LearningBrowserFixture _fixture;

    // Constructor injection: xUnit gives us the shared browser fixture
    public Lesson_NavigationAndTitles(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Example01_NavigateToHomepage_CheckTitle()
    {
        // ARRANGE: Get a fresh browser page
        var page = await _fixture.NewPageAsync();

        // ACT: Navigate to the homepage
        await page.GotoAsync(_fixture.BaseUrl);

        // ASSERT: Verify the page title contains "Summit Roasters"
        // We use Regex because the full title might be "Summit Roasters - Home"
        await Assertions.Expect(page).ToHaveTitleAsync(
            new System.Text.RegularExpressions.Regex("Summit Roasters"));
    }

    [Fact]
    public async Task Example02_NavigateToProducts_CheckURL()
    {
        var page = await _fixture.NewPageAsync();

        // Navigate to the products page using a full URL
        await page.GotoAsync($"{_fixture.BaseUrl}/products");

        // Verify the URL contains "/products"
        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex("/products"));
    }

    [Fact]
    public async Task Example03_NavigateToLogin_CheckBothTitleAndURL()
    {
        var page = await _fixture.NewPageAsync();

        await page.GotoAsync($"{_fixture.BaseUrl}/account/login");

        // You can make multiple assertions — if any fails, the test fails
        await Assertions.Expect(page).ToHaveTitleAsync(
            new System.Text.RegularExpressions.Regex("Sign [Ii]n|Login"));
        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex("/account/login"));
    }
}
