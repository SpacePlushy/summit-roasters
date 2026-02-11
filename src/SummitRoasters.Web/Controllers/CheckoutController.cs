using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;
using SummitRoasters.Web.ViewModels;

namespace SummitRoasters.Web.Controllers;

[Authorize]
public class CheckoutController : Controller
{
    private readonly ICartService _cartService;
    private readonly IPricingService _pricingService;
    private readonly IOrderService _orderService;
    private readonly IShippingAddressRepository _shippingAddressRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public CheckoutController(
        ICartService cartService,
        IPricingService pricingService,
        IOrderService orderService,
        IShippingAddressRepository shippingAddressRepository,
        IOrderRepository orderRepository,
        UserManager<ApplicationUser> userManager)
    {
        _cartService = cartService;
        _pricingService = pricingService;
        _orderService = orderService;
        _shippingAddressRepository = shippingAddressRepository;
        _orderRepository = orderRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var cart = _cartService.GetCart();
        if (cart.Items.Count == 0)
            return RedirectToAction("Index", "Cart");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var savedAddresses = await _shippingAddressRepository.GetByUserIdAsync(userId);

        var subtotal = cart.Subtotal;
        var shipping = _pricingService.CalculateShipping(subtotal);
        var tax = _pricingService.CalculateTax(subtotal);

        var viewModel = new CheckoutViewModel
        {
            Cart = new CartViewModel
            {
                Items = cart.Items.Select(i => new CartItemViewModel
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    ProductSlug = i.ProductSlug,
                    ImageUrl = i.ImageUrl,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    Weight = i.Weight,
                    Grind = i.Grind,
                    PriceAdjustment = i.PriceAdjustment
                }).ToList(),
                Subtotal = subtotal,
                Shipping = shipping,
                Tax = tax,
                Total = subtotal + shipping + tax,
                ItemCount = cart.TotalItems
            },
            SavedAddresses = savedAddresses.Select(a => new ShippingAddressFormViewModel
            {
                Id = a.Id,
                FullName = a.FullName,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                City = a.City,
                State = a.State,
                ZipCode = a.ZipCode,
                Country = a.Country,
                Phone = a.Phone,
                IsDefault = a.IsDefault
            }).ToList(),
            NewAddress = new ShippingAddressFormViewModel()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(ShippingAddressFormViewModel shipping, string? discountCode)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please provide a valid shipping address.";
            return RedirectToAction(nameof(Index));
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var addressDto = new ShippingAddressDto
        {
            Id = shipping.Id,
            FullName = shipping.FullName,
            AddressLine1 = shipping.AddressLine1,
            AddressLine2 = shipping.AddressLine2,
            City = shipping.City,
            State = shipping.State,
            ZipCode = shipping.ZipCode,
            Country = shipping.Country,
            Phone = shipping.Phone,
            IsDefault = shipping.IsDefault
        };

        try
        {
            var order = await _orderService.CreateFromCartAsync(userId, addressDto, discountCode);
            _cartService.ClearCart();
            return RedirectToAction(nameof(Confirmation), new { orderNumber = order.OrderNumber });
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Confirmation(string orderNumber)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order = await _orderRepository.GetByOrderNumberAsync(orderNumber);

        if (order == null || order.UserId != userId)
            return NotFound();

        var viewModel = new OrderConfirmationViewModel
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            Total = order.Total,
            ItemCount = order.Items.Count,
            ShippingName = order.ShippingName,
            EstimatedDelivery = DateTime.UtcNow.AddDays(5).ToString("MMMM d, yyyy"),
            ShippingAddress = new OrderShippingAddressViewModel
            {
                FullName = order.ShippingName,
                Street = order.ShippingAddressLine1,
                Street2 = order.ShippingAddressLine2,
                City = order.ShippingCity,
                State = order.ShippingState,
                ZipCode = order.ShippingZipCode
            }
        };

        return View(viewModel);
    }
}
