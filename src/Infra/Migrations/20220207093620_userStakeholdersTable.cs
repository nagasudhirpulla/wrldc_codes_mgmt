using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    public partial class userStakeholdersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStakeholder_AspNetUsers_UsrId",
                table: "UserStakeholder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStakeholder",
                table: "UserStakeholder");

            migrationBuilder.RenameTable(
                name: "UserStakeholder",
                newName: "UserStakeholders");

            migrationBuilder.RenameIndex(
                name: "IX_UserStakeholder_UsrId",
                table: "UserStakeholders",
                newName: "IX_UserStakeholders_UsrId");

            migrationBuilder.RenameIndex(
                name: "IX_UserStakeholder_StakeHolderId_UsrId",
                table: "UserStakeholders",
                newName: "IX_UserStakeholders_StakeHolderId_UsrId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStakeholders",
                table: "UserStakeholders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStakeholders_AspNetUsers_UsrId",
                table: "UserStakeholders",
                column: "UsrId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStakeholders_AspNetUsers_UsrId",
                table: "UserStakeholders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStakeholders",
                table: "UserStakeholders");

            migrationBuilder.RenameTable(
                name: "UserStakeholders",
                newName: "UserStakeholder");

            migrationBuilder.RenameIndex(
                name: "IX_UserStakeholders_UsrId",
                table: "UserStakeholder",
                newName: "IX_UserStakeholder_UsrId");

            migrationBuilder.RenameIndex(
                name: "IX_UserStakeholders_StakeHolderId_UsrId",
                table: "UserStakeholder",
                newName: "IX_UserStakeholder_StakeHolderId_UsrId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStakeholder",
                table: "UserStakeholder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStakeholder_AspNetUsers_UsrId",
                table: "UserStakeholder",
                column: "UsrId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
