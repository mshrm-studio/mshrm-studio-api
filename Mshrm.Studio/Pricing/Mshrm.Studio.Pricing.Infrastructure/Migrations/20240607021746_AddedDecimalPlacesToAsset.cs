using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mshrm.Studio.Pricing.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedDecimalPlacesToAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DecimalPlaces",
                schema: "dbo",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecimalPlaces",
                schema: "dbo",
                table: "Assets");
        }
    }
}
