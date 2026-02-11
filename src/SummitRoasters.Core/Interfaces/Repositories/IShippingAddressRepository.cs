using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Repositories;

public interface IShippingAddressRepository
{
    Task<List<ShippingAddress>> GetByUserIdAsync(string userId);
    Task<ShippingAddress?> GetByIdAsync(int id);
    Task<ShippingAddress> AddAsync(ShippingAddress address);
    Task UpdateAsync(ShippingAddress address);
    Task DeleteAsync(int id);
    Task SetDefaultAsync(string userId, int addressId);
}
