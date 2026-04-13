using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configs;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        #region Properties
        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(pi => pi.IsPrimary)
            .IsRequired();

        builder.HasIndex(pi => new {pi.ProductId, pi.IsPrimary})
            .IsUnique()
            .HasFilter("[IsPrimary] = 1");

        builder.Property(pi => pi.DisplayOrder)
            .HasColumnType("tinyint")
            .IsRequired();
        #endregion

        builder.ToTable("ProductImages", "Catalog");
    }
}
