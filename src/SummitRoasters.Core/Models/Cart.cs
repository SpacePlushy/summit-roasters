namespace SummitRoasters.Core.Models;

public class Cart
{
    public List<CartItem> Items { get; set; } = new();

    public decimal Subtotal => Items.Sum(i => i.LineTotal);
    public int TotalItems => Items.Sum(i => i.Quantity);
}

public class CartItem
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
}
