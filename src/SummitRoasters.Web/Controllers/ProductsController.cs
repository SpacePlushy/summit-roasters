using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Models.Enums;
using SummitRoasters.Web.ViewModels;

namespace SummitRoasters.Web.Controllers;

[Route("products")]
public class ProductsController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IReviewService _reviewService;

    public ProductsController(IProductRepository productRepository, IReviewService reviewService)
    {
        _productRepository = productRepository;
        _reviewService = reviewService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(
        Category? category,
        RoastLevel? roastLevel,
        decimal? minPrice,
        decimal? maxPrice,
        int? minRating,
        string? sortBy,
        int page = 1)
    {
        const int pageSize = 12;

        var filter = new ProductFilterDto
        {
            Category = category,
            RoastLevel = roastLevel,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            MinRating = minRating,
            SortBy = sortBy,
            Page = page,
            PageSize = pageSize
        };

        var (products, totalCount) = await _productRepository.GetFilteredAsync(filter);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var viewModel = new ProductListViewModel
        {
            Products = products.Select(MapToProductCard).ToList(),
            Filter = new ProductFilterViewModel
            {
                SelectedCategory = category,
                SelectedRoastLevel = roastLevel,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinRating = minRating,
                SortBy = sortBy
            },
            Pagination = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalCount,
                PageSize = pageSize,
                BaseUrl = "/products"
            }
        };

        return View(viewModel);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        var product = await _productRepository.GetBySlugAsync(slug);
        if (product == null)
            return NotFound();

        var relatedProducts = await _productRepository.GetRelatedAsync(product.Id, 4);
        var ratingBreakdown = await _reviewService.GetRatingBreakdownAsync(product.Id);

        var canReview = false;
        var hasReviewed = false;

        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                hasReviewed = await _reviewService.HasUserReviewedAsync(userId, product.Id);
                canReview = !hasReviewed;
            }
        }

        var viewModel = new ProductDetailViewModel
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
            FlavorNotes = !string.IsNullOrWhiteSpace(product.FlavorNotes)
                ? product.FlavorNotes.Split(',', StringSplitOptions.TrimEntries).ToList()
                : new List<string>(),
            ImageUrl = product.ImageUrl,
            StockStatus = product.StockStatus,
            StockQuantity = product.StockQuantity,
            IsFeatured = product.IsFeatured,
            WeightOptions = product.WeightOptions.Select(w => new WeightOptionViewModel
            {
                Weight = w.Weight,
                PriceAdjustment = w.PriceAdjustment,
                IsDefault = w.IsDefault
            }).ToList(),
            ReviewSummary = new ReviewSummaryViewModel
            {
                AverageRating = product.AverageRating,
                TotalReviews = product.ReviewCount,
                RatingBreakdown = ratingBreakdown
            },
            Reviews = product.Reviews.Select(r => new ReviewViewModel
            {
                UserName = r.UserName,
                Rating = r.Rating,
                Title = r.Title,
                Body = r.Body,
                IsVerifiedPurchase = r.IsVerifiedPurchase,
                CreatedAt = r.CreatedAt
            }).ToList(),
            RelatedProducts = relatedProducts.Select(MapToProductCard).ToList(),
            CanReview = canReview,
            HasReviewed = hasReviewed
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
