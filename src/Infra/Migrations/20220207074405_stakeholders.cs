using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    public partial class stakeholders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserStakeholder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StakeHolderId = table.Column<int>(type: "integer", nullable: false),
                    StakeHolderName = table.Column<string>(type: "text", nullable: false),
                    UsrId = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStakeholder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStakeholder_AspNetUsers_UsrId",
                        column: x => x.UsrId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStakeholder_StakeHolderId_UsrId",
                table: "UserStakeholder",
                columns: new[] { "StakeHolderId", "UsrId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStakeholder_UsrId",
                table: "UserStakeholder",
                column: "UsrId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStakeholder");
        }
    }
}
