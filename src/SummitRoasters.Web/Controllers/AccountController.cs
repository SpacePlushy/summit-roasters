using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;
using SummitRoasters.Web.ViewModels;

namespace SummitRoasters.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IOrderRepository _orderRepository;
    private readonly IShippingAddressRepository _shippingAddressRepository;
    private readonly ICartService _cartService;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IOrderRepository orderRepository,
        IShippingAddressRepository shippingAddressRepository,
        ICartService cartService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _orderRepository = orderRepository;
        _shippingAddressRepository = shippingAddressRepository;
        _cartService = cartService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        var viewModel = new LoginViewModel { ReturnUrl = returnUrl };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Customer");
            await _signInManager.SignInAsync(user, isPersistent: false);

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> Orders(int page = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        const int pageSize = 10;

        var (orders, totalCount) = await _orderRepository.GetByUserIdAsync(userId, page, pageSize);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var viewModel = new OrderHistoryViewModel
        {
            Orders = orders.Select(o => new OrderSummaryViewModel
            {
                OrderNumber = o.OrderNumber,
                Date = o.CreatedAt,
                Total = o.Total,
                Status = o.Status.ToString(),
                ItemCount = o.Items.Count
            }).ToList(),
            Pagination = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalCount,
                PageSize = pageSize,
                Action = "Orders",
                Controller = "Account"
            }
        };

        return View(viewModel);
    }

    [Authorize]
    public async Task<IActionResult> OrderDetail(string orderNumber)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order = await _orderRepository.GetByOrderNumberAsync(orderNumber);

        if (order == null || order.UserId != userId)
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

        return View(viewModel);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Settings()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var addresses = await _shippingAddressRepository.GetByUserIdAsync(user.Id);

        var viewModel = new AccountSettingsViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Addresses = addresses.Select(a => new ShippingAddressFormViewModel
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
            }).ToList()
        };

        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(AccountSettingsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var addresses = await _shippingAddressRepository.GetByUserIdAsync(userId);
            model.Addresses = addresses.Select(a => new ShippingAddressFormViewModel
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
            }).ToList();
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.UserName = model.Email;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Your settings have been updated.";
            return RedirectToAction(nameof(Settings));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAddress(ShippingAddressFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please fill in all required address fields.";
            return RedirectToAction(nameof(Settings));
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var address = new ShippingAddress
        {
            UserId = userId,
            FullName = model.FullName,
            AddressLine1 = model.AddressLine1,
            AddressLine2 = model.AddressLine2,
            City = model.City,
            State = model.State,
            ZipCode = model.ZipCode,
            Country = model.Country,
            Phone = model.Phone,
            IsDefault = model.IsDefault
        };

        await _shippingAddressRepository.AddAsync(address);

        if (model.IsDefault)
        {
            await _shippingAddressRepository.SetDefaultAsync(userId, address.Id);
        }

        TempData["SuccessMessage"] = "Address added successfully.";
        return RedirectToAction(nameof(Settings));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var address = await _shippingAddressRepository.GetByIdAsync(id);

        if (address == null || address.UserId != userId)
            return NotFound();

        await _shippingAddressRepository.DeleteAsync(id);

        TempData["SuccessMessage"] = "Address deleted successfully.";
        return RedirectToAction(nameof(Settings));
    }
}
