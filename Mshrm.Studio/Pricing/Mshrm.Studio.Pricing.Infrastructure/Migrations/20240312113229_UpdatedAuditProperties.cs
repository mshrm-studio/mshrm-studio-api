using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mshrm.Studio.Pricing.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAuditProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                schema: "dbo",
                table: "ExchangePricingPairs",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                schema: "dbo",
                table: "ExchangePricingPairs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                schema: "dbo",
                table: "ExchangePricingPairs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "dbo",
                table: "ExchangePricingPairs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                schema: "dbo",
                table: "Currencies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                schema: "dbo",
                table: "Currencies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "dbo",
                table: "Currencies",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "dbo",
                table: "ExchangePricingPairs");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                schema: "dbo",
                table: "ExchangePricingPairs");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "dbo",
                table: "ExchangePricingPairs");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "dbo",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                schema: "dbo",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "dbo",
                table: "Currencies");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "dbo",
                table: "ExchangePricingPairs",
                newName: "Value");
        }
    }
}
