using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Web.ViewModels;

namespace SummitRoasters.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IPricingService _pricingService;

    public CartController(ICartService cartService, IPricingService pricingService)
    {
        _cartService = cartService;
        _pricingService = pricingService;
    }

    public IActionResult Index()
    {
        var cart = _cartService.GetCart();
        var subtotal = cart.Subtotal;
        var shipping = _pricingService.CalculateShipping(subtotal);
        var tax = _pricingService.CalculateTax(subtotal);
        var total = subtotal + shipping + tax;

        var viewModel = new CartViewModel
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
            Total = total,
            ItemCount = cart.TotalItems
        };

        return View(viewModel);
    }
}
