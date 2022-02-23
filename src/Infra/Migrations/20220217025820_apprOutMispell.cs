using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations;

public partial class apprOutMispell : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "OutageAprovalId",
            table: "CodeRequests",
            newName: "OutageApprovalId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "OutageApprovalId",
            table: "CodeRequests",
            newName: "OutageAprovalId");
    }
}
