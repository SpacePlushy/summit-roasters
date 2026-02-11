namespace SummitRoasters.Web.ViewModels;

public class CategoryCardViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public int ProductCount { get; set; }
}
