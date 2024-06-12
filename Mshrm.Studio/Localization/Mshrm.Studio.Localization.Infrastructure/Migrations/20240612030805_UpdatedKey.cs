using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mshrm.Studio.Localization.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "dbo",
                table: "LocalizationResources",
                newName: "Key");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "GuidId",
                value: new Guid("9883c99f-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "GuidId",
                value: new Guid("bfdbdce1-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 3,
                column: "GuidId",
                value: new Guid("7aeda311-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 4,
                column: "GuidId",
                value: new Guid("dda39430-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 5,
                column: "GuidId",
                value: new Guid("9b4f4d7c-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 6,
                column: "GuidId",
                value: new Guid("e7dcf3da-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 7,
                column: "GuidId",
                value: new Guid("b2f64f03-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 8,
                column: "GuidId",
                value: new Guid("fac2a8cd-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 9,
                column: "GuidId",
                value: new Guid("7ec54fe2-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 10,
                column: "GuidId",
                value: new Guid("d48e4db7-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 11,
                column: "GuidId",
                value: new Guid("b824dd67-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 12,
                column: "GuidId",
                value: new Guid("580488c5-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 13,
                column: "GuidId",
                value: new Guid("d9f10ea2-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 14,
                column: "GuidId",
                value: new Guid("971ade7d-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 15,
                column: "GuidId",
                value: new Guid("b2f64f03-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 16,
                column: "GuidId",
                value: new Guid("b07f69dd-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 17,
                column: "GuidId",
                value: new Guid("87ed5b61-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 18,
                column: "GuidId",
                value: new Guid("8b7d01af-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 19,
                column: "GuidId",
                value: new Guid("83e56390-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 20,
                column: "GuidId",
                value: new Guid("61294040-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 21,
                column: "GuidId",
                value: new Guid("0329fb3a-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 22,
                column: "GuidId",
                value: new Guid("5a9998e0-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 23,
                column: "GuidId",
                value: new Guid("28aeb50d-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 24,
                column: "GuidId",
                value: new Guid("3ef433fa-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 25,
                column: "GuidId",
                value: new Guid("60582306-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "GuidId", "Key", "Value" },
                values: new object[] { new Guid("455d0715-0000-0000-0000-000000000000"), "AssetAlreadyExists", "The asset already exists" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "GuidId", "Key", "Value" },
                values: new object[] { new Guid("20d5260b-0000-0000-0000-000000000000"), "AssetNotSupported", "Asset is not supported by the provider" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "GuidId", "Key", "Value" },
                values: new object[] { new Guid("c9f197da-0000-0000-0000-000000000000"), "BaseAssetNotSupported", "The base asset symbol provided is not supported" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "GuidId", "Key", "Value" },
                values: new object[] { new Guid("8dac8ede-0000-0000-0000-000000000000"), "BaseAssetPriceDoesntExist", "The base asset symbols price doesn't exist" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "GuidId", "Key", "Value" },
                values: new object[] { new Guid("c0df2371-0000-0000-0000-000000000000"), "AssetDoesntExist", "The asset doesn't exists" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 31,
                column: "GuidId",
                value: new Guid("1de54c93-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 32,
                column: "GuidId",
                value: new Guid("1ef82270-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 33,
                column: "GuidId",
                value: new Guid("01e3a1f4-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 34,
                column: "GuidId",
                value: new Guid("c1585671-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 35,
                column: "GuidId",
                value: new Guid("7cd16eab-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "LocalizationResources",
                columns: new[] { "Id", "Comment", "Culture", "GuidId", "Key", "LocalizationArea", "Value" },
                values: new object[,]
                {
                    { 36, null, "en-US", new Guid("e7f39836-0000-0000-0000-000000000000"), "SystemError", "Errors", "System error, please contact admin" },
                    { 37, null, "en-US", new Guid("4832254a-0000-0000-0000-000000000000"), "PricingStructureDoesntExist", "Errors", "The pricing structure doesn't exist" },
                    { 38, null, "en-US", new Guid("55a58ae7-0000-0000-0000-000000000000"), "AssetPriceMustBeACurrency", "Errors", "The asset price must be a currency" },
                    { 39, null, "en-US", new Guid("d3e735e8-0000-0000-0000-000000000000"), "FailureCodeIsInvalid", "Errors", "The failure code provided is invalid" },
                    { 40, null, "en-US", new Guid("f8f578b3-0000-0000-0000-000000000000"), "LocalizationAreaNotSupported", "Errors", "The localization area provided is not supported" },
                    { 41, null, "en-US", new Guid("42629b7a-0000-0000-0000-000000000000"), "CultureNotSupported", "Errors", "The culture provided is not supported" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.RenameColumn(
                name: "Key",
                schema: "dbo",
                table: "LocalizationResources",
                newName: "Name");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "GuidId",
                value: new Guid("c412242e-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "GuidId",
                value: new Guid("40da5f8d-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 3,
                column: "GuidId",
                value: new Guid("02aa7313-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 4,
                column: "GuidId",
                value: new Guid("3b741795-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 5,
                column: "GuidId",
                value: new Guid("c986530e-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 6,
                column: "GuidId",
                value: new Guid("2038106f-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 7,
                column: "GuidId",
                value: new Guid("3cfae5a2-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 8,
                column: "GuidId",
                value: new Guid("e92f9f37-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 9,
                column: "GuidId",
                value: new Guid("7df6faf7-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 10,
                column: "GuidId",
                value: new Guid("02f47445-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 11,
                column: "GuidId",
                value: new Guid("36421318-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 12,
                column: "GuidId",
                value: new Guid("46c4cdef-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 13,
                column: "GuidId",
                value: new Guid("56e73ab4-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 14,
                column: "GuidId",
                value: new Guid("4d04932b-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 15,
                column: "GuidId",
                value: new Guid("3cfae5a2-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 16,
                column: "GuidId",
                value: new Guid("859d1c16-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 17,
                column: "GuidId",
                value: new Guid("4965bd5b-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 18,
                column: "GuidId",
                value: new Guid("49e7c86e-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 19,
                column: "GuidId",
                value: new Guid("50c36127-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 20,
                column: "GuidId",
                value: new Guid("7e0da04d-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 21,
                column: "GuidId",
                value: new Guid("e06b6f0a-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 22,
                column: "GuidId",
                value: new Guid("b155ce82-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 23,
                column: "GuidId",
                value: new Guid("590a0955-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 24,
                column: "GuidId",
                value: new Guid("c8b746ef-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 25,
                column: "GuidId",
                value: new Guid("944d91ec-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "GuidId", "Name", "Value" },
                values: new object[] { new Guid("38ac0623-0000-0000-0000-000000000000"), "CurrencyAlreadyExists", "The currency already exists" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "GuidId", "Name", "Value" },
                values: new object[] { new Guid("af58d46e-0000-0000-0000-000000000000"), "CurrencyNotSupported", "Currency is not supported by the provider" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "GuidId", "Name", "Value" },
                values: new object[] { new Guid("13236e27-0000-0000-0000-000000000000"), "BaseCurrencyNotSupported", "The base currency symbol provided is not supported" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "GuidId", "Name", "Value" },
                values: new object[] { new Guid("7c3e4555-0000-0000-0000-000000000000"), "BaseCurrencyPriceDoesntExist", "The base currency symbols price doesn't exist" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "GuidId", "Name", "Value" },
                values: new object[] { new Guid("f63dd92b-0000-0000-0000-000000000000"), "CurrencyDoesntExist", "The currency doesn't exists" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 31,
                column: "GuidId",
                value: new Guid("002ed62b-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 32,
                column: "GuidId",
                value: new Guid("9331af71-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 33,
                column: "GuidId",
                value: new Guid("558a5b17-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 34,
                column: "GuidId",
                value: new Guid("59e42c4f-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 35,
                column: "GuidId",
                value: new Guid("15344791-0000-0000-0000-000000000000"));
        }
    }
}
