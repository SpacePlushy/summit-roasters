using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Core.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public Category Category { get; set; }
    public RoastLevel? RoastLevel { get; set; }
    public string? Origin { get; set; }
    public string? Region { get; set; }
    public string? Altitude { get; set; }
    public string? Process { get; set; }
    public string? FlavorNotes { get; set; }
    public string? ImageUrl { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<WeightOption> WeightOptions { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();

    public string StockStatus => StockQuantity switch
    {
        <= 0 => "Out of Stock",
        <= 5 => "Low Stock",
        _ => "In Stock"
    };

    public double AverageRating => Reviews.Count > 0
        ? Math.Round(Reviews.Average(r => r.Rating), 1)
        : 0;

    public int ReviewCount => Reviews.Count;
}
