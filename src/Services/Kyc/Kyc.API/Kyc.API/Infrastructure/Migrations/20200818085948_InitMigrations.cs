using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kyc.API.Infrastructure.Migrations
{
    public partial class InitMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryGuid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    IsoCode = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

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
                    IsKycVerified = table.Column<bool>(nullable: false),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Countries",
                columns: new[] { "Id", "CountryGuid", "CurrencyCode", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 1, new Guid("dab5f416-e25a-43e4-a277-5cfe92b7ae30"), "BDT", "BD", "Bangladesh" },
                    { 2, new Guid("0b5e1eaa-bfae-44cb-8eba-752bb95ef359"), "INR", "IN", "India" },
                    { 3, new Guid("bd50407e-ec38-4cb0-bf23-f198c7d0097e"), "NOK", "NO", "Norway" }
                });

            migrationBuilder.InsertData(
                table: "KycStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Approved" },
                    { (short)2, "Pending" },
                    { (short)3, "Failed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kycs_KycStatusId",
                table: "Kycs",
                column: "KycStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Kycs_UserId",
                table: "Kycs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryId",
                table: "Users",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kycs");

            migrationBuilder.DropTable(
                name: "KycStatuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
