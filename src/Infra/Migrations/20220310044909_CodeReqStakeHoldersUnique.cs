using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations;

public partial class CodeReqStakeHoldersUnique : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_CodeRequestStakeHolders_AspNetUsers_StakeholderId",
            table: "CodeRequestStakeHolders");

        migrationBuilder.DropPrimaryKey(
            name: "PK_CodeRequestStakeHolders",
            table: "CodeRequestStakeHolders");

        migrationBuilder.AlterColumn<int>(
            name: "Id",
            table: "CodeRequestStakeHolders",
            type: "integer",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer")
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        migrationBuilder.AlterColumn<string>(
            name: "StakeholderId",
            table: "CodeRequestStakeHolders",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AddPrimaryKey(
            name: "PK_CodeRequestStakeHolders",
            table: "CodeRequestStakeHolders",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_CodeRequestStakeHolders_StakeholderId_CodeRequestId",
            table: "CodeRequestStakeHolders",
            columns: new[] { "StakeholderId", "CodeRequestId" },
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_CodeRequestStakeHolders_AspNetUsers_StakeholderId",
            table: "CodeRequestStakeHolders",
            column: "StakeholderId",
            principalTable: "AspNetUsers",
            principalColumn: "Id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_CodeRequestStakeHolders_AspNetUsers_StakeholderId",
            table: "CodeRequestStakeHolders");

        migrationBuilder.DropPrimaryKey(
            name: "PK_CodeRequestStakeHolders",
            table: "CodeRequestStakeHolders");

        migrationBuilder.DropIndex(
            name: "IX_CodeRequestStakeHolders_StakeholderId_CodeRequestId",
            table: "CodeRequestStakeHolders");

        migrationBuilder.AlterColumn<string>(
            name: "StakeholderId",
            table: "CodeRequestStakeHolders",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "Id",
            table: "CodeRequestStakeHolders",
            type: "integer",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer")
            .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        migrationBuilder.AddPrimaryKey(
            name: "PK_CodeRequestStakeHolders",
            table: "CodeRequestStakeHolders",
            columns: new[] { "StakeholderId", "CodeRequestId" });

        migrationBuilder.AddForeignKey(
            name: "FK_CodeRequestStakeHolders_AspNetUsers_StakeholderId",
            table: "CodeRequestStakeHolders",
            column: "StakeholderId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
