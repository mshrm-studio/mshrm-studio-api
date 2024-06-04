using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mshrm.Studio.Pricing.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenamedCurrenciesToAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangePricingPairs_Currencies_BaseCurrencyId",
                schema: "dbo",
                table: "ExchangePricingPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangePricingPairs_Currencies_CurrencyId",
                schema: "dbo",
                table: "ExchangePricingPairs");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                newName: "BaseAssetId");

            migrationBuilder.RenameColumn(
                name: "BaseCurrencyId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                newName: "AssetId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangePricingPairs_CurrencyId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                newName: "IX_ExchangePricingPairs_BaseAssetId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangePricingPairs_BaseCurrencyId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                newName: "IX_ExchangePricingPairs_AssetId");

            migrationBuilder.CreateTable(
                name: "Assets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SymbolNative = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    LogoGuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProviderType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssetType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Symbol_ProviderType_Active",
                schema: "dbo",
                table: "Assets",
                columns: new[] { "Symbol", "ProviderType", "Active" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangePricingPairs_Assets_AssetId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                column: "AssetId",
                principalSchema: "dbo",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangePricingPairs_Assets_BaseAssetId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                column: "BaseAssetId",
                principalSchema: "dbo",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangePricingPairs_Assets_AssetId",
                schema: "dbo",
                table: "ExchangePricingPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangePricingPairs_Assets_BaseAssetId",
                schema: "dbo",
                table: "ExchangePricingPairs");

            migrationBuilder.DropTable(
                name: "Assets",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "BaseAssetId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                newName: "CurrencyId");

            migrationBuilder.RenameColumn(
                name: "AssetId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                newName: "BaseCurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangePricingPairs_BaseAssetId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                newName: "IX_ExchangePricingPairs_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangePricingPairs_AssetId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                newName: "IX_ExchangePricingPairs_BaseCurrencyId");

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogoGuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SymbolNative = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Symbol_ProviderType_Active",
                schema: "dbo",
                table: "Currencies",
                columns: new[] { "Symbol", "ProviderType", "Active" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangePricingPairs_Currencies_BaseCurrencyId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                column: "BaseCurrencyId",
                principalSchema: "dbo",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangePricingPairs_Currencies_CurrencyId",
                schema: "dbo",
                table: "ExchangePricingPairs",
                column: "CurrencyId",
                principalSchema: "dbo",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
