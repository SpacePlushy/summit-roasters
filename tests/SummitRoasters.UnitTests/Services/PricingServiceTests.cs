using FluentAssertions;
using Moq;
using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Models.Enums;
using SummitRoasters.Core.Services;

namespace SummitRoasters.UnitTests.Services;

public class PricingServiceTests
{
    private readonly Mock<IDiscountCodeRepository> _discountCodeRepositoryMock;
    private readonly PricingService _pricingService;

    public PricingServiceTests()
    {
        _discountCodeRepositoryMock = new Mock<IDiscountCodeRepository>();
        _pricingService = new PricingService(_discountCodeRepositoryMock.Object);
    }

    [Fact]
    public void CalculateLineTotal_ReturnsCorrectTotal()
    {
        // Arrange
        var unitPrice = 14.99m;
        var quantity = 2;
        var priceAdjustment = 0m;

        // Act
        var result = _pricingService.CalculateLineTotal(unitPrice, quantity, priceAdjustment);

        // Assert
        result.Should().Be(29.98m);
    }

    [Fact]
    public void CalculateShipping_FreeWhenOverThreshold()
    {
        // Arrange
        var subtotal = 55m;

        // Act
        var result = _pricingService.CalculateShipping(subtotal);

        // Assert
        result.Should().Be(0m);
    }

    [Fact]
    public void CalculateShipping_ChargesWhenUnderThreshold()
    {
        // Arrange
        var subtotal = 30m;

        // Act
        var result = _pricingService.CalculateShipping(subtotal);

        // Assert
        result.Should().Be(5.99m);
    }

    // TODO: CalculateTax - verify that CalculateTax returns the correct rounded tax amount (subtotal * 0.08, rounded to 2 decimal places)

    // TODO: ApplyDiscountCode_ValidPercentage - verify that a valid percentage discount code returns the correct discount amount (e.g., 10% of subtotal)

    // TODO: ApplyDiscountCode_InvalidCode - verify that a non-existent discount code returns a failure result with an appropriate error message

    // TODO: ApplyDiscountCode_ExpiredCode - verify that an expired or inactive discount code returns a failure result indicating the code is no longer available

    // TODO: ApplyDiscountCode_MinimumNotMet - verify that a discount code with a minimum order amount returns a failure result when the subtotal is below the minimum
}
