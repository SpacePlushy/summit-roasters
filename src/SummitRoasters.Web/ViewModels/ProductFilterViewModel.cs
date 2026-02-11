using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Web.ViewModels;

public class ProductFilterViewModel
{
    public Category? SelectedCategory { get; set; }
    public RoastLevel? SelectedRoastLevel { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinRating { get; set; }
    public string? SortBy { get; set; }

    // Filter option lists for sidebar
    public List<FilterOptionViewModel>? Categories { get; set; }
    public List<FilterOptionViewModel>? RoastLevels { get; set; }

    public int ActiveFilterCount
    {
        get
        {
            int count = 0;
            if (SelectedCategory.HasValue) count++;
            if (SelectedRoastLevel.HasValue) count++;
            if (MinPrice.HasValue) count++;
            if (MaxPrice.HasValue) count++;
            if (MinRating.HasValue) count++;
            return count;
        }
    }
}

public class FilterOptionViewModel
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
    public int Count { get; set; }
}
