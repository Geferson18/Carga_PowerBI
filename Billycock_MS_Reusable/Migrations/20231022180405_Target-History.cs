using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Billycock_MS_Reusable.Migrations
{
    /// <inheritdoc />
    public partial class TargetHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "pin",
                table: "USERPLATFORMACCOUNT",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AddColumn<string>(
                name: "target",
                table: "HISTORY",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "target",
                table: "HISTORY");

            migrationBuilder.AlterColumn<string>(
                name: "pin",
                table: "USERPLATFORMACCOUNT",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
