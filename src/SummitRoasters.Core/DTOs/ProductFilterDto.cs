using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Core.DTOs;

public class ProductFilterDto
{
    public Category? Category { get; set; }
    public RoastLevel? RoastLevel { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinRating { get; set; }
    public string? SortBy { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
