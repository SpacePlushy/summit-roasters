using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Core.Services;

public class PricingService : IPricingService
{
    private readonly IDiscountCodeRepository _discountCodeRepository;
    private const decimal FreeShippingThreshold = 50m;
    private const decimal ShippingCost = 5.99m;
    private const decimal TaxRate = 0.08m;

    public PricingService(IDiscountCodeRepository discountCodeRepository)
    {
        _discountCodeRepository = discountCodeRepository;
    }

    public decimal CalculateLineTotal(decimal unitPrice, int quantity, decimal priceAdjustment = 0)
    {
        return (unitPrice + priceAdjustment) * quantity;
    }

    public async Task<DiscountResult> ApplyDiscountCodeAsync(string code, decimal subtotal)
    {
        var discount = await _discountCodeRepository.GetByCodeAsync(code.Trim().ToUpperInvariant());

        if (discount == null)
            return DiscountResult.Failure("Invalid discount code.");

        if (!discount.IsValid)
            return DiscountResult.Failure("This discount code has expired or is no longer available.");

        if (discount.MinimumOrderAmount.HasValue && subtotal < discount.MinimumOrderAmount.Value)
            return DiscountResult.Failure($"Minimum order of {discount.MinimumOrderAmount:C} required for this code.");

        var amount = discount.Type switch
        {
            DiscountType.Percentage => Math.Round(subtotal * (discount.Value / 100m), 2),
            DiscountType.FixedAmount => Math.Min(discount.Value, subtotal),
            DiscountType.FreeShipping => CalculateShipping(subtotal),
            _ => 0m
        };

        return DiscountResult.Success(amount, discount.Code);
    }

    public decimal CalculateShipping(decimal subtotal)
    {
        return subtotal >= FreeShippingThreshold ? 0m : ShippingCost;
    }

    public decimal CalculateTax(decimal subtotal)
    {
        return Math.Round(subtotal * TaxRate, 2);
    }
}
