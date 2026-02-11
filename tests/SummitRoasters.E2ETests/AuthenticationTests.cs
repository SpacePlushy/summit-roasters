using SummitRoasters.E2ETests.PageObjects;

namespace SummitRoasters.E2ETests;

public class AuthenticationTests
{
    private const string BaseUrl = "https://localhost:5001";

    // TODO: Set up Playwright browser in test fixture
    // private IPlaywright _playwright;
    // private IBrowser _browser;
    // private IPage _page;

    [Fact(Skip = "E2E tests require running application")]
    public async Task LoginPage_Loads()
    {
        // var page = ... // create page from browser
        // var loginPage = new LoginPage(page, BaseUrl);
        // await loginPage.NavigateAsync();
        // await Expect(loginPage.EmailInput).ToBeVisibleAsync();
        // await Expect(loginPage.PasswordInput).ToBeVisibleAsync();
        // await Expect(loginPage.LoginButton).ToBeVisibleAsync();
        await Task.CompletedTask;
    }

    // TODO: Register_NewUser - filling out registration form creates a new account
    // TODO: Login_ValidCredentials - logging in with valid credentials redirects to home
    // TODO: Login_InvalidCredentials - logging in with wrong password shows error message
    // TODO: Logout - clicking logout returns to home page and clears session
    // TODO: ProtectedPages_RedirectToLogin - accessing account pages without auth redirects to login
}
