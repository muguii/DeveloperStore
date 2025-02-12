using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Carts;

namespace Sales.Infrastructure.Persistence.Configurations;

internal sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        string className = builder.Metadata.ClrType.Name;

        builder.ToTable(className);

        builder.Ignore(ci => ci.DomainEvents);

        builder.HasKey(ci => ci.Id)
               .HasName($"PK_{className}_Id");
        builder.Property(ci => ci.Id)
               .ValueGeneratedNever()
               .IsRequired()
               .HasColumnOrder(1);

        builder.HasOne(ci => ci.Product)
               .WithMany()
               .HasForeignKey(ci => ci.ProductId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}