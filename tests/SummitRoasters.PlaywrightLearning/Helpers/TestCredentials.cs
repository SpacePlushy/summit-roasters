namespace SummitRoasters.PlaywrightLearning.Helpers;

/// <summary>
/// Centralized test credentials from Summit Roasters seed data.
/// NEVER put real credentials in test code — these are dev-only seed accounts.
/// </summary>
public static class TestCredentials
{
    public const string AdminEmail = "admin@summitroasters.com";
    public const string AdminPassword = "Admin123!";

    public const string CustomerSarahEmail = "sarah@example.com";
    public const string CustomerSarahPassword = "Customer123!";

    public const string CustomerMikeEmail = "mike@example.com";
    public const string CustomerMikePassword = "Customer123!";

    public const string DiscountWelcome10 = "WELCOME10";
    public const string DiscountFiveBucks = "FIVEBUCKS";
    public const string DiscountFreeShip = "FREESHIP";
}
