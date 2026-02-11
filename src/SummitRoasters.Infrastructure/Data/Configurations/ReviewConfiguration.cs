using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => new { r.UserId, r.ProductId }).IsUnique();

        builder.Property(r => r.Title).IsRequired().HasMaxLength(200);
        builder.Property(r => r.Body).IsRequired().HasMaxLength(2000);
        builder.Property(r => r.UserName).IsRequired().HasMaxLength(100);

        builder.HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId);

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId);
    }
}
