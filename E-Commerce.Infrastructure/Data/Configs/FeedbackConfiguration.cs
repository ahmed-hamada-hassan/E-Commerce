using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configs;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        #region Properties
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Rating)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(r => r.Comment)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(r => r.CreatedDate)
            .IsRequired();

        builder.Property(r => r.UpdatedDate)
            .IsRequired(false);

        builder.Property(r => r.IsApproved)
            .IsRequired();

        builder.Property(r => r.IsVerifiedPurchase)
            .IsRequired();
        #endregion

        #region Relationships
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        builder.ToTable("Reviews", "Catalog");
    }
}
