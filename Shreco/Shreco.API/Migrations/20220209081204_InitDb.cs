using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shreco.API.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameIdentifer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Qrs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QrType = table.Column<int>(type: "int", nullable: false),
                    WhoCreatedId = table.Column<int>(type: "int", nullable: false),
                    ForWhoCreatedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qrs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Qrs_Users_ForWhoCreatedId",
                        column: x => x.ForWhoCreatedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Qrs_Users_WhoCreatedId",
                        column: x => x.WhoCreatedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QrId = table.Column<int>(type: "int", nullable: false),
                    WhoAppliedId = table.Column<int>(type: "int", nullable: false),
                    WhoUsedId = table.Column<int>(type: "int", nullable: false),
                    DateApplied = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Histories_Qrs_QrId",
                        column: x => x.QrId,
                        principalTable: "Qrs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Histories_Users_WhoAppliedId",
                        column: x => x.WhoAppliedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Histories_Users_WhoUsedId",
                        column: x => x.WhoUsedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Histories_QrId",
                table: "Histories",
                column: "QrId");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_WhoAppliedId",
                table: "Histories",
                column: "WhoAppliedId");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_WhoUsedId",
                table: "Histories",
                column: "WhoUsedId");

            migrationBuilder.CreateIndex(
                name: "IX_Qrs_ForWhoCreatedId",
                table: "Qrs",
                column: "ForWhoCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Qrs_WhoCreatedId",
                table: "Qrs",
                column: "WhoCreatedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.DropTable(
                name: "Qrs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
