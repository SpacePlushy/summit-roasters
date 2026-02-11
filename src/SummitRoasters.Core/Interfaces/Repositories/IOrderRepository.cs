using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<Order?> GetByOrderNumberAsync(string orderNumber);
    Task<(List<Order> Orders, int TotalCount)> GetByUserIdAsync(string userId, int page = 1, int pageSize = 10);
    Task<List<Order>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task<int> GetTotalCountAsync();
    Task<decimal> GetTotalRevenueAsync();
    Task<bool> UserHasPurchasedProductAsync(string userId, int productId);
}
