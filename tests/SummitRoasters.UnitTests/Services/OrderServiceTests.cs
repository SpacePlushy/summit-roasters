using FluentAssertions;
using Moq;
using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Models.Enums;
using SummitRoasters.Core.Services;

namespace SummitRoasters.UnitTests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ICartService> _cartServiceMock;
    private readonly Mock<IInventoryService> _inventoryServiceMock;
    private readonly Mock<IPricingService> _pricingServiceMock;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _cartServiceMock = new Mock<ICartService>();
        _inventoryServiceMock = new Mock<IInventoryService>();
        _pricingServiceMock = new Mock<IPricingService>();

        _orderService = new OrderService(
            _orderRepositoryMock.Object,
            _cartServiceMock.Object,
            _inventoryServiceMock.Object,
            _pricingServiceMock.Object);
    }

    [Fact]
    public void GenerateOrderNumber_HasCorrectFormat()
    {
        // Act
        var orderNumber = _orderService.GenerateOrderNumber();

        // Assert
        orderNumber.Should().StartWith("SR-");
        orderNumber.Should().Contain(DateTime.UtcNow.ToString("yyyyMMdd"));
        // Format: SR-YYYYMMDD-XXXXXXXX (8-char guid segment)
        var parts = orderNumber.Split('-');
        parts.Should().HaveCount(3);
        parts[2].Should().HaveLength(8);
    }

    [Fact]
    public async Task UpdateStatus_AllowsValidTransition()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            OrderNumber = "SR-20260101-ABCD1234",
            Status = OrderStatus.Pending,
            Items = new List<OrderItem>()
        };
        _orderRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(order);
        _orderRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _orderService.UpdateStatusAsync(1, OrderStatus.Processing);

        // Assert
        result.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Processing);
    }

    [Fact]
    public async Task UpdateStatus_RejectsInvalidTransition()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            OrderNumber = "SR-20260101-ABCD1234",
            Status = OrderStatus.Delivered,
            Items = new List<OrderItem>()
        };
        _orderRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(order);

        // Act
        var result = await _orderService.UpdateStatusAsync(1, OrderStatus.Pending);

        // Assert
        result.Should().BeFalse();
        order.Status.Should().Be(OrderStatus.Delivered);
    }

    [Fact]
    public async Task CreateFromCart_CreatesOrderWithCorrectTotals()
    {
        // Arrange
        var cart = new Cart();
        cart.Items.Add(new CartItem
        {
            ProductId = 1,
            ProductName = "Summit Blend",
            ProductSlug = "summit-blend",
            UnitPrice = 16.99m,
            Quantity = 2
        });
        _cartServiceMock.Setup(s => s.GetCart()).Returns(cart);

        _inventoryServiceMock
            .Setup(s => s.ReserveStockAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        _pricingServiceMock.Setup(s => s.CalculateShipping(33.98m)).Returns(5.99m);
        _pricingServiceMock.Setup(s => s.CalculateTax(33.98m)).Returns(2.72m);

        _orderRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => o);

        var address = new ShippingAddressDto
        {
            FullName = "Test User",
            AddressLine1 = "123 Test St",
            City = "Denver",
            State = "CO",
            ZipCode = "80202"
        };

        // Act
        var result = await _orderService.CreateFromCartAsync("user-1", address);

        // Assert
        result.Subtotal.Should().Be(33.98m);
        result.ShippingCost.Should().Be(5.99m);
        result.Tax.Should().Be(2.72m);
        result.Total.Should().Be(33.98m + 5.99m + 2.72m);
    }

    [Fact]
    public async Task CreateFromCart_ThrowsWhenCartEmpty()
    {
        // Arrange
        var emptyCart = new Cart();
        _cartServiceMock.Setup(s => s.GetCart()).Returns(emptyCart);

        var address = new ShippingAddressDto
        {
            FullName = "Test User",
            AddressLine1 = "123 Test St",
            City = "Denver",
            State = "CO",
            ZipCode = "80202"
        };

        // Act
        var act = () => _orderService.CreateFromCartAsync("user-1", address);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*empty*");
    }

    [Fact]
    public async Task CreateFromCart_ReservesStock()
    {
        // Arrange
        var cart = new Cart();
        cart.Items.Add(new CartItem { ProductId = 1, ProductName = "A", ProductSlug = "a", UnitPrice = 10m, Quantity = 2 });
        cart.Items.Add(new CartItem { ProductId = 2, ProductName = "B", ProductSlug = "b", UnitPrice = 15m, Quantity = 1 });
        _cartServiceMock.Setup(s => s.GetCart()).Returns(cart);

        _inventoryServiceMock
            .Setup(s => s.ReserveStockAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        _pricingServiceMock.Setup(s => s.CalculateShipping(It.IsAny<decimal>())).Returns(0m);
        _pricingServiceMock.Setup(s => s.CalculateTax(It.IsAny<decimal>())).Returns(0m);

        _orderRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => o);

        var address = new ShippingAddressDto
        {
            FullName = "Test User",
            AddressLine1 = "123 Test St",
            City = "Denver",
            State = "CO",
            ZipCode = "80202"
        };

        // Act
        await _orderService.CreateFromCartAsync("user-1", address);

        // Assert
        _inventoryServiceMock.Verify(s => s.ReserveStockAsync(1, 2), Times.Once);
        _inventoryServiceMock.Verify(s => s.ReserveStockAsync(2, 1), Times.Once);
    }

    [Fact]
    public async Task UpdateStatus_CancelledReleasesStock()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            OrderNumber = "SR-20260101-ABCD1234",
            Status = OrderStatus.Pending,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, Quantity = 2 },
                new OrderItem { ProductId = 2, Quantity = 3 }
            }
        };
        _orderRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);
        _orderRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
        _inventoryServiceMock.Setup(s => s.ReleaseStockAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

        // Act
        await _orderService.UpdateStatusAsync(1, OrderStatus.Cancelled);

        // Assert
        _inventoryServiceMock.Verify(s => s.ReleaseStockAsync(1, 2), Times.Once);
        _inventoryServiceMock.Verify(s => s.ReleaseStockAsync(2, 3), Times.Once);
    }

    [Fact]
    public async Task UpdateStatus_CancelledSetsRefunded()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            OrderNumber = "SR-20260101-ABCD1234",
            Status = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Paid,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, Quantity = 1 }
            }
        };
        _orderRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);
        _orderRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
        _inventoryServiceMock.Setup(s => s.ReleaseStockAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

        // Act
        await _orderService.UpdateStatusAsync(1, OrderStatus.Cancelled);

        // Assert
        order.PaymentStatus.Should().Be(PaymentStatus.Refunded);
    }
}
