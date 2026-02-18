namespace SummitRoasters.PlaywrightLearning.Fixtures;

using Microsoft.Playwright;

/// <summary>
/// BrowserFixture manages the browser lifecycle for all tests.
///
/// KEY CONCEPTS:
/// - IAsyncLifetime: xUnit interface for async setup/teardown
/// - InitializeAsync() runs BEFORE any test in the collection
/// - DisposeAsync() runs AFTER all tests in the collection finish
/// - We create ONE browser and share it across tests for speed
/// - Each test gets its own Page (via NewPageAsync) for isolation
///
/// ENVIRONMENT VARIABLES:
/// - Set HEADED=1 to see the browser:  HEADED=1 dotnet test
/// - Set SLOW_MO=500 to slow actions:  SLOW_MO=500 dotnet test
///   (combine them: HEADED=1 SLOW_MO=500 dotnet test)
/// </summary>
public class LearningBrowserFixture : IAsyncLifetime
{
    public IPlaywright Playwright { get; private set; } = null!;
    public IBrowser Browser { get; private set; } = null!;
    public string BaseUrl { get; } = "http://localhost:5273";

    public async Task InitializeAsync()
    {
        // Step 1: Create the Playwright instance
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        // Step 2: Launch a Chromium browser
        // Headless = true means no visible window (faster, used in CI)
        // Set HEADED=1 environment variable to watch tests run visually
        var headless = Environment.GetEnvironmentVariable("HEADED") != "1";
        var slowMo = int.TryParse(
            Environment.GetEnvironmentVariable("SLOW_MO"), out var ms) ? ms : 0;

        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless,
            SlowMo = slowMo // Adds delay (ms) between every Playwright action
        });
    }

    public async Task DisposeAsync()
    {
        await Browser.DisposeAsync();
        Playwright.Dispose();
    }

    /// <summary>
    /// Creates a fresh browser page for each test.
    /// Each page has its own session, cookies, and localStorage.
    /// This ensures tests don't interfere with each other.
    /// </summary>
    public async Task<IPage> NewPageAsync()
    {
        var context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        return await context.NewPageAsync();
    }
}
