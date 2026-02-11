using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartService _cartService;
    private readonly IInventoryService _inventoryService;
    private readonly IPricingService _pricingService;

    public OrderService(
        IOrderRepository orderRepository,
        ICartService cartService,
        IInventoryService inventoryService,
        IPricingService pricingService)
    {
        _orderRepository = orderRepository;
        _cartService = cartService;
        _inventoryService = inventoryService;
        _pricingService = pricingService;
    }

    public async Task<Order> CreateFromCartAsync(string userId, ShippingAddressDto address, string? discountCode = null)
    {
        var cart = _cartService.GetCart();
        if (!cart.Items.Any())
            throw new InvalidOperationException("Cart is empty.");

        // Reserve stock for all items
        foreach (var item in cart.Items)
        {
            var reserved = await _inventoryService.ReserveStockAsync(item.ProductId, item.Quantity);
            if (!reserved)
                throw new InvalidOperationException($"Insufficient stock for {item.ProductName}.");
        }

        var subtotal = cart.Subtotal;
        var shipping = _pricingService.CalculateShipping(subtotal);
        var tax = _pricingService.CalculateTax(subtotal);
        var discountAmount = 0m;

        if (!string.IsNullOrWhiteSpace(discountCode))
        {
            var discountResult = await _pricingService.ApplyDiscountCodeAsync(discountCode, subtotal);
            if (discountResult.IsValid)
                discountAmount = discountResult.DiscountAmount;
        }

        var order = new Order
        {
            OrderNumber = GenerateOrderNumber(),
            UserId = userId,
            ShippingName = address.FullName,
            ShippingAddressLine1 = address.AddressLine1,
            ShippingAddressLine2 = address.AddressLine2,
            ShippingCity = address.City,
            ShippingState = address.State,
            ShippingZipCode = address.ZipCode,
            ShippingCountry = address.Country,
            ShippingPhone = address.Phone,
            Subtotal = subtotal,
            ShippingCost = shipping,
            Tax = tax,
            DiscountAmount = discountAmount,
            DiscountCode = discountCode,
            Total = subtotal + shipping + tax - discountAmount,
            Status = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Paid,
            Items = cart.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductSlug = i.ProductSlug,
                UnitPrice = i.EffectivePrice,
                Quantity = i.Quantity,
                Weight = i.Weight,
                Grind = i.Grind,
                LineTotal = i.LineTotal
            }).ToList()
        };

        await _orderRepository.AddAsync(order);
        _cartService.ClearCart();

        return order;
    }

    public string GenerateOrderNumber()
    {
        return $"SR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus newStatus)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null) return false;

        // Validate status transitions
        var validTransitions = new Dictionary<OrderStatus, OrderStatus[]>
        {
            { OrderStatus.Pending, new[] { OrderStatus.Processing, OrderStatus.Cancelled } },
            { OrderStatus.Processing, new[] { OrderStatus.Shipped, OrderStatus.Cancelled } },
            { OrderStatus.Shipped, new[] { OrderStatus.Delivered } },
            { OrderStatus.Delivered, Array.Empty<OrderStatus>() },
            { OrderStatus.Cancelled, Array.Empty<OrderStatus>() }
        };

        if (!validTransitions.TryGetValue(order.Status, out var allowed) || !allowed.Contains(newStatus))
            return false;

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;

        if (newStatus == OrderStatus.Cancelled)
        {
            // Release reserved stock
            foreach (var item in order.Items)
            {
                await _inventoryService.ReleaseStockAsync(item.ProductId, item.Quantity);
            }
            order.PaymentStatus = PaymentStatus.Refunded;
        }

        await _orderRepository.UpdateAsync(order);
        return true;
    }
}
