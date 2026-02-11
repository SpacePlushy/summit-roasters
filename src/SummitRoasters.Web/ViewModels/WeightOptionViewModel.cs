namespace SummitRoasters.Web.ViewModels;

public class WeightOptionViewModel
{
    public string Weight { get; set; } = string.Empty;
    public decimal PriceAdjustment { get; set; }
    public bool IsDefault { get; set; }
}
