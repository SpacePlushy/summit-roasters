using Microsoft.EntityFrameworkCore;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;
using SummitRoasters.Infrastructure.Data;

namespace SummitRoasters.Infrastructure.Repositories;

public class ShippingAddressRepository : IShippingAddressRepository
{
    private readonly ApplicationDbContext _context;

    public ShippingAddressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ShippingAddress>> GetByUserIdAsync(string userId)
    {
        return await _context.ShippingAddresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ToListAsync();
    }

    public async Task<ShippingAddress?> GetByIdAsync(int id)
    {
        return await _context.ShippingAddresses.FindAsync(id);
    }

    public async Task<ShippingAddress> AddAsync(ShippingAddress address)
    {
        _context.ShippingAddresses.Add(address);
        await _context.SaveChangesAsync();
        return address;
    }

    public async Task UpdateAsync(ShippingAddress address)
    {
        _context.ShippingAddresses.Update(address);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var address = await _context.ShippingAddresses.FindAsync(id);
        if (address != null)
        {
            _context.ShippingAddresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SetDefaultAsync(string userId, int addressId)
    {
        var addresses = await _context.ShippingAddresses
            .Where(a => a.UserId == userId)
            .ToListAsync();

        foreach (var address in addresses)
        {
            address.IsDefault = address.Id == addressId;
        }

        await _context.SaveChangesAsync();
    }
}
