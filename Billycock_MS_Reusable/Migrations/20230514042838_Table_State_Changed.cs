using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Billycock_MS_Reusable.Migrations
{
    /// <inheritdoc />
    public partial class Table_State_Changed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACCOUNT",
                columns: table => new
                {
                    idAccount = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "varchar(100)", nullable: false),
                    diminutive = table.Column<string>(type: "varchar(10)", nullable: false),
                    idState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCOUNT", x => x.idAccount);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT",
                columns: table => new
                {
                    idAudit = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Input = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Output = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    AuditMethod = table.Column<string>(type: "varchar(50)", nullable: false),
                    date = table.Column<string>(type: "varchar(30)", nullable: false),
                    integration = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT", x => x.idAudit);
                });

            migrationBuilder.CreateTable(
                name: "CORRELATIVE",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    idPlatformAccount = table.Column<int>(type: "int", nullable: false),
                    idUserPlatform = table.Column<int>(type: "int", nullable: false),
                    idUserPlatformAccount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORRELATIVE", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "HISTORY",
                columns: table => new
                {
                    idHistory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Request = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Response = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    date = table.Column<string>(type: "varchar(50)", nullable: false),
                    integration = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HISTORY", x => x.idHistory);
                });

            migrationBuilder.CreateTable(
                name: "PLATFORM",
                columns: table => new
                {
                    idPlatform = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "varchar(50)", nullable: false),
                    numberMaximumUsers = table.Column<int>(type: "int", nullable: false),
                    cost = table.Column<int>(type: "int", nullable: false),
                    lowPrice = table.Column<int>(type: "int", nullable: false),
                    highPrice = table.Column<int>(type: "int", nullable: false),
                    idState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLATFORM", x => x.idPlatform);
                });

            migrationBuilder.CreateTable(
                name: "STATE",
                columns: table => new
                {
                    idState = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATE", x => x.idState);
                });

            migrationBuilder.CreateTable(
                name: "TOKENUSER",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accessFailedCount = table.Column<int>(type: "int", nullable: false),
                    lockoutEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOKENUSER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    idUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "varchar(100)", nullable: false),
                    inscriptionDate = table.Column<string>(type: "varchar(30)", nullable: false),
                    idState = table.Column<int>(type: "int", nullable: false),
                    billing = table.Column<string>(type: "varchar(30)", nullable: false),
                    pay = table.Column<int>(type: "int", nullable: false),
                    contact = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.idUser);
                });

            migrationBuilder.CreateTable(
                name: "PLATFORMACCOUNT",
                columns: table => new
                {
                    idPlatform = table.Column<int>(type: "int", nullable: false),
                    idAccount = table.Column<int>(type: "int", nullable: false),
                    GuiID = table.Column<int>(type: "int", nullable: false),
                    freeUsers = table.Column<int>(type: "int", nullable: false),
                    payDate = table.Column<string>(type: "varchar(20)", nullable: false),
                    password = table.Column<string>(type: "varchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLATFORMACCOUNT", x => new { x.idPlatform, x.idAccount });
                    table.ForeignKey(
                        name: "FK_PLATFORMACCOUNT_ACCOUNT_idAccount",
                        column: x => x.idAccount,
                        principalTable: "ACCOUNT",
                        principalColumn: "idAccount",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PLATFORMACCOUNT_PLATFORM_idPlatform",
                        column: x => x.idPlatform,
                        principalTable: "PLATFORM",
                        principalColumn: "idPlatform",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USERPLATFORM",
                columns: table => new
                {
                    idUser = table.Column<int>(type: "int", nullable: false),
                    idPlatform = table.Column<int>(type: "int", nullable: false),
                    GuiID = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERPLATFORM", x => new { x.idUser, x.idPlatform });
                    table.ForeignKey(
                        name: "FK_USERPLATFORM_PLATFORM_idPlatform",
                        column: x => x.idPlatform,
                        principalTable: "PLATFORM",
                        principalColumn: "idPlatform",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USERPLATFORM_USER_idUser",
                        column: x => x.idUser,
                        principalTable: "USER",
                        principalColumn: "idUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USERPLATFORMACCOUNT",
                columns: table => new
                {
                    idUser = table.Column<int>(type: "int", nullable: false),
                    idPlatform = table.Column<int>(type: "int", nullable: false),
                    idAccount = table.Column<int>(type: "int", nullable: false),
                    GuiID = table.Column<int>(type: "int", nullable: false),
                    pin = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERPLATFORMACCOUNT", x => new { x.idUser, x.idPlatform, x.idAccount, x.GuiID });
                    table.ForeignKey(
                        name: "FK_USERPLATFORMACCOUNT_ACCOUNT_idAccount",
                        column: x => x.idAccount,
                        principalTable: "ACCOUNT",
                        principalColumn: "idAccount",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USERPLATFORMACCOUNT_PLATFORM_idPlatform",
                        column: x => x.idPlatform,
                        principalTable: "PLATFORM",
                        principalColumn: "idPlatform",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USERPLATFORMACCOUNT_USER_idUser",
                        column: x => x.idUser,
                        principalTable: "USER",
                        principalColumn: "idUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PLATFORMACCOUNT_idAccount",
                table: "PLATFORMACCOUNT",
                column: "idAccount");

            migrationBuilder.CreateIndex(
                name: "IX_USERPLATFORM_idPlatform",
                table: "USERPLATFORM",
                column: "idPlatform");

            migrationBuilder.CreateIndex(
                name: "IX_USERPLATFORMACCOUNT_idAccount",
                table: "USERPLATFORMACCOUNT",
                column: "idAccount");

            migrationBuilder.CreateIndex(
                name: "IX_USERPLATFORMACCOUNT_idPlatform",
                table: "USERPLATFORMACCOUNT",
                column: "idPlatform");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDIT");

            migrationBuilder.DropTable(
                name: "CORRELATIVE");

            migrationBuilder.DropTable(
                name: "HISTORY");

            migrationBuilder.DropTable(
                name: "PLATFORMACCOUNT");

            migrationBuilder.DropTable(
                name: "STATE");

            migrationBuilder.DropTable(
                name: "TOKENUSER");

            migrationBuilder.DropTable(
                name: "USERPLATFORM");

            migrationBuilder.DropTable(
                name: "USERPLATFORMACCOUNT");

            migrationBuilder.DropTable(
                name: "ACCOUNT");

            migrationBuilder.DropTable(
                name: "PLATFORM");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
