namespace SummitRoasters.PlaywrightLearning.Module07_RealWorldWorkflows;

using FluentAssertions;
using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;
using SummitRoasters.PlaywrightLearning.Helpers;
using SummitRoasters.PlaywrightLearning.Module07_RealWorldWorkflows.PageObjects;

/// <summary>
/// MODULE 7 EXERCISES — CAPSTONE
/// ==============================
///
/// These are the capstone exercises. Each one is a realistic user journey
/// that combines EVERYTHING you've learned in Modules 1-6.
///
/// TIPS:
/// - Use the WorkflowLoginPage for any test that needs authentication
/// - Break your test into clear STEPS with comments
/// - If a step fails, add a screenshot to debug:
///   await page.ScreenshotAsync(new() { Path = "debug.png" });
/// - Run these headed to watch the flow:
///   HEADED=1 SLOW_MO=500 dotnet test --filter "Exercise_FullJourneys"
/// </summary>
[Collection("Learning")]
public class Exercise_FullJourneys
{
    private readonly LearningBrowserFixture _fixture;

    public Exercise_FullJourneys(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Exercise01_LoginBrowseAndSearch()
    {
        // GOAL: Simulate a user who logs in, browses products, then searches
        //
        // STEPS:
        // 1. Create a new page and a WorkflowLoginPage:
        //    var page = await _fixture.NewPageAsync();
        //    var loginPage = new WorkflowLoginPage(page, _fixture.BaseUrl);
        // 2. Login as customer: await loginPage.LoginAsCustomerAsync();
        // 3. Navigate to /products
        // 4. Assert product cards are visible (count > 0)
        // 5. Use the header search to search for "Ethiopian":
        //    - Fill data-testid="header-search-input" with "Ethiopian"
        //    - Press Enter
        //    - Wait for URL to contain "/search" or "/Search"
        // 6. Assert search results heading is visible
        //
        // EXPECTED: Full login -> browse -> search flow works

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }

    [Fact]
    public async Task Exercise02_VerifyEmptyCartExperience()
    {
        // GOAL: Verify the empty cart shows a friendly message
        //
        // STEPS:
        // 1. Create a new page (no login needed for cart)
        // 2. Navigate to /cart
        // 3. Wait for load: await page.WaitForLoadStateAsync(LoadState.NetworkIdle)
        // 4. Assert the empty state is visible (data-testid="empty-state")
        // 5. Assert the empty state title contains "empty"
        //    HINT: Use ToContainTextAsync with a case-insensitive Regex:
        //    new Regex("empty", RegexOptions.IgnoreCase)
        // 6. Find the action button (data-testid="empty-state-action")
        // 7. If it's visible, click it and verify it navigates to /products
        //
        // EXPECTED: Empty cart shows friendly message with option to browse

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }

    [Fact]
    public async Task Exercise03_AdminAccessControl()
    {
        // GOAL: Verify that admin pages are properly protected
        //
        // This is THREE mini-tests in one — testing different access levels.
        //
        // PART A — Unauthenticated access:
        // 1. Create a new page (don't login)
        // 2. Navigate to /admin
        // 3. Assert you were redirected to login (URL contains "/account/login")
        //
        // PART B — Customer access (wrong role):
        // 4. Create a NEW page (fresh session)
        // 5. Create a WorkflowLoginPage and login as customer
        // 6. Try navigating to /admin:
        //    var response = await page.GotoAsync($"{_fixture.BaseUrl}/admin");
        // 7. Check the response — you should get 403 (Forbidden) or be redirected
        //    HINT: var status = response?.Status ?? 0;
        //          The URL might contain "AccessDenied" or status might be 403
        //
        // PART C — Admin access (correct role):
        // 8. Create another NEW page
        // 9. Login as admin: loginPage.LoginAsAdminAsync()
        // 10. Navigate to /admin
        // 11. Assert the admin dashboard heading is visible
        //
        // EXPECTED: Only admin users can access admin pages

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }

    [Fact]
    public async Task Exercise04_FullPurchaseJourney()
    {
        // GOAL: Complete an entire purchase flow from login to order confirmation
        //
        // This is the ULTIMATE capstone exercise. It combines every module.
        //
        // STEPS:
        // 1. Create a new page and login as customer (WorkflowLoginPage)
        //
        // 2. Navigate to /products and click the first product card
        //
        // 3. On the product detail page, get the product ID:
        //    var productId = await page.Locator(
        //        "[data-testid='product-detail-form'] input[name='productId']")
        //        .GetAttributeAsync("value");
        //
        // 4. Add to cart via API (copy the pattern from Example01 in the Lesson):
        //    await page.EvaluateAsync(@"async (pid) => {
        //        const response = await fetch('/api/cart/add', {
        //            method: 'POST',
        //            headers: { 'Content-Type': 'application/json' },
        //            body: JSON.stringify({ productId: parseInt(pid), quantity: 1 })
        //        });
        //        return await response.json();
        //    }", productId);
        //
        // 5. Navigate to /cart
        //    Assert cart-items-list is visible (item was added)
        //
        // 6. Navigate to /checkout
        //    Assert checkout heading is visible
        //
        // 7. Fill the shipping form:
        //    - checkout-shipping-name   -> "Test Customer"
        //    - checkout-shipping-phone  -> "555-0199"
        //    - checkout-shipping-street -> "456 Test Ave"
        //    - checkout-shipping-city   -> "Portland"
        //    - checkout-shipping-state  -> "OR"
        //    - checkout-shipping-zip    -> "97201"
        //
        // 8. Look for a "Place Order" or "Continue" button and click it
        //    HINT: data-testid="checkout-place-order"
        //
        // 9. Wait briefly: await page.WaitForTimeoutAsync(2000)
        //
        // 10. Check the result:
        //     - If URL contains "confirmation": GREAT! Assert the heading is visible.
        //     - If still on checkout: That's OK too — the form might need
        //       additional steps. Assert checkout heading is still visible.
        //     HINT: if (page.Url.Contains("confirmation")) { ... } else { ... }
        //
        // EXPECTED: You make it to either confirmation or stay on checkout (both valid)

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }
}
