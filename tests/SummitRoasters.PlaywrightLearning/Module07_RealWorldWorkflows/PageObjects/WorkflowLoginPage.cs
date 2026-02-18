namespace SummitRoasters.PlaywrightLearning.Module07_RealWorldWorkflows.PageObjects;

using Microsoft.Playwright;
using SummitRoasters.PlaywrightLearning.Helpers;

/// <summary>
/// A complete page object for login workflows.
/// Used as helper infrastructure for Module 7 exercises.
///
/// This page object demonstrates a common pattern:
/// a "helper" page object with convenience methods like LoginAsCustomerAsync()
/// that multiple test files can reuse.
/// </summary>
public class WorkflowLoginPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public WorkflowLoginPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    // ============ LOCATORS ============

    public ILocator EmailInput => _page.GetByTestId("login-email");
    public ILocator PasswordInput => _page.GetByTestId("login-password");
    public ILocator SubmitButton => _page.GetByTestId("login-submit");
    public ILocator UserMenuToggle => _page.GetByTestId("header-user-menu-toggle");

    // ============ NAVIGATION ============

    public async Task NavigateAsync() =>
        await _page.GotoAsync($"{_baseUrl}/account/login");

    // ============ ACTIONS ============

    /// <summary>
    /// Login as a specific user and wait for the redirect to complete.
    /// </summary>
    public async Task LoginAsync(string email, string password)
    {
        await NavigateAsync();
        await EmailInput.FillAsync(email);
        await PasswordInput.FillAsync(password);
        await SubmitButton.ClickAsync();
        await _page.WaitForURLAsync(
            url => !url.Contains("/account/login"),
            new PageWaitForURLOptions { Timeout = 10000 });
    }

    /// <summary>Login as Sarah (customer account)</summary>
    public async Task LoginAsCustomerAsync() =>
        await LoginAsync(TestCredentials.CustomerSarahEmail, TestCredentials.CustomerSarahPassword);

    /// <summary>Login as admin</summary>
    public async Task LoginAsAdminAsync() =>
        await LoginAsync(TestCredentials.AdminEmail, TestCredentials.AdminPassword);
}
