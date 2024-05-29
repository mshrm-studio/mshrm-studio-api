using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mshrm.Studio.Pricing.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPriceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MarketCap",
                schema: "dbo",
                table: "ExchangePricingPairs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Volume",
                schema: "dbo",
                table: "ExchangePricingPairs",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketCap",
                schema: "dbo",
                table: "ExchangePricingPairs");

            migrationBuilder.DropColumn(
                name: "Volume",
                schema: "dbo",
                table: "ExchangePricingPairs");
        }
    }
}
