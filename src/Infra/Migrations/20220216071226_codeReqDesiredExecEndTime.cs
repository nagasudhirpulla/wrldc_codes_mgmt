using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    public partial class codeReqDesiredExecEndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DesiredExecutionTime",
                table: "CodeRequests",
                newName: "DesiredExecutionStartTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "DesiredExecutionEndTime",
                table: "CodeRequests",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestStatusChangedAt",
                table: "CodeRequests",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovalStatusChangedAt",
                table: "CodeRequestConsents",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesiredExecutionEndTime",
                table: "CodeRequests");

            migrationBuilder.DropColumn(
                name: "RequestStatusChangedAt",
                table: "CodeRequests");

            migrationBuilder.RenameColumn(
                name: "DesiredExecutionStartTime",
                table: "CodeRequests",
                newName: "DesiredExecutionTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovalStatusChangedAt",
                table: "CodeRequestConsents",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
