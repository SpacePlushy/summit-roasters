namespace SummitRoasters.PlaywrightLearning.Module06_PageObjectModel;

using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;
using SummitRoasters.PlaywrightLearning.Module06_PageObjectModel.PageObjects;

/// <summary>
/// MODULE 6: PAGE OBJECT MODEL
/// ============================
///
/// WHAT YOU'LL LEARN:
/// - How to use page objects to organize test code
/// - Why page objects make tests more readable and maintainable
/// - How to create page objects with locators, navigation, and actions
///
/// COMPARE these examples with Modules 1-5 where we used raw locators.
/// Notice how much cleaner the test code becomes!
///
/// PATTERN:
/// 1. Create a page object class per page (or major component)
/// 2. Put locators as properties
/// 3. Put complex actions as methods
/// 4. Tests instantiate page objects and call their methods
///
/// This is the pattern you'll use at work every day.
/// </summary>
[Collection("Learning")]
public class Lesson_PageObjectIntro
{
    private readonly LearningBrowserFixture _fixture;

    public Lesson_PageObjectIntro(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Example01_UsingAboutPageObject()
    {
        var page = await _fixture.NewPageAsync();

        // Create the page object — it wraps the raw Playwright page
        var aboutPage = new AboutPage(page, _fixture.BaseUrl);

        // Navigate using the page object (no URL string needed here!)
        await aboutPage.NavigateAsync();

        // Assert using page object locators — much more readable
        await Assertions.Expect(aboutPage.Heading).ToBeVisibleAsync();
        await Assertions.Expect(aboutPage.Header).ToBeVisibleAsync();
        await Assertions.Expect(aboutPage.Footer).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Example02_PageObjectActionMethod()
    {
        var page = await _fixture.NewPageAsync();
        var aboutPage = new AboutPage(page, _fixture.BaseUrl);
        await aboutPage.NavigateAsync();

        // Use the page object's action method to navigate to products
        await aboutPage.ClickShopLink();

        // After the action, we're on the products page
        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex("/products"));
    }

    [Fact]
    public async Task Example03_MultiplePageObjectsTogether()
    {
        var page = await _fixture.NewPageAsync();

        // Start on the About page
        var aboutPage = new AboutPage(page, _fixture.BaseUrl);
        await aboutPage.NavigateAsync();

        // Navigate to products via the shop link
        await aboutPage.ClickShopLink();

        // Now use a DIFFERENT page object for the products page
        // This shows how page objects chain together in a user flow
        var productsPage = new ProductListPage(page, _fixture.BaseUrl);
        await Assertions.Expect(productsPage.Heading).ToBeVisibleAsync();
        await Assertions.Expect(productsPage.ProductCount).ToBeVisibleAsync();
    }
}
