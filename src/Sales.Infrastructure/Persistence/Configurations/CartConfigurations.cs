using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Carts;

namespace Sales.Infrastructure.Persistence.Configurations;

internal sealed class CartConfigurations : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        string className = builder.Metadata.ClrType.Name;

        builder.ToTable(className);

        builder.HasKey(p => p.Id)
               .HasName($"PK_{className}_Id");
        builder.Property(p => p.Id)
               .ValueGeneratedNever()
               .IsRequired()
               .HasColumnOrder(1);

        builder.HasMany(c => c.Products)
               .WithOne()
               .HasForeignKey(ci => ci.CartId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Cascade);
    }
}