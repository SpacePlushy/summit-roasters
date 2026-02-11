using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Web.ViewModels;

public class AdminProductListViewModel
{
    public List<AdminProductListItemViewModel> Products { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
    public int TotalCount { get; set; }
}

public class AdminProductListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }

    // Alias used by views
    public int StockQuantity => Stock;
}
