using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Services;

public interface IReviewService
{
    Task<Review> SubmitReviewAsync(CreateReviewDto dto);
    Task<Dictionary<int, int>> GetRatingBreakdownAsync(int productId);
    Task<bool> IsVerifiedPurchaseAsync(string userId, int productId);
    Task<bool> HasUserReviewedAsync(string userId, int productId);
}
