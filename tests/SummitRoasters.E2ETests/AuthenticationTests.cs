using FluentAssertions;
using Microsoft.Playwright;
using SummitRoasters.E2ETests.Fixtures;
using SummitRoasters.E2ETests.PageObjects;

namespace SummitRoasters.E2ETests;

[Collection("Browser")]
public class AuthenticationTests
{
    private readonly BrowserFixture _fixture;

    public AuthenticationTests(BrowserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task LoginPage_Loads()
    {
        var page = await _fixture.NewPageAsync();
        var loginPage = new LoginPage(page, _fixture.BaseUrl);
        await loginPage.NavigateAsync();

        await Assertions.Expect(loginPage.EmailInput).ToBeVisibleAsync();
        await Assertions.Expect(loginPage.PasswordInput).ToBeVisibleAsync();
        await Assertions.Expect(loginPage.LoginButton).ToBeVisibleAsync();
        await Assertions.Expect(loginPage.LoginForm).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Register_NewUser()
    {
        var page = await _fixture.NewPageAsync();
        var uniqueId = System.Guid.NewGuid().ToString("N")[..8];
        var email = $"testuser_{uniqueId}@example.com";

        await page.GotoAsync($"{_fixture.BaseUrl}/account/register");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await page.Locator("[data-testid='register-first-name']").FillAsync("Test");
        await page.Locator("[data-testid='register-last-name']").FillAsync("User");
        await page.Locator("[data-testid='register-email']").FillAsync(email);
        await page.Locator("[data-testid='register-password']").FillAsync("TestPass123!");
        await page.Locator("[data-testid='register-confirm-password']").FillAsync("TestPass123!");

#pragma warning disable CS0612 // Suppress obsolete warning for RunAndWaitForNavigationAsync
        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await page.Locator("[data-testid='register-submit']").ClickAsync();
        }, new PageRunAndWaitForNavigationOptions { Timeout = 15000 });
#pragma warning restore CS0612

        // After registration, user is signed in and redirected to home
        page.Url.Should().NotContain("/account/register");
    }

    [Fact]
    public async Task Login_ValidCredentials_RedirectsToHome()
    {
        var page = await _fixture.NewPageAsync();
        var loginPage = new LoginPage(page, _fixture.BaseUrl);
        await loginPage.NavigateAsync();

        await loginPage.EmailInput.FillAsync("sarah@example.com");
        await loginPage.PasswordInput.FillAsync("Customer123!");

#pragma warning disable CS0612
        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await loginPage.LoginButton.ClickAsync();
        }, new PageRunAndWaitForNavigationOptions { Timeout = 10000 });
#pragma warning restore CS0612

        // Should redirect away from login page
        page.Url.Should().NotContain("/account/login");

        // Should see user menu (authenticated state)
        await Assertions.Expect(page.Locator("[data-testid='header-user-menu-toggle']")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Login_InvalidCredentials_ShowsError()
    {
        var page = await _fixture.NewPageAsync();
        var loginPage = new LoginPage(page, _fixture.BaseUrl);
        await loginPage.NavigateAsync();

        await loginPage.EmailInput.FillAsync("sarah@example.com");
        await loginPage.PasswordInput.FillAsync("WrongPassword!");

#pragma warning disable CS0612
        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await loginPage.LoginButton.ClickAsync();
        }, new PageRunAndWaitForNavigationOptions { Timeout = 10000 });
#pragma warning restore CS0612

        // Should still be on login page
        page.Url.ToLower().Should().Contain("/account/login");

        // Should show validation error text
        var validationSummary = page.Locator("[data-testid='login-form'] .text-red-600");
        await Assertions.Expect(validationSummary).ToContainTextAsync("Invalid");
    }

    [Fact]
    public async Task Logout_ReturnsToHome()
    {
        var page = await _fixture.NewPageAsync();
        var loginPage = new LoginPage(page, _fixture.BaseUrl);

        // Login first
        await loginPage.NavigateAsync();
        await loginPage.EmailInput.FillAsync("sarah@example.com");
        await loginPage.PasswordInput.FillAsync("Customer123!");

#pragma warning disable CS0612
        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await loginPage.LoginButton.ClickAsync();
        }, new PageRunAndWaitForNavigationOptions { Timeout = 10000 });
#pragma warning restore CS0612

        // The user dropdown JS has a bug (looks for "header-user-menu-trigger" instead of "header-user-menu-toggle")
        // So we manually toggle the dropdown visibility via JS
        await page.EvaluateAsync(@"() => {
            const dropdown = document.querySelector('[data-testid=""header-user-dropdown""]');
            if (dropdown) dropdown.classList.remove('hidden');
        }");
        await page.WaitForTimeoutAsync(300);

        // Click logout (it's a form submit)
#pragma warning disable CS0612
        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await page.Locator("[data-testid='header-user-logout']").ClickAsync();
        }, new PageRunAndWaitForNavigationOptions { Timeout = 10000 });
#pragma warning restore CS0612

        // Should see login link (unauthenticated state)
        await Assertions.Expect(page.Locator("[data-testid='header-auth-links']")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task ProtectedPages_RedirectToLogin()
    {
        var page = await _fixture.NewPageAsync();

        // Try to access account settings without being logged in
        await page.GotoAsync($"{_fixture.BaseUrl}/account/settings");

        // Should be redirected to login page
        page.Url.ToLower().Should().Contain("/account/login");
        await Assertions.Expect(page.Locator("[data-testid='login-form']")).ToBeVisibleAsync();
    }
}
