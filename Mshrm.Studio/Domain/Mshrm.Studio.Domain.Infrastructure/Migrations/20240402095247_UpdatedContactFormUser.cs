using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mshrm.Studio.Domain.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedContactFormUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactForms_Users_UserId",
                schema: "dbo",
                table: "ContactForms");

            migrationBuilder.DropIndex(
                name: "IX_ContactForms_GuidId_UserId",
                schema: "dbo",
                table: "ContactForms");

            migrationBuilder.DropIndex(
                name: "IX_ContactForms_UserId",
                schema: "dbo",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "ContactForms");

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail",
                schema: "dbo",
                table: "ContactForms",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "GuidId",
                value: new Guid("e8d8917b-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ContactForms_GuidId_ContactEmail",
                schema: "dbo",
                table: "ContactForms",
                columns: new[] { "GuidId", "ContactEmail" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContactForms_GuidId_ContactEmail",
                schema: "dbo",
                table: "ContactForms");

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail",
                schema: "dbo",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "dbo",
                table: "ContactForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "GuidId",
                value: new Guid("74aa7f66-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ContactForms_GuidId_UserId",
                schema: "dbo",
                table: "ContactForms",
                columns: new[] { "GuidId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactForms_UserId",
                schema: "dbo",
                table: "ContactForms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactForms_Users_UserId",
                schema: "dbo",
                table: "ContactForms",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
