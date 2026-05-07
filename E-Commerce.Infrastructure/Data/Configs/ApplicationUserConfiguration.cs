using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configs;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        #region Properties
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(u => u.MiddleName)
            .IsRequired(false)
            .HasMaxLength(30);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(u => u.DateOfBirth)
            .IsRequired();
        #endregion

        #region Relationships
        builder.HasOne(u => u.DefaultShippingAddress)
            .WithMany()
            .HasForeignKey(u => u.DefaultShippingAddressId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Addresses)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //builder.Navigation(u => u.Addresses)
        //    .UsePropertyAccessMode(PropertyAccessMode.Field);

        //builder.HasOne(u => u.Cart)
        //    .WithOne(c => c.User)
        //    .HasForeignKey<Cart>(c => c.UserId)
        //    .IsRequired()
        //    .OnDelete(DeleteBehavior.Cascade);
        #endregion

        builder.ToTable("Users", "Identity");
    }
}
