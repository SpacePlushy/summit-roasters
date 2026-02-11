using SummitRoasters.E2ETests.PageObjects;

namespace SummitRoasters.E2ETests;

public class SmokeTests
{
    private const string BaseUrl = "https://localhost:5001";

    // TODO: Set up Playwright browser in test fixture
    // private IPlaywright _playwright;
    // private IBrowser _browser;
    // private IPage _page;

    [Fact(Skip = "E2E tests require running application")]
    public async Task Homepage_Loads_Successfully()
    {
        // var page = ... // create page from browser
        // var homePage = new HomePage(page, BaseUrl);
        // await homePage.NavigateAsync();
        // await Expect(page).ToHaveTitleAsync(new Regex("Summit Roasters"));
        await Task.CompletedTask;
    }

    // TODO: Navigation links work and lead to correct pages
    // TODO: Footer is visible on all pages
    // TODO: Mobile menu toggles on small screens
}
