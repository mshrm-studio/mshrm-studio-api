﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mshrm.Studio.Pricing.Api.Context;

#nullable disable

namespace Mshrm.Studio.Pricing.Api.Migrations
{
    [DbContext(typeof(MshrmStudioPricingDbContext))]
    [Migration("20240316114501_AddedProviderTypeToPricingPair")]
    partial class AddedProviderTypeToPricingPair
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Mshrm.Studio.Pricing.Api.Models.Entites.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurrencyType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("LogoGuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderType")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SymbolNative")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Symbol", "ProviderType", "Active");

                    b.ToTable("Currencies", "dbo");
                });

            modelBuilder.Entity("Mshrm.Studio.Pricing.Api.Models.Entites.ExchangePricingPair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BaseCurrencyId")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<Guid>("GuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("MarketCap")
                        .HasColumnType("decimal(28, 8)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(28, 8)");

                    b.Property<string>("PricingProviderType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Volume")
                        .HasColumnType("decimal(28, 8)");

                    b.HasKey("Id");

                    b.HasIndex("BaseCurrencyId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("GuidId");

                    b.ToTable("ExchangePricingPairs", "dbo");
                });

            modelBuilder.Entity("Mshrm.Studio.Pricing.Api.Models.Entites.ExchangePricingPairHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExchangePricingPairId")
                        .HasColumnType("int");

                    b.Property<Guid>("GuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("NewMarketCap")
                        .HasColumnType("decimal(28, 8)");

                    b.Property<decimal>("NewPrice")
                        .HasColumnType("decimal(28, 8)");

                    b.Property<decimal?>("NewVolume")
                        .HasColumnType("decimal(28, 8)");

                    b.Property<decimal?>("OldMarketCap")
                        .HasColumnType("decimal(28, 8)");

                    b.Property<decimal>("OldPrice")
                        .HasColumnType("decimal(28, 8)");

                    b.Property<decimal?>("OldVolume")
                        .HasColumnType("decimal(28, 8)");

                    b.Property<string>("PricingProviderType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ExchangePricingPairId");

                    b.HasIndex("GuidId");

                    b.ToTable("ExchangePricingPairHistories", "dbo");
                });

            modelBuilder.Entity("Mshrm.Studio.Pricing.Api.Models.Entites.ExchangePricingPair", b =>
                {
                    b.HasOne("Mshrm.Studio.Pricing.Api.Models.Entites.Currency", "BaseCurrency")
                        .WithMany("BasePricingPairs")
                        .HasForeignKey("BaseCurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mshrm.Studio.Pricing.Api.Models.Entites.Currency", "Currency")
                        .WithMany("PricingPairs")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BaseCurrency");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("Mshrm.Studio.Pricing.Api.Models.Entites.ExchangePricingPairHistory", b =>
                {
                    b.HasOne("Mshrm.Studio.Pricing.Api.Models.Entites.ExchangePricingPair", "ExchangePricingPair")
                        .WithMany("ExchangePricingPairHistories")
                        .HasForeignKey("ExchangePricingPairId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExchangePricingPair");
                });

            modelBuilder.Entity("Mshrm.Studio.Pricing.Api.Models.Entites.Currency", b =>
                {
                    b.Navigation("BasePricingPairs");

                    b.Navigation("PricingPairs");
                });

            modelBuilder.Entity("Mshrm.Studio.Pricing.Api.Models.Entites.ExchangePricingPair", b =>
                {
                    b.Navigation("ExchangePricingPairHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
