namespace SummitRoasters.PlaywrightLearning.Module02_FindingElements;

using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Fixtures;

/// <summary>
/// MODULE 2 EXERCISES
/// ==================
///
/// Practice finding elements using different locator strategies.
/// Each exercise tells you WHICH strategy to use.
/// </summary>
[Collection("Learning")]
public class Exercise_LocatorStrategies
{
    private readonly LearningBrowserFixture _fixture;

    public Exercise_LocatorStrategies(LearningBrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Exercise01_FindHeroSectionByTestId()
    {
        // GOAL: Find the hero section on the homepage using GetByTestId
        //
        // STEPS:
        // 1. Navigate to the homepage
        // 2. Use page.GetByTestId("hero-section") to find the hero
        // 3. Assert it is visible
        // 4. Also find the hero CTA button (data-testid="hero-cta-shop") and assert visible
        //
        // EXPECTED: Both elements are visible

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        var heroHeading = page.GetByTestId("hero-section");
        var heroCTAButton = page.GetByTestId("hero-cta-shop");


        await Assertions.Expect(heroHeading).ToBeVisibleAsync();
        await Assertions.Expect(heroCTAButton).ToBeVisibleAsync();

    }

    [Fact]
    public async Task Exercise02_FindFooterSections()
    {
        // GOAL: Find different footer sections and verify they exist
        //
        // STEPS:
        // 1. Navigate to the homepage
        // 2. Find and assert visibility of:
        //    - footer-company-info
        //    - footer-shop-links
        //    - footer-help-links
        //    - footer-newsletter
        // 3. Find the footer copyright (data-testid="footer-copyright")
        //    and assert it contains "2026"
        //
        // EXPECTED: All four sections visible, copyright contains year
    
        // TODO: Write your code here

        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        var footerCompanyInfo = page.GetByTestId("footer-company-info");
        var footerShopLinks = page.GetByTestId("footer-shop-links");
        var footerHelpLinks = page.GetByTestId("footer-help-links");
        var footerNewsletter = page.GetByTestId("footer-newsletter");

        await Assertions.Expect(footerCompanyInfo).ToBeVisibleAsync();
        await Assertions.Expect(footerShopLinks).ToBeVisibleAsync();
        await Assertions.Expect(footerHelpLinks).ToBeVisibleAsync();
        await Assertions.Expect(footerNewsletter).ToBeVisibleAsync();

        var footerCopyright = page.GetByTestId("footer-copyright");
        await Assertions.Expect(footerCopyright).ToContainTextAsync("2026");


    }

    [Fact]
    public async Task Exercise03_FindProductCards()
    {
        // GOAL: Navigate to the products page and find product card elements
        //
        // STEPS:
        // 1. Navigate to /products
        // 2. Find ALL product cards using CSS: [data-testid^='product-card-']
        //    (The ^= means "starts with" — a CSS attribute selector)
        // 3. Assert there is at least 1 product card (use CountAsync())
        // 4. Get the FIRST product card using .First
        //    and assert it is visible
        //
        // EXPECTED: At least 1 product card exists, first card is visible

        // TODO: Write your code here
        var page = await _fixture.NewPageAsync();
        await page.GotoAsync($"{_fixture.BaseUrl}/products");

        var productCards = page.Locator("[data-testid^='product-card-']");

        var productCardCount = await productCards.CountAsync();
        Assert.True(productCardCount >= 1, "There should be at least one product card");

        var firstCard = productCards.First;

        await Assertions.Expect(firstCard).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Exercise04_FindNavigationLinksByRole()
    {
        // GOAL: Find navigation links using GetByRole
        //
        // STEPS:
        // 1. Navigate to the homepage
        // 2. Use page.GetByRole(AriaRole.Link, new() { Name = "About" })
        //    to find the About link
        // 3. Assert it is visible
        // 4. Count all links on the page using page.GetByRole(AriaRole.Link)
        // 5. Assert there are more than 10 links
        //
        // EXPECTED: About link visible, page has many links

        // TODO: Write your code here

        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        
        var headerNav = page.GetByTestId("header-nav");
        var aboutLink = headerNav.GetByRole(AriaRole.Link, new() { Name = "About" });
        await Assertions.Expect(aboutLink).ToBeVisibleAsync();

        var allLinksCount = await page.GetByRole(AriaRole.Link).CountAsync();
        Assert.True(allLinksCount > 10, "Amount of links should be greater than 10");
    }

    [Fact]
    public async Task Exercise05_ChainLocatorsInsideHeader()
    {
        // GOAL: Find elements inside the header using chaining
        //
        // STEPS:
        // 1. Navigate to the homepage
        // 2. Find the header container: page.GetByTestId("header")
        // 3. INSIDE that header, find and assert:
        //    a. The logo (data-testid="header-logo") is visible
        //    b. The nav (data-testid="header-nav") is visible
        //    c. The search input (data-testid="header-search-input") is visible
        //    d. The cart link (data-testid="header-cart-link") is visible
        //
        // HINT: Chain like: header.GetByTestId("header-logo")
        //
        // EXPECTED: All four header elements are visible

        var page = await _fixture.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);
        
        var headerContainer = page.GetByTestId("header");
        var headerLogo = headerContainer.GetByTestId("header-logo");
        await Assertions.Expect(headerLogo).ToBeVisibleAsync();

        var headerNav = headerContainer.GetByTestId("header-nav");
        await Assertions.Expect(headerNav).ToBeVisibleAsync();

        var headerSearchInput = headerContainer.GetByTestId("header-search-input");
        await Assertions.Expect(headerSearchInput).ToBeVisibleAsync();

        var headerCartLink = headerContainer.GetByTestId("header-cart-link");
        await Assertions.Expect(headerCartLink).ToBeVisibleAsync();
    }
}
