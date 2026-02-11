namespace SummitRoasters.Web.ViewModels;

public class CheckoutViewModel
{
    public CartViewModel Cart { get; set; } = new();
    public List<ShippingAddressFormViewModel> SavedAddresses { get; set; } = new();
    public ShippingAddressFormViewModel NewAddress { get; set; } = new();

    // Convenience properties delegating to Cart
    public List<CartItemViewModel> Items => Cart.Items;
    public decimal Subtotal => Cart.Subtotal;
    public decimal ShippingCost => Cart.Shipping;
    public decimal Tax => Cart.Tax;
    public decimal Total => Cart.Total;
    public decimal DiscountAmount { get; set; }

    // Shipping address for form binding
    public ShippingAddressFormViewModel? ShippingAddress { get; set; }
}
