namespace SummitRoasters.E2ETests.PageObjects;

using Microsoft.Playwright;

public class LoginPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public LoginPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    public IPage Page => _page;
    public async Task NavigateAsync() => await _page.GotoAsync($"{_baseUrl}/account/login");
    public ILocator LoginForm => _page.Locator("[data-testid='login-form']");
    public ILocator EmailInput => _page.Locator("[data-testid='login-email']");
    public ILocator PasswordInput => _page.Locator("[data-testid='login-password']");
    public ILocator LoginButton => _page.Locator("[data-testid='login-submit']");
    public ILocator RegisterLink => _page.Locator("[data-testid='login-register-link']");
    public ILocator ErrorMessage => _page.Locator(".text-red-600");
    public ILocator UserMenuToggle => _page.Locator("[data-testid='header-user-menu-toggle']");
    public ILocator LogoutButton => _page.Locator("[data-testid='header-user-logout']");

    public async Task LoginAsync(string email, string password)
    {
        await EmailInput.FillAsync(email);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }
}
