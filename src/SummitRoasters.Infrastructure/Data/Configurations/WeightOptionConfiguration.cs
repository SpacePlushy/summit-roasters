using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Infrastructure.Data.Configurations;

public class WeightOptionConfiguration : IEntityTypeConfiguration<WeightOption>
{
    public void Configure(EntityTypeBuilder<WeightOption> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Weight).IsRequired().HasMaxLength(50);
        builder.Property(w => w.PriceAdjustment).HasPrecision(18, 2);

        builder.HasOne(w => w.Product)
            .WithMany(p => p.WeightOptions)
            .HasForeignKey(w => w.ProductId);
    }
}
