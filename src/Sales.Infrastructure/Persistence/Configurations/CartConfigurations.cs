﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Carts;

namespace Sales.Infrastructure.Persistence.Configurations;

internal sealed class CartConfigurations : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        string className = builder.Metadata.ClrType.Name;

        builder.ToTable(className);

        builder.Ignore(c => c.DomainEvents);

        builder.HasKey(c => c.Id)
               .HasName($"PK_{className}_Id");
        builder.Property(c => c.Id)
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