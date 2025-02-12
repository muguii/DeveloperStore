using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Sales;

namespace Sales.Infrastructure.Persistence.Configurations;

internal sealed class SaleConfigurations : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        string className = builder.Metadata.ClrType.Name;

        builder.ToTable(className);

        builder.Ignore(s => s.DomainEvents);

        builder.HasKey(s => s.Id)
               .HasName($"PK_{className}_Id");
        builder.Property(s => s.Id)
               .ValueGeneratedNever()
               .IsRequired()
               .HasColumnOrder(1);

        builder.Property(s => s.TotalAmount)
               .HasPrecision(19, 4)
               .HasDefaultValue(0M);

        builder.HasMany(s => s.Products)
               .WithOne()
               .HasForeignKey(si => si.SaleId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Cascade);
    }
}