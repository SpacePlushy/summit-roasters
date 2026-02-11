using Microsoft.EntityFrameworkCore;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;
using SummitRoasters.Infrastructure.Data;

namespace SummitRoasters.Infrastructure.Repositories;

public class NewsletterRepository : INewsletterRepository
{
    private readonly ApplicationDbContext _context;

    public NewsletterRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<NewsletterSubscription> AddAsync(NewsletterSubscription subscription)
    {
        _context.NewsletterSubscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.NewsletterSubscriptions
            .AnyAsync(n => n.Email.ToLower() == email.ToLower());
    }
}
