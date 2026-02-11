using Microsoft.EntityFrameworkCore;
using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Models.Enums;
using SummitRoasters.Infrastructure.Data;

namespace SummitRoasters.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.WeightOptions)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetBySlugAsync(string slug)
    {
        return await _context.Products
            .Include(p => p.WeightOptions)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<(List<Product> Products, int TotalCount)> GetFilteredAsync(ProductFilterDto filter)
    {
        var query = _context.Products
            .Include(p => p.Reviews)
            .Include(p => p.WeightOptions)
            .Where(p => p.IsActive)
            .AsQueryable();

        if (filter.Category.HasValue)
            query = query.Where(p => p.Category == filter.Category.Value);

        if (filter.RoastLevel.HasValue)
            query = query.Where(p => p.RoastLevel == filter.RoastLevel.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);

        if (filter.MinRating.HasValue)
            query = query.Where(p => p.Reviews.Any() && p.Reviews.Average(r => r.Rating) >= filter.MinRating.Value);

        var totalCount = await query.CountAsync();

        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "price-asc" => query.OrderBy(p => p.Price),
            "price-desc" => query.OrderByDescending(p => p.Price),
            "name" => query.OrderBy(p => p.Name),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            "rating" => query.OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0),
            _ => query.OrderByDescending(p => p.IsFeatured).ThenByDescending(p => p.CreatedAt)
        };

        var products = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<List<Product>> GetFeaturedAsync(int count = 6)
    {
        return await _context.Products
            .Include(p => p.Reviews)
            .Where(p => p.IsActive && p.IsFeatured)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<Product>> GetNewArrivalsAsync(int count = 6)
    {
        return await _context.Products
            .Include(p => p.Reviews)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<Product>> SearchAsync(string query, int maxResults = 20)
    {
        var lowerQuery = $"%{query.ToLowerInvariant()}%";
        return await _context.Products
            .Include(p => p.Reviews)
            .Where(p => p.IsActive && (
                EF.Functions.Like(p.Name.ToLower(), lowerQuery) ||
                EF.Functions.Like(p.Description.ToLower(), lowerQuery) ||
                (p.Origin != null && EF.Functions.Like(p.Origin.ToLower(), lowerQuery)) ||
                (p.FlavorNotes != null && EF.Functions.Like(p.FlavorNotes.ToLower(), lowerQuery))
            ))
            .OrderByDescending(p => p.IsFeatured)
            .Take(maxResults)
            .ToListAsync();
    }

    public async Task<List<Product>> GetRelatedAsync(int productId, int count = 4)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) return new List<Product>();

        return await _context.Products
            .Include(p => p.Reviews)
            .Where(p => p.IsActive && p.Id != productId && p.Category == product.Category)
            .OrderByDescending(p => p.IsFeatured)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetLowStockAsync(int threshold = 5)
    {
        return await _context.Products
            .Where(p => p.IsActive && p.StockQuantity <= threshold && p.StockQuantity > 0)
            .OrderBy(p => p.StockQuantity)
            .ToListAsync();
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Reviews)
            .Include(p => p.WeightOptions)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}
