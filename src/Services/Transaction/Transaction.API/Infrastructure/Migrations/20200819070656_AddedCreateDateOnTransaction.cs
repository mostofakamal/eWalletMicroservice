using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transaction.API.Infrastructure.Migrations
{
    public partial class AddedCreateDateOnTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(2020, 8, 19, 7, 6, 56, 178, DateTimeKind.Utc).AddTicks(5942));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Transactions");
        }
    }
}
