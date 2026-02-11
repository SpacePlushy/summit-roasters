namespace SummitRoasters.Web.ViewModels;

public class CartItemViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSlug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string? Weight { get; set; }
    public string? Grind { get; set; }
    public decimal PriceAdjustment { get; set; }
    public decimal EffectivePrice => UnitPrice + PriceAdjustment;
    public decimal LineTotal => EffectivePrice * Quantity;

    // Aliases used by views
    public string Name => ProductName;
    public string? GrindType => Grind;
}
