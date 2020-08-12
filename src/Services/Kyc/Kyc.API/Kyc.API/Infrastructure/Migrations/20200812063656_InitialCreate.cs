using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kyc.API.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KycStatuses",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KycStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsKycVerified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kycs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NID = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    KycStatusId = table.Column<short>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kycs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kycs_KycStatuses_KycStatusId",
                        column: x => x.KycStatusId,
                        principalTable: "KycStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kycs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "KycStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { (short)1, "Approved" });

            migrationBuilder.InsertData(
                table: "KycStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { (short)2, "Pending" });

            migrationBuilder.InsertData(
                table: "KycStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { (short)3, "Failed" });

            migrationBuilder.CreateIndex(
                name: "IX_Kycs_KycStatusId",
                table: "Kycs",
                column: "KycStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Kycs_UserId",
                table: "Kycs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kycs");

            migrationBuilder.DropTable(
                name: "KycStatuses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
