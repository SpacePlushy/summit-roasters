namespace SummitRoasters.Core.DTOs;

public class AddToCartDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public string? Weight { get; set; }
    public string? Grind { get; set; }
}
