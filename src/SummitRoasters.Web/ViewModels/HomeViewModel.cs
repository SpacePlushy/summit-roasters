namespace SummitRoasters.Web.ViewModels;

public class HomeViewModel
{
    public List<ProductCardViewModel> FeaturedProducts { get; set; } = new();
    public List<ProductCardViewModel> NewArrivals { get; set; } = new();
    public List<CategoryCardViewModel> CategoryCards { get; set; } = new();
}
