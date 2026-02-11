using Microsoft.EntityFrameworkCore;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;
using SummitRoasters.Infrastructure.Data;

namespace SummitRoasters.Infrastructure.Repositories;

public class DiscountCodeRepository : IDiscountCodeRepository
{
    private readonly ApplicationDbContext _context;

    public DiscountCodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DiscountCode?> GetByCodeAsync(string code)
    {
        return await _context.DiscountCodes
            .FirstOrDefaultAsync(d => d.Code == code);
    }

    public async Task IncrementUsageAsync(int id)
    {
        var code = await _context.DiscountCodes.FindAsync(id);
        if (code != null)
        {
            code.CurrentUses++;
            await _context.SaveChangesAsync();
        }
    }
}
