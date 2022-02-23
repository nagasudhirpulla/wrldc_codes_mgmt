using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations;

public partial class codetype : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "CodeType",
            table: "CodeRequests",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AlterColumn<string>(
            name: "ApprovalStatus",
            table: "CodeRequestConsents",
            type: "text",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CodeType",
            table: "CodeRequests");

        migrationBuilder.AlterColumn<int>(
            name: "ApprovalStatus",
            table: "CodeRequestConsents",
            type: "integer",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");
    }
}
