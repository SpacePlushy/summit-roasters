namespace SummitRoasters.E2ETests.PageObjects;

using Microsoft.Playwright;

public class HomePage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public HomePage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    public IPage Page => _page;
    public async Task NavigateAsync() => await _page.GotoAsync(_baseUrl);
    public ILocator HeroSection => _page.Locator("[data-testid='hero-section']");
    public ILocator HeroHeading => _page.Locator("[data-testid='hero-heading']");
    public ILocator FeaturedProductsSection => _page.Locator("[data-testid='featured-products-section']");
    public ILocator FeaturedProductsGrid => _page.Locator("[data-testid='featured-products-grid']");
    public ILocator CategoriesSection => _page.Locator("[data-testid='categories-section']");
    public ILocator NewArrivalsSection => _page.Locator("[data-testid='new-arrivals-section']");
    public ILocator NewsletterSection => _page.Locator("[data-testid='newsletter-section']");
    public ILocator Header => _page.Locator("[data-testid='header']");
    public ILocator HeaderNav => _page.Locator("[data-testid='header-nav']");
    public ILocator Footer => _page.Locator("[data-testid='footer']");
    public ILocator MobileMenuToggle => _page.Locator("[data-testid='header-mobile-menu-toggle']");
    public ILocator MobileMenu => _page.Locator("[data-testid='mobile-menu']");
    public ILocator MobileMenuClose => _page.Locator("[data-testid='mobile-menu-close']");
    public ILocator HeaderCartLink => _page.Locator("[data-testid='header-cart-link']");
    public ILocator HeaderSearchInput => _page.Locator("[data-testid='header-search-input']");
}
