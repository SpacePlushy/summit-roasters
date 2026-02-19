namespace SummitRoasters.PlaywrightLearning.Module04_AssertionsMastery;

using FluentAssertions;
using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;

/// <summary>
/// MODULE 4 EXERCISES
/// ==================
///
/// These exercises combine everything you've learned so far:
/// navigation (Module 1), finding elements (Module 2),
/// interactions (Module 3), plus new assertion techniques.
/// </summary>
[Collection("Learning")]
public class Exercise_Assertions
{
    private readonly LearningBrowserFixture _fixture;

    public Exercise_Assertions(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Exercise01_VerifyHomepageSections()
    {
        // GOAL: Verify all major sections exist on the homepage
        //
        // STEPS:
        // 1. Navigate to the homepage
        // 2. Assert each of these sections is visible (use GetByTestId):
        //    - hero-section
        //    - featured-products-section
        //    - categories-section
        //    - newsletter-section
        //    - footer
        // 3. Assert the hero heading contains "Summit Roasters"
        // 4. Assert the footer copyright (data-testid="footer-copyright")
        //    contains "2026"
        //
        // EXPECTED: All 5 sections visible, text assertions pass

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);
        await Assertions.Expect(page.GetByTestId("hero-section")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByTestId("featured-products-section")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByTestId("categories-section")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByTestId("newsletter-section")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByTestId("footer")).ToBeVisibleAsync();

        await Assertions.Expect(page.GetByTestId("hero-heading")).ToContainTextAsync("Summit Roasters");
        await Assertions.Expect(page.GetByTestId("footer-copyright")).ToContainTextAsync("2026");
        
    }

    [Fact]
    public async Task Exercise02_VerifyProductDetailContent()
    {
        // GOAL: Navigate to a product and verify all content areas exist
        //
        // STEPS:
        // 1. Navigate to /products
        // 2. Click the first product card
        // 3. Wait: await page.WaitForLoadStateAsync(LoadState.NetworkIdle)
        // 4. Assert these elements are visible:
        //    - product-detail-name
        //    - product-detail-price   (should contain "$")
        //    - product-detail-description
        //    - add-to-cart-button
        // 5. Get the price text with .TextContentAsync()
        //    and use FluentAssertions to verify it contains "$"
        //    HINT: priceText.Should().Contain("$");
        //
        // EXPECTED: All product detail elements visible, price has dollar sign

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/products");
        await page.GetByTestId("product-card-1").ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Assertions.Expect(page.GetByTestId("product-detail-name")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByTestId("product-detail-price")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByTestId("product-detail-description")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByTestId("add-to-cart-button")).ToBeVisibleAsync();

        var priceText = await page.GetByTestId("product-detail-price").TextContentAsync();
        priceText.Should().Contain("$");
    }

    [Fact]
    public async Task Exercise03_CountCategoryCards()
    {
        // GOAL: Verify the homepage has exactly 3 category cards
        //
        // STEPS:
        // 1. Navigate to the homepage
        // 2. Find the categories grid (data-testid="categories-grid")
        // 3. Count child elements inside it
        //    HINT: page.GetByTestId("categories-grid").Locator("> *")
        //    or find specific category cards:
        //    page.Locator("[data-testid^='category-card-']")
        // 4. Assert the count is exactly 3 (Single Origin, Blends, Equipment)
        //
        // EXPECTED: Exactly 3 category cards exist

        // TODO: Write your code here

        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        var categoriesGrid = page.GetByTestId("categories-grid");
        /*
         * Alternative approach: count children from categoriesGrid directly.
         * This keeps the locator scoped to the grid container.
         *
         * var categoryCardsInGrid = categoriesGrid.Locator("[data-testid^='category-card-']");
         * var count = await categoryCardsInGrid.CountAsync();
         *
         * You can also count all direct child nodes:
         * var childCount = await categoriesGrid.Locator("> *").CountAsync();
         */
        var categoriesChildElements = page.Locator("[data-testid^='category-card-']");

        var count = await categoriesChildElements.CountAsync();

        Assert.True(count == 3, "Exactly 3 category cards should exist");
        
    }

    [Fact]
    public async Task Exercise04_VerifySearchResultsHaveContent()
    {
        // GOAL: Search for "coffee" and verify results have names and prices
        //
        // STEPS:
        // 1. Navigate to /search?q=coffee
        // 2. Wait: await page.WaitForLoadStateAsync(LoadState.NetworkIdle)
        // 3. Assert search-results-heading is visible
        // 4. Count the product cards (data-testid^='product-card-')
        // 5. Assert count > 0
        // 6. For the FIRST card, assert:
        //    - A name element exists (data-testid^='product-card-name-')
        //      inside the first card
        //    - A price element exists (data-testid^='product-card-price-')
        //    - The price text contains "$"
        //
        // EXPECTED: Search returns results, each with name and price

        // TODO: Write your code here

        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/search?q=coffee");

        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Assertions.Expect(page.GetByTestId("search-results-heading")).ToBeVisibleAsync();

        var count = await page.Locator("[data-testid^='product-card-']").CountAsync();
        Assert.True(count > 0, "Count should be more than 0");
    }

    [Fact]
    public async Task Exercise05_VerifyLoginFormAttributes()
    {
        // GOAL: Verify form field attributes on the login page
        //
        // STEPS:
        // 1. Navigate to /account/login
        // 2. Assert the email field (data-testid="login-email"):
        //    - Has attribute "type" = "email"
        //      HINT: Assertions.Expect(field).ToHaveAttributeAsync("type", "email")
        // 3. Assert the password field (data-testid="login-password"):
        //    - Has attribute "type" = "password"
        // 4. Assert the login button (data-testid="login-submit"):
        //    - Is enabled (ToBeEnabledAsync)
        //    - Contains text (ToContainTextAsync with some text)
        // 5. Assert the register link (data-testid="login-register-link"):
        //    - Is visible
        //
        // EXPECTED: All form fields have correct attributes

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }
}
