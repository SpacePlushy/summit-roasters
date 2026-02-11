namespace SummitRoasters.Web.ViewModels;

public class OrderConfirmationViewModel
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public int ItemCount { get; set; }
    public string ShippingName { get; set; } = string.Empty;
    public string? EstimatedDelivery { get; set; }

    // Shipping address object for views
    public OrderShippingAddressViewModel? ShippingAddress { get; set; }
}
