using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Infrastructure.Data.Configurations;

public class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
{
    public void Configure(EntityTypeBuilder<DiscountCode> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasIndex(d => d.Code).IsUnique();

        builder.Property(d => d.Code).IsRequired().HasMaxLength(50);
        builder.Property(d => d.Value).HasPrecision(18, 2);
        builder.Property(d => d.MinimumOrderAmount).HasPrecision(18, 2);

        builder.Ignore(d => d.IsValid);
    }
}
