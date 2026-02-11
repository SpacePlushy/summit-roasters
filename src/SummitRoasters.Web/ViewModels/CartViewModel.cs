namespace SummitRoasters.Web.ViewModels;

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Shipping { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public int ItemCount { get; set; }
    public bool IsEmpty => ItemCount == 0;

    // Alias used by views
    public decimal ShippingCost => Shipping;
}
