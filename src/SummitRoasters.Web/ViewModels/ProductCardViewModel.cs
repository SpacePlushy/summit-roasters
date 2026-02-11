using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Web.ViewModels;

public class ProductCardViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public string? ImageUrl { get; set; }
    public Category Category { get; set; }
    public RoastLevel? RoastLevel { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public string StockStatus { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public bool IsFeatured { get; set; }
}
