using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Sales;

namespace Sales.Infrastructure.Persistence.Configurations;

internal sealed class SaleItemConfigurations : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        string className = builder.Metadata.ClrType.Name;

        builder.ToTable(className);

        builder.Ignore(si => si.DomainEvents);

        builder.HasKey(si => si.Id)
               .HasName($"PK_{className}_Id");
        builder.Property(si => si.Id)
               .ValueGeneratedNever()
               .IsRequired()
               .HasColumnOrder(1);

        builder.HasOne(si => si.Product)
               .WithMany()
               .HasForeignKey(si => si.ProductId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.Property(si => si.UnitPrice)
               .HasPrecision(19, 4);

        builder.Property(si => si.Discount)
               .HasPrecision(19, 4)
               .HasDefaultValue(0M);

        builder.Property(si => si.TotalAmount)
               .HasPrecision(19, 4);
    }
}