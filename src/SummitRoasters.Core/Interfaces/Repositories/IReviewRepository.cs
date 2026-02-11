using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Repositories;

public interface IReviewRepository
{
    Task<(List<Review> Reviews, int TotalCount)> GetByProductIdAsync(int productId, int page = 1, int pageSize = 10);
    Task<Review> AddAsync(Review review);
    Task<bool> ExistsForUserAndProductAsync(string userId, int productId);
}
