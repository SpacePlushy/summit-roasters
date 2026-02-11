using FluentAssertions;
using Moq;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Services;

namespace SummitRoasters.UnitTests.Services;

public class InventoryServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly InventoryService _inventoryService;

    public InventoryServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _inventoryService = new InventoryService(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task CheckAvailability_ReturnsAvailable_WhenStockSufficient()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Coffee", StockQuantity = 10 };
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _inventoryService.CheckAvailabilityAsync(1, 2);

        // Assert
        result.IsAvailable.Should().BeTrue();
        result.AvailableQuantity.Should().Be(10);
    }

    [Fact]
    public async Task CheckAvailability_ReturnsUnavailable_WhenStockInsufficient()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Coffee", StockQuantity = 1 };
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _inventoryService.CheckAvailabilityAsync(1, 5);

        // Assert
        result.IsAvailable.Should().BeFalse();
        result.AvailableQuantity.Should().Be(1);
    }

    [Fact]
    public async Task ReserveStock_DecrementsQuantity_WhenStockSufficient()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Coffee", StockQuantity = 10 };
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);
        _productRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Product>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _inventoryService.ReserveStockAsync(1, 3);

        // Assert
        result.Should().BeTrue();
        product.StockQuantity.Should().Be(7);
    }

    // TODO: ReserveStock_ReturnsFalse_WhenInsufficient - verify that ReserveStockAsync returns false when the requested quantity exceeds available stock and does not modify the product

    // TODO: ReleaseStock_IncrementsQuantity - verify that ReleaseStockAsync correctly increments the product's StockQuantity by the released amount

    // TODO: CheckAvailability_NotFound_ReturnsUnavailable - verify that CheckAvailabilityAsync returns IsAvailable=false and StockLevel="Not Found" when the product does not exist

    // TODO: GetLowStockProducts - verify that GetLowStockProductsAsync delegates to the repository and returns the correct list of low-stock products
}
