using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kyc.API.Infrastructure.Migrations
{
    public partial class KycCreatedDateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kycs_Users_UserId",
                table: "Kycs");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Kycs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1,
                column: "CountryGuid",
                value: new Guid("394a3be9-bd16-4daa-9642-0bebb70fe8e6"));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2,
                column: "CountryGuid",
                value: new Guid("7d40a97f-409e-4155-a2fc-fcb574d1f787"));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3,
                column: "CountryGuid",
                value: new Guid("d45c4ff8-98bf-4186-8bba-ffc7dafb471b"));

            migrationBuilder.AddForeignKey(
                name: "FK_Kycs_Users_UserId",
                table: "Kycs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kycs_Users_UserId",
                table: "Kycs");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Kycs");

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1,
                column: "CountryGuid",
                value: new Guid("dab5f416-e25a-43e4-a277-5cfe92b7ae30"));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2,
                column: "CountryGuid",
                value: new Guid("0b5e1eaa-bfae-44cb-8eba-752bb95ef359"));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3,
                column: "CountryGuid",
                value: new Guid("bd50407e-ec38-4cb0-bf23-f198c7d0097e"));

            migrationBuilder.AddForeignKey(
                name: "FK_Kycs_Users_UserId",
                table: "Kycs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
