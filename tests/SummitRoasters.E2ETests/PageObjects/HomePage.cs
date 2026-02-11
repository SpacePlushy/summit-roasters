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

    public async Task NavigateAsync() => await _page.GotoAsync(_baseUrl);
    public ILocator HeroBanner => _page.Locator("[data-testid='hero-banner']");
    public ILocator FeaturedProducts => _page.Locator("[data-testid='featured-products']");
    public ILocator ProductCards => _page.Locator("[data-testid='product-card']");
    public ILocator CategoryCards => _page.Locator("[data-testid='category-card']");
    public ILocator NewArrivals => _page.Locator("[data-testid='new-arrivals']");
    public ILocator NewsletterForm => _page.Locator("[data-testid='newsletter-form']");
    public ILocator NavLinks => _page.Locator("nav a");
}
