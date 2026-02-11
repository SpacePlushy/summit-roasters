using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetBySlugAsync(string slug);
    Task<(List<Product> Products, int TotalCount)> GetFilteredAsync(ProductFilterDto filter);
    Task<List<Product>> GetFeaturedAsync(int count = 6);
    Task<List<Product>> GetNewArrivalsAsync(int count = 6);
    Task<List<Product>> SearchAsync(string query, int maxResults = 20);
    Task<List<Product>> GetRelatedAsync(int productId, int count = 4);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task<List<Product>> GetLowStockAsync(int threshold = 5);
    Task<List<Product>> GetAllAsync();
}
