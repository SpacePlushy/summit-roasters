using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Services;

public interface IInventoryService
{
    Task<InventoryStatus> CheckAvailabilityAsync(int productId, int requestedQuantity = 1);
    Task<bool> ReserveStockAsync(int productId, int quantity);
    Task ReleaseStockAsync(int productId, int quantity);
    Task<List<Product>> GetLowStockProductsAsync(int threshold = 5);
}
