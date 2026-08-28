using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configs;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        #region Properties
        builder.HasKey(w => w.Id);
        #endregion

        #region Relationships
        builder.HasOne(w => w.User)
            .WithMany()
            .HasForeignKey(w => w.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata.FindNavigation(nameof(Wishlist.Items))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        #endregion
        builder.ToTable("Wishlists", "Shopping");
    }
}