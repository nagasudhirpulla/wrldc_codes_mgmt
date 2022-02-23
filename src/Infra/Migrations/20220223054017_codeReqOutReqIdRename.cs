using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    public partial class codeReqOutReqIdRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OutageApprovalId",
                table: "CodeRequests",
                newName: "OutageRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OutageRequestId",
                table: "CodeRequests",
                newName: "OutageApprovalId");
        }
    }
}
