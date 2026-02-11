namespace SummitRoasters.Core.Models;

public class WeightOption
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Weight { get; set; } = string.Empty;
    public decimal PriceAdjustment { get; set; }
    public bool IsDefault { get; set; }

    public Product Product { get; set; } = null!;
}
