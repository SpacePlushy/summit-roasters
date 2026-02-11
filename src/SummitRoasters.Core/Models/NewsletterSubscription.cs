namespace SummitRoasters.Core.Models;

public class NewsletterSubscription
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
}
