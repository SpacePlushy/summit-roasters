namespace SummitRoasters.Web.ViewModels;

public class OrderHistoryViewModel
{
    public List<OrderSummaryViewModel> Orders { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
}

public class OrderSummaryViewModel
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ItemCount { get; set; }

    // Alias used by views
    public DateTime OrderDate => Date;
}
