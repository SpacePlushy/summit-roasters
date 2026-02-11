using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Services;

public class InventoryService : IInventoryService
{
    private readonly IProductRepository _productRepository;

    public InventoryService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<InventoryStatus> CheckAvailabilityAsync(int productId, int requestedQuantity = 1)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            return new InventoryStatus
            {
                IsAvailable = false,
                AvailableQuantity = 0,
                StockLevel = "Not Found"
            };
        }

        return new InventoryStatus
        {
            IsAvailable = product.StockQuantity >= requestedQuantity,
            AvailableQuantity = product.StockQuantity,
            StockLevel = product.StockStatus
        };
    }

    public async Task<bool> ReserveStockAsync(int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null || product.StockQuantity < quantity)
            return false;

        product.StockQuantity -= quantity;
        product.UpdatedAt = DateTime.UtcNow;
        await _productRepository.UpdateAsync(product);
        return true;
    }

    public async Task ReleaseStockAsync(int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return;

        product.StockQuantity += quantity;
        product.UpdatedAt = DateTime.UtcNow;
        await _productRepository.UpdateAsync(product);
    }

    public async Task<List<Product>> GetLowStockProductsAsync(int threshold = 5)
    {
        return await _productRepository.GetLowStockAsync(threshold);
    }
}
