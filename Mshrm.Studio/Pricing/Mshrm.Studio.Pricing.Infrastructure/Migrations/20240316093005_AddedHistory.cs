using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mshrm.Studio.Pricing.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangePricingPairHistories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExchangePricingPairId = table.Column<int>(type: "int", nullable: false),
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangePricingPairHistories",
                schema: "dbo");
        }
    }
}
