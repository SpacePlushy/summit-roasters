using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IOrderRepository _orderRepository;

    public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository)
    {
        _reviewRepository = reviewRepository;
        _orderRepository = orderRepository;
    }

    public async Task<Review> SubmitReviewAsync(CreateReviewDto dto)
    {
        var alreadyReviewed = await _reviewRepository.ExistsForUserAndProductAsync(dto.UserId, dto.ProductId);
        if (alreadyReviewed)
            throw new InvalidOperationException("You have already reviewed this product.");

        var isVerified = await IsVerifiedPurchaseAsync(dto.UserId, dto.ProductId);

        var review = new Review
        {
            ProductId = dto.ProductId,
            UserId = dto.UserId,
            Rating = Math.Clamp(dto.Rating, 1, 5),
            Title = dto.Title,
            Body = dto.Body,
            IsVerifiedPurchase = isVerified
        };

        return await _reviewRepository.AddAsync(review);
    }

    public async Task<Dictionary<int, int>> GetRatingBreakdownAsync(int productId)
    {
        var (reviews, _) = await _reviewRepository.GetByProductIdAsync(productId, 1, int.MaxValue);
        var breakdown = new Dictionary<int, int>
        {
            { 5, 0 }, { 4, 0 }, { 3, 0 }, { 2, 0 }, { 1, 0 }
        };

        foreach (var review in reviews)
        {
            if (breakdown.ContainsKey(review.Rating))
                breakdown[review.Rating]++;
        }

        return breakdown;
    }

    public async Task<bool> IsVerifiedPurchaseAsync(string userId, int productId)
    {
        return await _orderRepository.UserHasPurchasedProductAsync(userId, productId);
    }

    public async Task<bool> HasUserReviewedAsync(string userId, int productId)
    {
        return await _reviewRepository.ExistsForUserAndProductAsync(userId, productId);
    }
}
