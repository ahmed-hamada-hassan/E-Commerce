using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configs;

public class CancellationConfiguration : IEntityTypeConfiguration<Cancellation>
{
    public void Configure(EntityTypeBuilder<Cancellation> builder)
    {
        #region Properties
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.CancellationDate)
            .IsRequired();
        #endregion

        #region Relationships
        builder.HasOne(c => c.Order)
            .WithOne(o => o.Cancellation)
            .HasForeignKey<Cancellation>(c => c.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion

        builder.ToTable("Cancellations", "CheckOut");
    }
}
