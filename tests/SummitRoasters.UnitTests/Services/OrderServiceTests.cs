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

    // TODO: CreateFromCart_CreatesOrderWithCorrectTotals - verify that CreateFromCartAsync creates an order with correctly calculated subtotal, shipping, tax, and total using the pricing service

    // TODO: CreateFromCart_ThrowsWhenCartEmpty - verify that CreateFromCartAsync throws an InvalidOperationException when the cart has no items

    // TODO: CreateFromCart_ReservesStock - verify that CreateFromCartAsync calls ReserveStockAsync for each item in the cart

    // TODO: UpdateStatus_CancelledReleasesStock - verify that transitioning to Cancelled status calls ReleaseStockAsync for each order item

    // TODO: UpdateStatus_CancelledSetsRefunded - verify that transitioning to Cancelled status sets PaymentStatus to Refunded
}
