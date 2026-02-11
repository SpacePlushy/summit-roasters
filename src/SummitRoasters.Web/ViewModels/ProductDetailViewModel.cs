using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Web.ViewModels;

public class ProductDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? FullDescription { get; set; }
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public Category Category { get; set; }
    public RoastLevel? RoastLevel { get; set; }
    public string? Origin { get; set; }
    public string? Region { get; set; }
    public string? Altitude { get; set; }
    public string? Process { get; set; }
    public List<string> FlavorNotes { get; set; } = new();
    public string? ImageUrl { get; set; }
    public string StockStatus { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public bool IsFeatured { get; set; }
    public List<WeightOptionViewModel> WeightOptions { get; set; } = new();
    public List<string> GrindOptions { get; set; } = new();
    public ReviewSummaryViewModel ReviewSummary { get; set; } = new();
    public List<ReviewViewModel> Reviews { get; set; } = new();
    public List<ProductCardViewModel> RelatedProducts { get; set; } = new();
    public bool CanReview { get; set; }
    public bool HasReviewed { get; set; }

    // Convenience aliases delegating to ReviewSummary
    public double AverageRating => ReviewSummary.AverageRating;
    public int ReviewCount => ReviewSummary.TotalReviews;
    public Dictionary<int, int>? RatingDistribution => ReviewSummary.RatingBreakdown;

    // Alias for Process
    public string? ProcessMethod => Process;
}
