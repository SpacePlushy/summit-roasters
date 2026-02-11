using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Web.ViewModels;

public class OrderDetailViewModel
{
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }

    // Shipping address (flat properties)
    public string ShippingFullName { get; set; } = string.Empty;
    public string ShippingAddressLine1 { get; set; } = string.Empty;
    public string? ShippingAddressLine2 { get; set; }
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingState { get; set; } = string.Empty;
    public string ShippingZipCode { get; set; } = string.Empty;
    public string ShippingCountry { get; set; } = string.Empty;

    // Order items
    public List<OrderItemViewModel> Items { get; set; } = new();

    // Totals
    public decimal Subtotal { get; set; }
    public decimal Shipping { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }

    // Aliases used by views
    public DateTime OrderDate => CreatedAt;
    public decimal ShippingCost => Shipping;
    public decimal DiscountAmount => Discount;

    // Shipping address object for views
    public OrderShippingAddressViewModel ShippingAddress => new()
    {
        FullName = ShippingFullName,
        Street = ShippingAddressLine1,
        Street2 = ShippingAddressLine2,
        City = ShippingCity,
        State = ShippingState,
        ZipCode = ShippingZipCode
    };
}

public class OrderShippingAddressViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string? Street2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}

public class OrderItemViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
    public string? Weight { get; set; }
    public string? Grind { get; set; }
    public int ProductId { get; set; }
    public string? ImageUrl { get; set; }

    // Alias used by views
    public string? GrindType => Grind;
}
