using SummitRoasters.Core.DTOs;

namespace SummitRoasters.Core.Interfaces.Services;

public interface IPricingService
{
    decimal CalculateLineTotal(decimal unitPrice, int quantity, decimal priceAdjustment = 0);
    Task<DiscountResult> ApplyDiscountCodeAsync(string code, decimal subtotal);
    decimal CalculateShipping(decimal subtotal);
    decimal CalculateTax(decimal subtotal);
}
