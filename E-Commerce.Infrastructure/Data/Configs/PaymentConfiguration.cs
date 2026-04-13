using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configs;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        #region Properties
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.PaymentMethod)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(p => p.PaymentStatus)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(p => p.PaymentDate)
            .IsRequired();

        builder.Property(p => p.TransactionId)
            .IsRequired(false)
            .HasMaxLength(150);
        #endregion

        #region Relationships
        builder.HasMany(p => p.Refunds)
            .WithOne(r => r.Payment)
            .HasForeignKey(r => r.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Order>()
            .WithMany(o => o.Payments)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion

        builder.ToTable("Payments", "Finance");
    }
}
