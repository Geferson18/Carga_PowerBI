using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Billycock_MS_Reusable.Migrations
{
    /// <inheritdoc />
    public partial class NuevoEsquema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PLATFORMACCOUNT_ACCOUNT_idAccount",
                table: "PLATFORMACCOUNT");

            migrationBuilder.DropForeignKey(
                name: "FK_PLATFORMACCOUNT_PLATFORM_idPlatform",
                table: "PLATFORMACCOUNT");

            migrationBuilder.DropForeignKey(
                name: "FK_USERPLATFORM_PLATFORM_idPlatform",
                table: "USERPLATFORM");

            migrationBuilder.DropForeignKey(
                name: "FK_USERPLATFORM_USER_idUser",
                table: "USERPLATFORM");

            migrationBuilder.DropForeignKey(
                name: "FK_USERPLATFORMACCOUNT_ACCOUNT_idAccount",
                table: "USERPLATFORMACCOUNT");

            migrationBuilder.DropForeignKey(
                name: "FK_USERPLATFORMACCOUNT_PLATFORM_idPlatform",
                table: "USERPLATFORMACCOUNT");

            migrationBuilder.DropForeignKey(
                name: "FK_USERPLATFORMACCOUNT_USER_idUser",
                table: "USERPLATFORMACCOUNT");

            migrationBuilder.DropIndex(
                name: "IX_USERPLATFORMACCOUNT_idAccount",
                table: "USERPLATFORMACCOUNT");

            migrationBuilder.DropIndex(
                name: "IX_USERPLATFORMACCOUNT_idPlatform",
                table: "USERPLATFORMACCOUNT");

            migrationBuilder.DropIndex(
                name: "IX_USERPLATFORM_idPlatform",
                table: "USERPLATFORM");

            migrationBuilder.DropIndex(
                name: "IX_PLATFORMACCOUNT_idAccount",
                table: "PLATFORMACCOUNT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_USERPLATFORMACCOUNT_idAccount",
                table: "USERPLATFORMACCOUNT",
                column: "idAccount");

            migrationBuilder.CreateIndex(
                name: "IX_USERPLATFORMACCOUNT_idPlatform",
                table: "USERPLATFORMACCOUNT",
                column: "idPlatform");

            migrationBuilder.CreateIndex(
                name: "IX_USERPLATFORM_idPlatform",
                table: "USERPLATFORM",
                column: "idPlatform");

            migrationBuilder.CreateIndex(
                name: "IX_PLATFORMACCOUNT_idAccount",
                table: "PLATFORMACCOUNT",
                column: "idAccount");

            migrationBuilder.AddForeignKey(
                name: "FK_PLATFORMACCOUNT_ACCOUNT_idAccount",
                table: "PLATFORMACCOUNT",
                column: "idAccount",
                principalTable: "ACCOUNT",
                principalColumn: "idAccount",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PLATFORMACCOUNT_PLATFORM_idPlatform",
                table: "PLATFORMACCOUNT",
                column: "idPlatform",
                principalTable: "PLATFORM",
                principalColumn: "idPlatform",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_USERPLATFORM_PLATFORM_idPlatform",
                table: "USERPLATFORM",
                column: "idPlatform",
                principalTable: "PLATFORM",
                principalColumn: "idPlatform",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_USERPLATFORM_USER_idUser",
                table: "USERPLATFORM",
                column: "idUser",
                principalTable: "USER",
                principalColumn: "idUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_USERPLATFORMACCOUNT_ACCOUNT_idAccount",
                table: "USERPLATFORMACCOUNT",
                column: "idAccount",
                principalTable: "ACCOUNT",
                principalColumn: "idAccount",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_USERPLATFORMACCOUNT_PLATFORM_idPlatform",
                table: "USERPLATFORMACCOUNT",
                column: "idPlatform",
                principalTable: "PLATFORM",
                principalColumn: "idPlatform",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_USERPLATFORMACCOUNT_USER_idUser",
                table: "USERPLATFORMACCOUNT",
                column: "idUser",
                principalTable: "USER",
                principalColumn: "idUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
