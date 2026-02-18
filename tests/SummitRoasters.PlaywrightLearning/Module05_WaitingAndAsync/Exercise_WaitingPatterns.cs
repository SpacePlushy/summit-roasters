namespace SummitRoasters.PlaywrightLearning.Module05_WaitingAndAsync;

using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;
using SummitRoasters.PlaywrightLearning.Helpers;

/// <summary>
/// MODULE 5 EXERCISES
/// ==================
///
/// These exercises involve operations where timing matters:
/// form submissions, page transitions, and dynamic content loading.
/// </summary>
[Collection("Learning")]
public class Exercise_WaitingPatterns
{
    private readonly LearningBrowserFixture _fixture;

    public Exercise_WaitingPatterns(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Exercise01_NavigateAndWaitForFullLoad()
    {
        // GOAL: Navigate to the products page and wait for everything to load
        //
        // STEPS:
        // 1. Create a new page
        // 2. Navigate to /products using GotoAsync with:
        //    new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle }
        // 3. After load, count product cards and assert count > 0
        // 4. Assert the products count text (data-testid="products-count") is visible
        //
        // EXPECTED: Products fully loaded with product count displayed

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }

    [Fact]
    public async Task Exercise02_LoginAndWaitForRedirect()
    {
        // GOAL: Login and wait for redirect, then verify authenticated state
        //
        // STEPS:
        // 1. Navigate to /account/login
        // 2. Fill email with TestCredentials.CustomerMikeEmail
        // 3. Fill password with TestCredentials.CustomerMikePassword
        // 4. Click the login button (data-testid="login-submit")
        // 5. Wait for URL to no longer contain "/account/login":
        //    await page.WaitForURLAsync(
        //        url => !url.Contains("/account/login"),
        //        new PageWaitForURLOptions { Timeout = 10000 });
        // 6. Assert user menu toggle (data-testid="header-user-menu-toggle") is visible
        //
        // WHY THIS MATTERS: Login redirects are a common source of race conditions.
        // Without proper waiting, you might assert before the redirect happens.
        //
        // EXPECTED: Mike is logged in, user menu visible

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }

    [Fact]
    public async Task Exercise03_FilterAndWaitForReload()
    {
        // GOAL: Apply a sort filter and wait for the page to reload
        //
        // STEPS:
        // 1. Navigate to /products with NetworkIdle
        // 2. Select "price-asc" from the sort dropdown in the filter sidebar:
        //    page.Locator("[data-testid='products-filter-sidebar'] [data-testid='filter-sort-select']")
        // 3. Click Apply Filters (data-testid="filter-apply")
        // 4. Wait: await page.WaitForLoadStateAsync(LoadState.NetworkIdle)
        // 5. Assert product cards are still visible (count > 0)
        // 6. Assert the URL changed (contains "sort" or "sortBy")
        //
        // EXPECTED: Products page reloads with sort applied

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }

    [Fact]
    public async Task Exercise04_SearchAndWaitForResults()
    {
        // GOAL: Perform a search and properly wait for results
        //
        // STEPS:
        // 1. Navigate to the homepage
        // 2. Fill the header search input with "dark"
        // 3. Press Enter
        // 4. Wait for URL to contain "/search" or "/Search":
        //    await page.WaitForURLAsync(
        //        new System.Text.RegularExpressions.Regex("/[Ss]earch"));
        // 5. Wait for load: await page.WaitForLoadStateAsync(LoadState.NetworkIdle)
        // 6. Assert the search heading (data-testid="search-results-heading") is visible
        //
        // EXPECTED: Search results page loads with correct query

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }
}
