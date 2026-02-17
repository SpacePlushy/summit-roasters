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

    [Fact]
    public void CalculateTax_ReturnsCorrectRoundedAmount()
    {
        // Arrange
        var subtotal = 33.33m;

        // Act
        var result = _pricingService.CalculateTax(subtotal);

        // Assert
        result.Should().Be(2.67m); // 33.33 * 0.08 = 2.6664, rounded to 2.67
    }

    [Fact]
    public async Task ApplyDiscountCode_ValidPercentage_ReturnsCorrectDiscount()
    {
        // Arrange
        var discount = new DiscountCode
        {
            Id = 1,
            Code = "WELCOME10",
            Type = DiscountType.Percentage,
            Value = 10m,
            IsActive = true,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };
        _discountCodeRepositoryMock
            .Setup(r => r.GetByCodeAsync("WELCOME10"))
            .ReturnsAsync(discount);

        // Act
        var result = await _pricingService.ApplyDiscountCodeAsync("WELCOME10", 100m);

        // Assert
        result.IsValid.Should().BeTrue();
        result.DiscountAmount.Should().Be(10m); // 10% of 100
        result.DiscountCode.Should().Be("WELCOME10");
    }

    [Fact]
    public async Task ApplyDiscountCode_InvalidCode_ReturnsFailure()
    {
        // Arrange
        _discountCodeRepositoryMock
            .Setup(r => r.GetByCodeAsync("BADCODE"))
            .ReturnsAsync((DiscountCode?)null);

        // Act
        var result = await _pricingService.ApplyDiscountCodeAsync("BADCODE", 50m);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Invalid discount code");
    }

    [Fact]
    public async Task ApplyDiscountCode_ExpiredCode_ReturnsFailure()
    {
        // Arrange
        var discount = new DiscountCode
        {
            Id = 2,
            Code = "EXPIRED",
            Type = DiscountType.Percentage,
            Value = 15m,
            IsActive = true,
            ExpiresAt = DateTime.UtcNow.AddDays(-1) // expired yesterday
        };
        _discountCodeRepositoryMock
            .Setup(r => r.GetByCodeAsync("EXPIRED"))
            .ReturnsAsync(discount);

        // Act
        var result = await _pricingService.ApplyDiscountCodeAsync("EXPIRED", 50m);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Contain("expired");
    }

    [Fact]
    public async Task ApplyDiscountCode_MinimumNotMet_ReturnsFailure()
    {
        // Arrange
        var discount = new DiscountCode
        {
            Id = 3,
            Code = "BIGORDER",
            Type = DiscountType.Percentage,
            Value = 20m,
            IsActive = true,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            MinimumOrderAmount = 75m
        };
        _discountCodeRepositoryMock
            .Setup(r => r.GetByCodeAsync("BIGORDER"))
            .ReturnsAsync(discount);

        // Act
        var result = await _pricingService.ApplyDiscountCodeAsync("BIGORDER", 30m);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Minimum order");
    }
}
