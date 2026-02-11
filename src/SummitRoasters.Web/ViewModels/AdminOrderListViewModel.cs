using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Web.ViewModels;

public class AdminOrderListViewModel
{
    public List<AdminOrderListItemViewModel> Orders { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
    public int TotalCount { get; set; }
}

public class AdminOrderListItemViewModel
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public decimal Total { get; set; }
    public int ItemCount { get; set; }

    // Alias used by views
    public DateTime OrderDate => CreatedAt;
}
