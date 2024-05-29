using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mshrm.Studio.Auth.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSeedData4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "041f0bcc-0000-0000-0000-000000000000");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1a8114ac-0000-0000-0000-000000000000", "8eb904d7-0000-0000-0000-000000000000" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a8114ac-0000-0000-0000-000000000000");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8eb904d7-0000-0000-0000-000000000000");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "dc56da55-0000-0000-0000-000000000000", null, "User", "USER" },
                    { "facc499b-0000-0000-0000-000000000000", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b596831d-0000-0000-0000-000000000000", 0, "6c618049-3f49-4867-9869-3af9b35d2904", "matt19sharp@gmail.com", true, true, null, "MATT19SHARP@GMAIL.COM", "MATT19SHARP@GMAIL.COM", "AQAAAAEAACcQAAAAEPpUn321ISFRDU6O/RNwe3HSkgjX9IKfBWjNtgtt0cxhy+PdD3SOzdzz3kFXpz9dAg==", null, true, "", new DateTime(2024, 3, 18, 23, 1, 12, 225, DateTimeKind.Utc).AddTicks(2110), "FQFHV4T5NUPQWD6YI4JH76LA4W5PCSZT", false, "matt19sharp@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "facc499b-0000-0000-0000-000000000000", "b596831d-0000-0000-0000-000000000000" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc56da55-0000-0000-0000-000000000000");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "facc499b-0000-0000-0000-000000000000", "b596831d-0000-0000-0000-000000000000" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "facc499b-0000-0000-0000-000000000000");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b596831d-0000-0000-0000-000000000000");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "041f0bcc-0000-0000-0000-000000000000", null, "User", "USER" },
                    { "1a8114ac-0000-0000-0000-000000000000", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8eb904d7-0000-0000-0000-000000000000", 0, "6c8c0c24-58e0-405f-b252-1e8ef47c4644", "matt19sharp@gmail.com", true, true, null, "MATT19SHARP@GMAIL.COM", "MATT19SHARP@GMAIL.COM", "AQAAAAEAACcQAAAAEPpUn321ISFRDU6O/RNwe3HSkgjX9IKfBWjNtgtt0cxhy+PdD3SOzdzz3kFXpz9dAg==", null, true, "", new DateTime(2024, 3, 18, 22, 59, 44, 317, DateTimeKind.Utc).AddTicks(6422), "FQFHV4T5NUPQWD6YI4JH76LA4W5PCSZT", false, "matt19sharp@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1a8114ac-0000-0000-0000-000000000000", "8eb904d7-0000-0000-0000-000000000000" });
        }
    }
}
