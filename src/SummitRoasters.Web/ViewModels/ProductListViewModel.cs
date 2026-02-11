namespace SummitRoasters.Web.ViewModels;

public class ProductListViewModel
{
    public List<ProductCardViewModel> Products { get; set; } = new();
    public ProductFilterViewModel Filter { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
    public int TotalCount { get; set; }
}
