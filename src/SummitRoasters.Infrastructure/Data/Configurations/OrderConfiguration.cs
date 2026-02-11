using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasIndex(o => o.OrderNumber).IsUnique();

        builder.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
        builder.Property(o => o.Subtotal).HasPrecision(18, 2);
        builder.Property(o => o.ShippingCost).HasPrecision(18, 2);
        builder.Property(o => o.Tax).HasPrecision(18, 2);
        builder.Property(o => o.DiscountAmount).HasPrecision(18, 2);
        builder.Property(o => o.Total).HasPrecision(18, 2);
        builder.Property(o => o.DiscountCode).HasMaxLength(50);

        builder.Property(o => o.ShippingName).IsRequired().HasMaxLength(200);
        builder.Property(o => o.ShippingAddressLine1).IsRequired().HasMaxLength(200);
        builder.Property(o => o.ShippingAddressLine2).HasMaxLength(200);
        builder.Property(o => o.ShippingCity).IsRequired().HasMaxLength(100);
        builder.Property(o => o.ShippingState).IsRequired().HasMaxLength(50);
        builder.Property(o => o.ShippingZipCode).IsRequired().HasMaxLength(20);
        builder.Property(o => o.ShippingCountry).IsRequired().HasMaxLength(10);
        builder.Property(o => o.ShippingPhone).HasMaxLength(20);

        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId);
    }
}
