using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    public partial class codeRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestState = table.Column<int>(type: "integer", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ElementId = table.Column<int>(type: "integer", nullable: true),
                    ElementName = table.Column<string>(type: "text", nullable: true),
                    ElementTypeId = table.Column<int>(type: "integer", nullable: true),
                    ElementType = table.Column<string>(type: "text", nullable: true),
                    OutageTypeId = table.Column<int>(type: "integer", nullable: true),
                    OutageType = table.Column<string>(type: "text", nullable: true),
                    OutageTagId = table.Column<int>(type: "integer", nullable: true),
                    OutageTag = table.Column<string>(type: "text", nullable: true),
                    OutageAprovalId = table.Column<int>(type: "integer", nullable: true),
                    DesiredExecutionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeRequests_AspNetUsers_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeRequestConsents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodeRequestId = table.Column<int>(type: "integer", nullable: false),
                    StakeholderId = table.Column<string>(type: "text", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    RldcRemarks = table.Column<string>(type: "text", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "integer", nullable: false),
                    ApprovalStatusChangedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeRequestConsents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeRequestConsents_AspNetUsers_StakeholderId",
                        column: x => x.StakeholderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodeRequestConsents_CodeRequests_CodeRequestId",
                        column: x => x.CodeRequestId,
                        principalTable: "CodeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeRequestElementOwners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    OwnerName = table.Column<string>(type: "text", nullable: false),
                    CodeRequestId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeRequestElementOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeRequestElementOwners_CodeRequests_CodeRequestId",
                        column: x => x.CodeRequestId,
                        principalTable: "CodeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeRequestRemarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodeRequestId = table.Column<int>(type: "integer", nullable: false),
                    StakeholderId = table.Column<string>(type: "text", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    RldcRemarks = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeRequestRemarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeRequestRemarks_AspNetUsers_StakeholderId",
                        column: x => x.StakeholderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodeRequestRemarks_CodeRequests_CodeRequestId",
                        column: x => x.CodeRequestId,
                        principalTable: "CodeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeRequestStakeHolders",
                columns: table => new
                {
                    StakeholderId = table.Column<string>(type: "text", nullable: false),
                    CodeRequestId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeRequestStakeHolders", x => new { x.StakeholderId, x.CodeRequestId });
                    table.ForeignKey(
                        name: "FK_CodeRequestStakeHolders_AspNetUsers_StakeholderId",
                        column: x => x.StakeholderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodeRequestStakeHolders_CodeRequests_CodeRequestId",
                        column: x => x.CodeRequestId,
                        principalTable: "CodeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeRequestConsents_CodeRequestId",
                table: "CodeRequestConsents",
                column: "CodeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeRequestConsents_StakeholderId",
                table: "CodeRequestConsents",
                column: "StakeholderId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeRequestElementOwners_CodeRequestId",
                table: "CodeRequestElementOwners",
                column: "CodeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeRequestElementOwners_OwnerId_CodeRequestId",
                table: "CodeRequestElementOwners",
                columns: new[] { "OwnerId", "CodeRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeRequestRemarks_CodeRequestId",
                table: "CodeRequestRemarks",
                column: "CodeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeRequestRemarks_StakeholderId",
                table: "CodeRequestRemarks",
                column: "StakeholderId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeRequests_RequesterId",
                table: "CodeRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeRequestStakeHolders_CodeRequestId",
                table: "CodeRequestStakeHolders",
                column: "CodeRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeRequestConsents");

            migrationBuilder.DropTable(
                name: "CodeRequestElementOwners");

            migrationBuilder.DropTable(
                name: "CodeRequestRemarks");

            migrationBuilder.DropTable(
                name: "CodeRequestStakeHolders");

            migrationBuilder.DropTable(
                name: "CodeRequests");
        }
    }
}
