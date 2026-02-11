using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Repositories;

public interface INewsletterRepository
{
    Task<NewsletterSubscription> AddAsync(NewsletterSubscription subscription);
    Task<bool> ExistsByEmailAsync(string email);
}
