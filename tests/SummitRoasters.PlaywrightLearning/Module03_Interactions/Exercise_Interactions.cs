namespace SummitRoasters.PlaywrightLearning.Module03_Interactions;

using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;
using SummitRoasters.PlaywrightLearning.Helpers;

/// <summary>
/// MODULE 3 EXERCISES
/// ==================
///
/// Now you'll interact with real page elements.
/// Each exercise builds on Module 1 (navigation) and Module 2 (finding elements).
/// </summary>
[Collection("Learning")]
public class Exercise_Interactions
{
    private readonly LearningBrowserFixture _fixture;

    public Exercise_Interactions(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Exercise01_ClickProductCard_NavigatesToDetail()
    {
        // GOAL: Click a product card on the products page and verify navigation
        //
        // STEPS:
        // 1. Navigate to /products
        // 2. Find all product cards: page.Locator("[data-testid^='product-card-']")
        // 3. Click the FIRST product card: .First.ClickAsync()
        // 4. Wait for load: await page.WaitForLoadStateAsync(LoadState.NetworkIdle)
        // 5. Assert the product detail name is visible (data-testid="product-detail-name")
        //
        // EXPECTED: After clicking, you're on a product detail page

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/products");

        var productCards = page.Locator("[data-testid^='product-card-']");
        await productCards.First.ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Assertions.Expect(page.GetByTestId("product-detail-name")).ToBeVisibleAsync();
        // throw new NotImplementedException("Complete this exercise!");
        }

    [Fact]
    public async Task Exercise02_LoginWithValidCredentials()
    {
        // GOAL: Fill in the login form and submit it
        //
        // STEPS:
        // 1. Navigate to /account/login
        // 2. Fill the email field (data-testid="login-email")
        //    with TestCredentials.CustomerSarahEmail
        // 3. Fill the password field (data-testid="login-password")
        //    with TestCredentials.CustomerSarahPassword
        // 4. Click the login button (data-testid="login-submit")
        // 5. Wait for redirect:
        //    await page.WaitForURLAsync(url => !url.Contains("/account/login"),
        //        new PageWaitForURLOptions { Timeout = 10000 });
        // 6. Assert that the user menu toggle is visible:
        //    data-testid="header-user-menu-toggle"
        //
        // EXPECTED: After login, user menu appears (means we're authenticated)

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/account/login");

        await page.GetByTestId("login-email").FillAsync(TestCredentials.CustomerSarahEmail);
        await page.GetByTestId("login-password").FillAsync(TestCredentials.CustomerSarahPassword);

        var loginButton = page.GetByTestId("login-submit");

        await loginButton.ClickAsync();

        await page.WaitForURLAsync(url => !url.Contains("/account/login"),
                new PageWaitForURLOptions { Timeout = 10000 });
        await Assertions.Expect(page.GetByTestId("header-user-menu-toggle")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Exercise03_SearchFromHeader()
    {
        // GOAL: Type a search term in the header and submit
        //
        // STEPS:
        // 1. Navigate to the homepage
        // 2. Find the header search input (data-testid="header-search-input")
        // 3. Fill it with "blend"
        // 4. Press Enter using .PressAsync("Enter")
        // 5. Wait for URL: await page.WaitForURLAsync(new Regex("/[Ss]earch"))
        // 6. Assert search results heading (data-testid="search-results-heading") is visible
        //
        // EXPECTED: Search results page loads with results

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        var headerSearchInput = page.GetByTestId("header-search-input");
        await headerSearchInput.FillAsync("blend");

        await headerSearchInput.PressAsync("Enter");
        await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/[Ss]earch"));

        var searchResultsHeading = page.GetByTestId("search-results-heading");
        await Assertions.Expect(searchResultsHeading).ToBeVisibleAsync(); 
        
        }

    [Fact]
    public async Task Exercise04_SortProductsByPrice()
    {
        // GOAL: Change the sort order on the products page
        //
        // STEPS:
        // 1. Navigate to /products
        // 2. Find the sort select dropdown in the filter sidebar:
        //    page.Locator("[data-testid='products-filter-sidebar'] [data-testid='filter-sort-select']")
        // 3. Select "price-desc" (Price: High to Low) using SelectOptionAsync
        // 4. Click the Apply Filters button (data-testid="filter-apply")
        // 5. Wait: await page.WaitForLoadStateAsync(LoadState.NetworkIdle)
        // 6. Assert that product cards are still visible (at least 1 exists)
        //
        // EXPECTED: Products page reloads with new sort order

        // TODO: Write your code here
        /*
         * FIX SOLUTION (copy these lines if you want):
         *
         * var page = await _fixture.NewPageAsync();
         * await page.GotoAsync($"{_fixture.BaseUrl}/products");
         *
         * var dropdown = page.Locator("[data-testid='products-filter-sidebar'] [data-testid='filter-sort-select']");
         * await dropdown.SelectOptionAsync("price-desc");
         *
         * var filterApplyButton = page.GetByTestId("filter-apply");
         * await filterApplyButton.ClickAsync();
         * await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
         *
         * var productCards = page.Locator("[data-testid^='product-card-']");
         * var productCardCount = await productCards.CountAsync();
         * Assert.True(productCardCount >= 1, "Expected at least one product card after applying filters.");
         */
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/products");

        var filterSidebar = page.GetByTestId("products-filter-sidebar");
        var dropdown = filterSidebar.GetByTestId("filter-sort-select");
        await dropdown.SelectOptionAsync("price-desc");

        var filterApplyButton = filterSidebar.GetByTestId("filter-apply");
        await filterApplyButton.ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var productCards = page.Locator("[data-testid^='product-card-']");
        var productCardCount = await productCards.CountAsync();

        Assert.True(productCardCount >= 1, "Expected at least one product card after applying filters.");
    }

    [Fact]
    public async Task Exercise05_NavigateBetweenCategories()
    {
        // GOAL: Navigate between categories using the header nav links
        //
        // STEPS:
        // 1. Navigate to the homepage
        // 2. Click the "Single Origin" nav link (data-testid="header-nav-single-origin")
        // 3. Wait for load: await page.WaitForLoadStateAsync(LoadState.NetworkIdle)
        // 4. Assert URL contains "category" (filter was applied)
        // 5. Assert at least one product card is visible
        // 6. Click the "Equipment" nav link (data-testid="header-nav-equipment")
        // 7. Wait for load
        // 8. Assert at least one product card is visible
        //
        // WHY THIS MATTERS: Navigation tests catch routing bugs early.
        //
        // EXPECTED: Both category pages load with products

        // TODO: Write your code here

        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        var singleOriginLink = page.GetByTestId("header-nav-single-origin");
        await singleOriginLink.ClickAsync();

        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex("category="));

        var productCards = page.Locator("[data-testid^='product-card-']");
        var productCardsCount = await productCards.CountAsync();
        Assert.True(productCardsCount >= 1, "Product cards count should be greater or equal to 1");

        var equipmentNavLink = page.GetByTestId("header-nav-equipment");
        await equipmentNavLink.ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        productCards = page.Locator("[data-testid^='product-card-']");
        productCardsCount = await productCards.CountAsync();
        Assert.True(productCardsCount >= 1, "Product cards count should be greater or equal to 1");

    }
}
