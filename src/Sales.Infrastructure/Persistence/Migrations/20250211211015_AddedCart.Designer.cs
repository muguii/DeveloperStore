﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sales.Infrastructure.Persistence;

#nullable disable

namespace Sales.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250211211015_AddedCart")]
    partial class AddedCart
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Sales.Domain.Carts.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("PK_Cart_Id");

                    b.ToTable("Cart", (string)null);
                });

            modelBuilder.Entity("Sales.Domain.Carts.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(1);

                    b.Property<Guid>("CartId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("PK_CartItem_Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId")
                        .IsUnique();

                    b.ToTable("CartItem", (string)null);
                });

            modelBuilder.Entity("Sales.Domain.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(1);

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ComplexProperty<Dictionary<string, object>>("Rating", "Sales.Domain.Products.Product.Rating#Rating", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Count")
                                .HasColumnType("integer");

                            b1.Property<decimal>("Rate")
                                .HasColumnType("numeric");
                        });

                    b.HasKey("Id")
                        .HasName("PK_Product_Id");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("Sales.Domain.Carts.CartItem", b =>
                {
                    b.HasOne("Sales.Domain.Carts.Cart", null)
                        .WithMany("Products")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Sales.Domain.Products.Product", "Product")
                        .WithOne()
                        .HasForeignKey("Sales.Domain.Carts.CartItem", "ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Sales.Domain.Carts.Cart", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
