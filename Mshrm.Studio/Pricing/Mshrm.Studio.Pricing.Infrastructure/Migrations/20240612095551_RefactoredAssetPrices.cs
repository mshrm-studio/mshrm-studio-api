using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mshrm.Studio.Pricing.Api.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredAssetPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangePricingPairHistories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ExchangePricingPairs",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "AssetPrices",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseAssetId = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(28,8)", nullable: false),
                    MarketCap = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    Volume = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    PricingProviderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetPrices_Assets_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "dbo",
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetPrices_Assets_BaseAssetId",
                        column: x => x.BaseAssetId,
                        principalSchema: "dbo",
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetPriceHistories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetPriceId = table.Column<int>(type: "int", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(28,8)", nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(28,8)", nullable: false),
                    OldMarketCap = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    NewMarketCap = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    OldVolume = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    NewVolume = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    PricingProviderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetPriceHistories_AssetPrices_AssetPriceId",
                        column: x => x.AssetPriceId,
                        principalSchema: "dbo",
                        principalTable: "AssetPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetPriceHistories_AssetPriceId",
                schema: "dbo",
                table: "AssetPriceHistories",
                column: "AssetPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetPriceHistories_GuidId",
                schema: "dbo",
                table: "AssetPriceHistories",
                column: "GuidId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetPrices_AssetId",
                schema: "dbo",
                table: "AssetPrices",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetPrices_BaseAssetId",
                schema: "dbo",
                table: "AssetPrices",
                column: "BaseAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetPrices_GuidId",
                schema: "dbo",
                table: "AssetPrices",
                column: "GuidId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetPriceHistories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AssetPrices",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "ExchangePricingPairs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    BaseAssetId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarketCap = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(28,8)", nullable: false),
                    PricingProviderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Volume = table.Column<decimal>(type: "decimal(28,8)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangePricingPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangePricingPairs_Assets_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "dbo",
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangePricingPairs_Assets_BaseAssetId",
                        column: x => x.BaseAssetId,
                        principalSchema: "dbo",
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExchangePricingPairHistories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExchangePricingPairId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewMarketCap = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    NewPrice = table.Column<decimal>(type: "decimal(28,8)", nullable: false),
                    NewVolume = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    OldMarketCap = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    OldPrice = table.Column<decimal>(type: "decimal(28,8)", nullable: false),
                    OldVolume = table.Column<decimal>(type: "decimal(28,8)", nullable: true),
                    PricingProviderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangePricingPairHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangePricingPairHistories_ExchangePricingPairs_ExchangePricingPairId",
                        column: x => x.ExchangePricingPairId,
                        principalSchema: "dbo",
                        principalTable: "ExchangePricingPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangePricingPairHistories_ExchangePricingPairId",
                schema: "dbo",
                table: "ExchangePricingPairHistories",
                column: "ExchangePricingPairId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangePricingPairHistories_GuidId",
                schema: "dbo",
                table: "ExchangePricingPairHistories",
                column: "GuidId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangePricingPairs_AssetId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangePricingPairs_BaseAssetId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                column: "BaseAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangePricingPairs_GuidId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                column: "GuidId");
        }
    }
}
