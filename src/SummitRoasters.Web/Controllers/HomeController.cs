using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Models.Enums;
using SummitRoasters.Web.Models;
using SummitRoasters.Web.ViewModels;

namespace SummitRoasters.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository _productRepository;

    public HomeController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Index()
    {
        var featuredProducts = await _productRepository.GetFeaturedAsync(6);
        var newArrivals = await _productRepository.GetNewArrivalsAsync(6);

        var viewModel = new HomeViewModel
        {
            FeaturedProducts = featuredProducts.Select(MapToProductCard).ToList(),
            NewArrivals = newArrivals.Select(MapToProductCard).ToList(),
            CategoryCards = GetCategoryCards()
        };

        return View(viewModel);
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

    private static List<CategoryCardViewModel> GetCategoryCards()
    {
        return new List<CategoryCardViewModel>
        {
            new()
            {
                Name = "Single Origin",
                Slug = "single-origin",
                Description = "Explore unique flavors from around the world",
                IconClass = "bi-globe"
            },
            new()
            {
                Name = "Blends",
                Slug = "blends",
                Description = "Expertly crafted flavor combinations",
                IconClass = "bi-layers"
            },
            new()
            {
                Name = "Decaf",
                Slug = "decaf",
                Description = "All the flavor, none of the caffeine",
                IconClass = "bi-moon"
            },
            new()
            {
                Name = "Equipment",
                Slug = "equipment",
                Description = "Brew the perfect cup at home",
                IconClass = "bi-gear"
            },
            new()
            {
                Name = "Accessories",
                Slug = "accessories",
                Description = "Complete your coffee experience",
                IconClass = "bi-cup-hot"
            },
            new()
            {
                Name = "Subscriptions",
                Slug = "subscriptions",
                Description = "Never run out of great coffee",
                IconClass = "bi-arrow-repeat"
            }
        };
    }
}
