using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;
using SummitRoasters.Web.ViewModels;

namespace SummitRoasters.Web.Controllers;

public class SearchController : Controller
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<IActionResult> Index(string q)
    {
        var results = new List<Product>();

        if (!string.IsNullOrWhiteSpace(q))
        {
            results = await _searchService.SearchAsync(q);
        }

        var viewModel = new SearchResultsViewModel
        {
            Query = q ?? string.Empty,
            Results = results.Select(MapToProductCard).ToList(),
            TotalResults = results.Count
        };

        return View(viewModel);
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
