namespace SummitRoasters.Web.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int LowStockCount { get; set; }
    public List<ProductCardViewModel> LowStockProducts { get; set; } = new();
    public int PendingOrders { get; set; }
}
