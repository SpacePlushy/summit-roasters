using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.Slug).IsUnique();

        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Slug).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Description).HasMaxLength(2000);
        builder.Property(p => p.Price).HasPrecision(18, 2);
        builder.Property(p => p.CompareAtPrice).HasPrecision(18, 2);
        builder.Property(p => p.Origin).HasMaxLength(100);
        builder.Property(p => p.Region).HasMaxLength(100);
        builder.Property(p => p.Altitude).HasMaxLength(50);
        builder.Property(p => p.Process).HasMaxLength(100);
        builder.Property(p => p.ImageUrl).HasMaxLength(500);

        builder.Property(p => p.FlavorNotes)
            .HasMaxLength(1000);

        builder.Ignore(p => p.StockStatus);
        builder.Ignore(p => p.AverageRating);
        builder.Ignore(p => p.ReviewCount);
    }
}
