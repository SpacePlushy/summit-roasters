using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Models.Enums;
using SummitRoasters.Web.ViewModels;

namespace SummitRoasters.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderService _orderService;
    private readonly IInventoryService _inventoryService;

    public AdminController(
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IOrderService orderService,
        IInventoryService inventoryService)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _orderService = orderService;
        _inventoryService = inventoryService;
    }

    public async Task<IActionResult> Index()
    {
        var allProducts = await _productRepository.GetAllAsync();
        var totalOrders = await _orderRepository.GetTotalCountAsync();
        var totalRevenue = await _orderRepository.GetTotalRevenueAsync();
        var lowStockProducts = await _inventoryService.GetLowStockProductsAsync(5);

        var viewModel = new AdminDashboardViewModel
        {
            TotalProducts = allProducts.Count,
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue,
            LowStockCount = lowStockProducts.Count,
            LowStockProducts = lowStockProducts.Select(MapToProductCard).ToList()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Products()
    {
        var products = await _productRepository.GetAllAsync();

        var viewModel = new AdminProductListViewModel
        {
            Products = products.Select(p => new AdminProductListItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category,
                Price = p.Price,
                Stock = p.StockQuantity,
                IsActive = p.IsActive
            }).ToList()
        };

        return View("Products/Index", viewModel);
    }

    [HttpGet]
    public IActionResult CreateProduct()
    {
        return View("Products/Create", new AdminProductFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(AdminProductFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Products/Create", model);

        var product = new Product
        {
            Name = model.Name,
            Slug = model.Slug,
            Description = model.Description,
            Price = model.Price,
            CompareAtPrice = model.CompareAtPrice,
            Category = model.Category,
            RoastLevel = model.RoastLevel,
            Origin = model.Origin,
            Region = model.Region,
            Altitude = model.Altitude,
            Process = model.Process,
            FlavorNotes = model.FlavorNotesInput,
            ImageUrl = model.ImageUrl,
            StockQuantity = model.StockQuantity,
            IsActive = model.IsActive,
            IsFeatured = model.IsFeatured
        };

        await _productRepository.AddAsync(product);

        TempData["SuccessMessage"] = "Product created successfully.";
        return RedirectToAction(nameof(Products));
    }

    [HttpGet]
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        var viewModel = new AdminProductFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Description = product.Description,
            Price = product.Price,
            CompareAtPrice = product.CompareAtPrice,
            Category = product.Category,
            RoastLevel = product.RoastLevel,
            Origin = product.Origin,
            Region = product.Region,
            Altitude = product.Altitude,
            Process = product.Process,
            FlavorNotesInput = product.FlavorNotes,
            ImageUrl = product.ImageUrl,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            IsFeatured = product.IsFeatured
        };

        return View("Products/Edit", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(AdminProductFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Products/Edit", model);

        var product = await _productRepository.GetByIdAsync(model.Id!.Value);
        if (product == null)
            return NotFound();

        product.Name = model.Name;
        product.Slug = model.Slug;
        product.Description = model.Description;
        product.Price = model.Price;
        product.CompareAtPrice = model.CompareAtPrice;
        product.Category = model.Category;
        product.RoastLevel = model.RoastLevel;
        product.Origin = model.Origin;
        product.Region = model.Region;
        product.Altitude = model.Altitude;
        product.Process = model.Process;
        product.FlavorNotes = model.FlavorNotesInput;
        product.ImageUrl = model.ImageUrl;
        product.StockQuantity = model.StockQuantity;
        product.IsActive = model.IsActive;
        product.IsFeatured = model.IsFeatured;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product);

        TempData["SuccessMessage"] = "Product updated successfully.";
        return RedirectToAction(nameof(Products));
    }

    public async Task<IActionResult> Orders()
    {
        var orders = await _orderRepository.GetAllAsync();

        return View("Orders/Index", orders);
    }

    public async Task<IActionResult> OrderDetail(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            return NotFound();

        var viewModel = new OrderDetailViewModel
        {
            OrderNumber = order.OrderNumber,
            CreatedAt = order.CreatedAt,
            Status = order.Status,
            PaymentStatus = order.PaymentStatus,
            ShippingFullName = order.ShippingName,
            ShippingAddressLine1 = order.ShippingAddressLine1,
            ShippingAddressLine2 = order.ShippingAddressLine2,
            ShippingCity = order.ShippingCity,
            ShippingState = order.ShippingState,
            ShippingZipCode = order.ShippingZipCode,
            ShippingCountry = order.ShippingCountry,
            Items = order.Items.Select(i => new OrderItemViewModel
            {
                Name = i.ProductName,
                Slug = i.ProductSlug,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.LineTotal,
                Weight = i.Weight,
                Grind = i.Grind
            }).ToList(),
            Subtotal = order.Subtotal,
            Shipping = order.ShippingCost,
            Tax = order.Tax,
            Discount = order.DiscountAmount,
            Total = order.Total
        };

        return View("Orders/Detail", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrderStatus(int id, string status)
    {
        if (!Enum.TryParse<OrderStatus>(status, out var orderStatus))
        {
            TempData["ErrorMessage"] = "Invalid order status.";
            return RedirectToAction(nameof(OrderDetail), new { id });
        }

        var success = await _orderService.UpdateStatusAsync(id, orderStatus);
        if (!success)
        {
            TempData["ErrorMessage"] = "Failed to update order status.";
            return RedirectToAction(nameof(OrderDetail), new { id });
        }

        TempData["SuccessMessage"] = "Order status updated successfully.";
        return RedirectToAction(nameof(OrderDetail), new { id });
    }

    private static ProductCardViewModel MapToProductCard(Product p)
    {
        return new ProductCardViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Slug = p.Slug,
            Price = p.Price,
            CompareAtPrice = p.CompareAtPrice,
            ImageUrl = p.ImageUrl,
            Category = p.Category,
            RoastLevel = p.RoastLevel,
            AverageRating = p.AverageRating,
            ReviewCount = p.ReviewCount,
            StockStatus = p.StockStatus,
            IsFeatured = p.IsFeatured
        };
    }
}
