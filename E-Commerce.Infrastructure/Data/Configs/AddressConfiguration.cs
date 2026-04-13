using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configs;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        #region Properties
        builder.HasKey(a => a.Id);

        builder.Property(a => a.AddressLine1)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.AddressLine2)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.StateOrProvince)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(a => a.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(a => a.AddressType)
            .HasConversion<string>()
            .HasMaxLength(25)
            .IsRequired();
        #endregion

        builder.ToTable("Addresses", "Identity");
    }
}
