using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Products;

namespace Sales.Infrastructure.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        string className = builder.Metadata.ClrType.Name;

        builder.ToTable(className);

        builder.Ignore(p => p.DomainEvents);

        builder.HasKey(p => p.Id)
               .HasName($"PK_{className}_Id");
        builder.Property(p => p.Id)
               .ValueGeneratedNever()
               .IsRequired()
               .HasColumnOrder(1);

        builder.ComplexProperty(p => p.Rating);
    }
}