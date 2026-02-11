namespace SummitRoasters.Web.ViewModels;

public class SearchResultsViewModel
{
    public string Query { get; set; } = string.Empty;
    public List<ProductCardViewModel> Results { get; set; } = new();
    public int TotalResults { get; set; }
    public PaginationViewModel? Pagination { get; set; }

    // Aliases used by views
    public List<ProductCardViewModel> Products => Results;
    public int TotalCount => TotalResults;
}
