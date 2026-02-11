namespace SummitRoasters.Core.DTOs;

public class DiscountResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? DiscountCode { get; set; }

    public static DiscountResult Success(decimal amount, string code) => new()
    {
        IsValid = true,
        DiscountAmount = amount,
        DiscountCode = code
    };

    public static DiscountResult Failure(string message) => new()
    {
        IsValid = false,
        ErrorMessage = message
    };
}
