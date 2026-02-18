namespace SummitRoasters.PlaywrightLearning.Module03_Interactions;

using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;
using SummitRoasters.PlaywrightLearning.Helpers;

/// <summary>
/// MODULE 3: INTERACTIONS
/// ======================
///
/// WHAT YOU'LL LEARN:
/// - ClickAsync()          — click buttons, links, checkboxes
/// - FillAsync()           — clear a field and type into it
/// - PressAsync()          — press keyboard keys (Enter, Tab, etc.)
/// - SelectOptionAsync()   — choose from a dropdown/select element
///
/// IMPORTANT CONCEPT — AUTO-WAITING:
/// Playwright automatically waits for elements to be:
/// 1. Attached to the DOM
/// 2. Visible
/// 3. Stable (not animating)
/// 4. Enabled (not disabled)
/// 5. Not obscured by other elements
///
/// You rarely need manual waits! Playwright handles timing for you.
/// This is a HUGE advantage over older tools like Selenium.
/// </summary>
[Collection("Learning")]
public class Lesson_ClickFillType
{
    private readonly LearningBrowserFixture _fixture;

    public Lesson_ClickFillType(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Example01_ClickNavigationLink()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        // Click the "Shop" navigation link
        await page.GetByTestId("header-nav-shop").ClickAsync();

        // After clicking, the page navigates. Playwright auto-waits.
        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex("/products"));
    }

    [Fact]
    public async Task Example02_FillLoginForm()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/account/login");

        // FillAsync CLEARS the field first, then types the text.
        // This is different from TypeAsync which appends to existing text.
        await page.GetByTestId("login-email").FillAsync(TestCredentials.CustomerSarahEmail);
        await page.GetByTestId("login-password").FillAsync(TestCredentials.CustomerSarahPassword);

        // Verify the fields have the values we entered
        await Assertions.Expect(page.GetByTestId("login-email"))
            .ToHaveValueAsync(TestCredentials.CustomerSarahEmail);
        await Assertions.Expect(page.GetByTestId("login-password"))
            .ToHaveValueAsync(TestCredentials.CustomerSarahPassword);
    }

    [Fact]
    public async Task Example03_SearchWithKeyboardSubmit()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        // Fill the search input in the header
        var searchInput = page.GetByTestId("header-search-input");
        await searchInput.FillAsync("Ethiopian");

        // Press Enter to submit the search form
        await searchInput.PressAsync("Enter");

        // Wait for navigation to the search results page
        await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/[Ss]earch"));

        await Assertions.Expect(page.GetByTestId("search-results-heading")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Example04_SelectDropdownOption()
    {
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/products");

        // SelectOptionAsync works on <select> elements
        // You can select by value, label, or index
        var sortSelect = page.Locator(
            "[data-testid='products-filter-sidebar'] [data-testid='filter-sort-select']");

        // Select "Price: Low to High" by its value attribute
        await sortSelect.SelectOptionAsync("price-asc");

        // Verify the selection was made
        await Assertions.Expect(sortSelect).ToHaveValueAsync("price-asc");
    }
}
