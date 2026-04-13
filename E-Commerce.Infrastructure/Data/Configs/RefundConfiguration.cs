using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;

namespace E_Commerce.Infrastructure.Data.Configs;

public class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        #region Properties
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(r => r.Reason)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(r => r.RefundDate)
            .IsRequired();

        builder.Property(r => r.RefundStatus)
            .HasConversion<string>()
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(r => r.RefundTransactionId)
            .HasMaxLength(150)
            .IsRequired(false);
        #endregion

        #region Relationships
        builder.HasOne(r => r.Order)
            .WithMany(o => o.Refunds)
            .HasForeignKey(r => r.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        builder.ToTable("Refunds", "Finance");
    }
}
