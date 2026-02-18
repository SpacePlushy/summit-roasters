namespace SummitRoasters.PlaywrightLearning.Module06_PageObjectModel.PageObjects;

using Microsoft.Playwright;

/// <summary>
/// EXAMPLE PAGE OBJECT: AboutPage
///
/// A Page Object encapsulates:
/// 1. How to NAVIGATE to the page
/// 2. How to FIND elements on the page (locators as properties)
/// 3. How to PERFORM actions on the page (methods)
///
/// Benefits:
/// - If the UI changes, you update ONE place (the page object), not every test
/// - Tests read like user stories: aboutPage.NavigateAsync() is clearer than
///   page.GotoAsync($"{baseUrl}/Home/About")
/// - Locators are defined once and reused across many tests
/// </summary>
public class AboutPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public AboutPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    // Expose the underlying page for direct access when needed
    public IPage Page => _page;

    // ============ NAVIGATION ============

    public async Task NavigateAsync() =>
        await _page.GotoAsync($"{_baseUrl}/Home/About");

    // ============ LOCATORS ============
    // Locators are lazy — they don't query the DOM until you use them.
    // Defining them as properties means you never have stale references.

    public ILocator Heading => _page.GetByRole(AriaRole.Heading).First;
    public ILocator Header => _page.GetByTestId("header");
    public ILocator Footer => _page.GetByTestId("footer");

    // ============ ACTIONS ============

    public async Task ClickShopLink()
    {
        await _page.GetByTestId("header-nav-shop").ClickAsync();
        await _page.WaitForURLAsync("**/products**");
    }

    // ============ QUERIES ============

    public async Task<string> GetHeadingTextAsync()
    {
        return await Heading.TextContentAsync() ?? "";
    }
}
