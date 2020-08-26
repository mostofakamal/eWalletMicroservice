using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reward.API.Infrastructure.Migrations
{
    public partial class TransactionEligibleAndIsCountryAdminColAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCountryAdmin",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTransactionEligible",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "RewardRule",
                columns: new[] { "Id", "Amount", "IsEnabled", "OperationId", "RequiredMinOccurance", "ValidFrom", "ValidTo" },
                values: new object[] { 1, 2m, true, 1, 1, new DateTime(2020, 8, 25, 9, 6, 10, 795, DateTimeKind.Utc).AddTicks(8966), new DateTime(2020, 9, 25, 9, 6, 10, 795, DateTimeKind.Utc).AddTicks(9490) });

            migrationBuilder.InsertData(
                table: "RewardRule",
                columns: new[] { "Id", "Amount", "IsEnabled", "OperationId", "RequiredMinOccurance", "ValidFrom", "ValidTo" },
                values: new object[] { 2, 1m, true, 2, 2, new DateTime(2020, 8, 25, 9, 6, 10, 796, DateTimeKind.Utc).AddTicks(137), new DateTime(2020, 10, 25, 9, 6, 10, 796, DateTimeKind.Utc).AddTicks(148) });

            migrationBuilder.InsertData(
                table: "RewardRule",
                columns: new[] { "Id", "Amount", "IsEnabled", "OperationId", "RequiredMinOccurance", "ValidFrom", "ValidTo" },
                values: new object[] { 3, 2m, true, 3, 1, new DateTime(2020, 8, 25, 9, 6, 10, 796, DateTimeKind.Utc).AddTicks(174), new DateTime(2020, 10, 25, 9, 6, 10, 796, DateTimeKind.Utc).AddTicks(175) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RewardRule",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RewardRule",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RewardRule",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "IsCountryAdmin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsTransactionEligible",
                table: "Users");
        }
    }
}
