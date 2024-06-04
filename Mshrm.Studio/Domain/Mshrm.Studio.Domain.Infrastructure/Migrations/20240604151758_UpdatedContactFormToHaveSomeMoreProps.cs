using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mshrm.Studio.Domain.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedContactFormToHaveSomeMoreProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentGuidIds",
                schema: "dbo",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "dbo",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                schema: "dbo",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "GuidId",
                value: new Guid("1f8f6880-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentGuidIds",
                schema: "dbo",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "dbo",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "dbo",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                schema: "dbo",
                table: "ContactForms");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "GuidId",
                value: new Guid("e8d8917b-0000-0000-0000-000000000000"));
        }
    }
}
