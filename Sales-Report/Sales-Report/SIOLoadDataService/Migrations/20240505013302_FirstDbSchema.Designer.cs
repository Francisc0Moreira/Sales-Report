﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SIOLoadDataService.Models;

#nullable disable

namespace SIOLoadDataService.Migrations
{
    [DbContext(typeof(OnContext))]
    [Migration("20240505013302_FirstDbSchema")]
    partial class FirstDbSchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SIOLoadDataService.Models.Clients", b =>
                {
                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocalId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nif")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClientId");

                    b.HasIndex("LocalId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("SIOLoadDataService.Models.Local", b =>
                {
                    b.Property<string>("LocalId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocalId");

                    b.ToTable("Locals");
                });

            modelBuilder.Entity("SIOLoadDataService.Models.Products", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Family")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("Price_Iva")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Unity")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SIOLoadDataService.Models.Sales", b =>
                {
                    b.Property<string>("SalesId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2(7)");

                    b.Property<decimal?>("GrossTotal")
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal?>("Total")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("SalesId");

                    b.HasIndex("ClientsId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("SIOLoadDataService.Models.SalesLines", b =>
                {
                    b.Property<int>("SalesLinesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SalesLinesId"));

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Line")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("ProductPrice")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("ProductsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SalesId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("Total")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("SalesLinesId");

                    b.HasIndex("ProductsId");

                    b.HasIndex("SalesId");

                    b.ToTable("SalesLines");
                });

            modelBuilder.Entity("SIOLoadDataService.Models.Clients", b =>
                {
                    b.HasOne("SIOLoadDataService.Models.Local", "Local")
                        .WithMany()
                        .HasForeignKey("LocalId");

                    b.Navigation("Local");
                });

            modelBuilder.Entity("SIOLoadDataService.Models.Sales", b =>
                {
                    b.HasOne("SIOLoadDataService.Models.Clients", "Clients")
                        .WithMany()
                        .HasForeignKey("ClientsId");

                    b.Navigation("Clients");
                });

            modelBuilder.Entity("SIOLoadDataService.Models.SalesLines", b =>
                {
                    b.HasOne("SIOLoadDataService.Models.Products", "Products")
                        .WithMany()
                        .HasForeignKey("ProductsId");

                    b.HasOne("SIOLoadDataService.Models.Sales", "Sales")
                        .WithMany()
                        .HasForeignKey("SalesId");

                    b.Navigation("Products");

                    b.Navigation("Sales");
                });
#pragma warning restore 612, 618
        }
    }
}
