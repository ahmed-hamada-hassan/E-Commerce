using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configs;

public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
{
    public void Configure(EntityTypeBuilder<WishlistItem> builder)
    {
        #region Properties
        builder.HasKey(wi => wi.Id);
        builder.Property(wi => wi.Id).ValueGeneratedNever();
        builder.Property(w => w.AddedAt)
            .IsRequired();
        #endregion

        #region Relationships
        builder.HasOne(wi => wi.Product)
            .WithMany()
            .HasForeignKey(w => w.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wi => wi.Wishlist)
            .WithMany(w => w.Items)
            .HasForeignKey(wi => wi.WishlistId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


        #endregion

        builder.ToTable("WishlistItems", "Shopping");
    }
}
