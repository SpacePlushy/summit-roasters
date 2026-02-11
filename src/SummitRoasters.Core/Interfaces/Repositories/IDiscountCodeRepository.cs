using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Repositories;

public interface IDiscountCodeRepository
{
    Task<DiscountCode?> GetByCodeAsync(string code);
    Task IncrementUsageAsync(int id);
}
