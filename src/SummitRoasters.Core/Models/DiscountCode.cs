using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Core.Models;

public class DiscountCode
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DiscountType Type { get; set; }
    public decimal Value { get; set; }
    public decimal? MinimumOrderAmount { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int? MaxUses { get; set; }
    public int CurrentUses { get; set; }
    public bool IsActive { get; set; } = true;

    public bool IsValid => IsActive
        && (ExpiresAt == null || ExpiresAt > DateTime.UtcNow)
        && (MaxUses == null || CurrentUses < MaxUses);
}
