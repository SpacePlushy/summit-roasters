namespace SummitRoasters.PlaywrightLearning.Module06_PageObjectModel;

using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;
using SummitRoasters.PlaywrightLearning.Module06_PageObjectModel.PageObjects;

/// <summary>
/// MODULE 6 EXERCISES
/// ==================
///
/// PREREQUISITE: Complete the TODO items in PageObjects/ProductListPage.cs first!
/// Then come back here and write tests that USE your completed page object.
///
/// The exercises will NOT work until you add the missing locators and methods
/// to ProductListPage.cs. That's the whole point — build the page object first,
/// then use it in tests.
/// </summary>
[Collection("Learning")]
public class Exercise_BuildPageObject
{
    private readonly LearningBrowserFixture _fixture;

    public Exercise_BuildPageObject(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Exercise01_NavigateAndVerifyProducts()
    {
        // GOAL: Use your completed ProductListPage to navigate and verify products
        //
        // PREREQUISITE: You must have completed the TODOs in ProductListPage.cs
        //
        // STEPS:
        // 1. Create a new page with _fixture.NewPageAsync()
        // 2. Create a ProductListPage instance: new ProductListPage(page, _fixture.BaseUrl)
        // 3. Navigate using the page object: await productsPage.NavigateAsync()
        // 4. Assert the Heading is visible
        // 5. Use GetProductCardCountAsync() to get the count
        // 6. Assert count > 0
        //
        // EXPECTED: Products page loads, count is positive

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }

    [Fact]
    public async Task Exercise02_NavigateWithCategoryFilter()
    {
        // GOAL: Navigate to a specific category using the page object
        //
        // STEPS:
        // 1. Create a ProductListPage
        // 2. Navigate with a category: await productsPage.NavigateAsync("Blend")
        // 3. Assert Heading is visible
        // 4. Assert ProductCount is visible
        // 5. Use GetProductCardCountAsync() and assert count > 0
        //
        // EXPECTED: Filtered products page shows blend products

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }

    [Fact]
    public async Task Exercise03_SortProducts()
    {
        // GOAL: Sort products using the page object's SortByAsync method
        //
        // STEPS:
        // 1. Create a ProductListPage and navigate (no category filter)
        // 2. Call await productsPage.SortByAsync("price-asc")
        // 3. Assert product cards still exist (count > 0)
        //
        // EXPECTED: Products sorted by price, results still showing

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }

    [Fact]
    public async Task Exercise04_ClickThroughToProductDetail()
    {
        // GOAL: Use ClickFirstProductAsync() to navigate to product detail
        //
        // STEPS:
        // 1. Create a ProductListPage and navigate
        // 2. Call await productsPage.ClickFirstProductAsync()
        // 3. After the click, use the raw page to assert:
        //    - URL contains "/products/" (the slug-based detail URL)
        //    - product-detail-name is visible
        //    - product-detail-price is visible
        //    - add-to-cart-button is visible
        //
        // HINT: Access the raw page via productsPage.Page
        //
        // EXPECTED: Detail page loads with full product information

        // TODO: Write your code here
        throw new NotImplementedException("Complete this exercise!");
    }
}
