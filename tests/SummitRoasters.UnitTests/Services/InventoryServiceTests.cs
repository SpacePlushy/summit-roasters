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

    [Fact]
    public async Task ReserveStock_ReturnsFalse_WhenInsufficient()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Coffee", StockQuantity = 2 };
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _inventoryService.ReserveStockAsync(1, 5);

        // Assert
        result.Should().BeFalse();
        product.StockQuantity.Should().Be(2); // unchanged
        _productRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task ReleaseStock_IncrementsQuantity()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Coffee", StockQuantity = 5 };
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);
        _productRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Product>()))
            .Returns(Task.CompletedTask);

        // Act
        await _inventoryService.ReleaseStockAsync(1, 3);

        // Assert
        product.StockQuantity.Should().Be(8);
        _productRepositoryMock.Verify(r => r.UpdateAsync(product), Times.Once);
    }

    [Fact]
    public async Task CheckAvailability_NotFound_ReturnsUnavailable()
    {
        // Arrange
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _inventoryService.CheckAvailabilityAsync(999);

        // Assert
        result.IsAvailable.Should().BeFalse();
        result.AvailableQuantity.Should().Be(0);
        result.StockLevel.Should().Be("Not Found");
    }

    [Fact]
    public async Task GetLowStockProducts_DelegatesToRepository()
    {
        // Arrange
        var lowStockProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Low Stock Coffee", StockQuantity = 2 },
            new Product { Id = 2, Name = "Almost Out", StockQuantity = 1 }
        };
        _productRepositoryMock
            .Setup(r => r.GetLowStockAsync(5))
            .ReturnsAsync(lowStockProducts);

        // Act
        var result = await _inventoryService.GetLowStockProductsAsync(5);

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Low Stock Coffee");
        _productRepositoryMock.Verify(r => r.GetLowStockAsync(5), Times.Once);
    }
}
