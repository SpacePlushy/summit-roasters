using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Core.Interfaces.Services;

public interface IOrderService
{
    Task<Order> CreateFromCartAsync(string userId, ShippingAddressDto address, string? discountCode = null);
    string GenerateOrderNumber();
    Task<bool> UpdateStatusAsync(int orderId, OrderStatus newStatus);
}
