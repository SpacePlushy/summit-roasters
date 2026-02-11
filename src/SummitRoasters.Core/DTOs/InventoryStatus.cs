namespace SummitRoasters.Core.DTOs;

public class InventoryStatus
{
    public bool IsAvailable { get; set; }
    public int AvailableQuantity { get; set; }
    public string StockLevel { get; set; } = string.Empty;
}
