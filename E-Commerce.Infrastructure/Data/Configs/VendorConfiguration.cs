using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configs;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        #region Properties
        builder.HasKey(v => v.Id);

        builder.Property(v => v.StoreName)
            .IsRequired().HasMaxLength(100);

        builder.Property(v => v.CommercialRegistrationNumber)
            .IsRequired().HasMaxLength(50);

        builder.Property(v => v.IsActive)
            .IsRequired().HasDefaultValue(false);

        builder.HasIndex(v => v.StoreName).IsUnique();

        builder.HasIndex(v => v.CommercialRegistrationNumber).IsUnique();
        #endregion

        #region Relationships
        builder.HasOne(v => v.User)
            .WithOne()
            .HasForeignKey<Vendor>(v => v.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany<Product>()
            .WithOne(p => p.Vendor)
            .HasForeignKey(p => p.VendorId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        builder.ToTable("Vendors", "Catalog");
    }
}
