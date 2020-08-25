using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reward.API.Infrastructure.Migrations
{
    public partial class WalletUserIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RewardRule_RewardOperation_OperationId",
                table: "RewardRule");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRewards_RewardRule_RewardRuleId",
                table: "UserRewards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RewardRule",
                table: "RewardRule");

            migrationBuilder.RenameTable(
                name: "RewardRule",
                newName: "RewardRules");

            migrationBuilder.RenameIndex(
                name: "IX_RewardRule_OperationId",
                table: "RewardRules",
                newName: "IX_RewardRules_OperationId");

            migrationBuilder.AddColumn<int>(
                name: "WalletUserId",
                table: "UserRewards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RewardRules",
                table: "RewardRules",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "RewardRules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2020, 8, 25, 10, 54, 43, 801, DateTimeKind.Utc).AddTicks(8143), new DateTime(2020, 9, 25, 10, 54, 43, 801, DateTimeKind.Utc).AddTicks(9292) });

            migrationBuilder.UpdateData(
                table: "RewardRules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2020, 8, 25, 10, 54, 43, 802, DateTimeKind.Utc).AddTicks(787), new DateTime(2020, 10, 25, 10, 54, 43, 802, DateTimeKind.Utc).AddTicks(816) });

            migrationBuilder.UpdateData(
                table: "RewardRules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2020, 8, 25, 10, 54, 43, 802, DateTimeKind.Utc).AddTicks(863), new DateTime(2020, 10, 25, 10, 54, 43, 802, DateTimeKind.Utc).AddTicks(866) });

            migrationBuilder.InsertData(
                table: "TransactionType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Transfer" },
                    { 2, "BillPayment" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_WalletUserId",
                table: "UserRewards",
                column: "WalletUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RewardRules_RewardOperation_OperationId",
                table: "RewardRules",
                column: "OperationId",
                principalTable: "RewardOperation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRewards_RewardRules_RewardRuleId",
                table: "UserRewards",
                column: "RewardRuleId",
                principalTable: "RewardRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRewards_Users_WalletUserId",
                table: "UserRewards",
                column: "WalletUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RewardRules_RewardOperation_OperationId",
                table: "RewardRules");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRewards_RewardRules_RewardRuleId",
                table: "UserRewards");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRewards_Users_WalletUserId",
                table: "UserRewards");

            migrationBuilder.DropIndex(
                name: "IX_UserRewards_WalletUserId",
                table: "UserRewards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RewardRules",
                table: "RewardRules");

            migrationBuilder.DeleteData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "WalletUserId",
                table: "UserRewards");

            migrationBuilder.RenameTable(
                name: "RewardRules",
                newName: "RewardRule");

            migrationBuilder.RenameIndex(
                name: "IX_RewardRules_OperationId",
                table: "RewardRule",
                newName: "IX_RewardRule_OperationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RewardRule",
                table: "RewardRule",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "RewardRule",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2020, 8, 25, 9, 6, 10, 795, DateTimeKind.Utc).AddTicks(8966), new DateTime(2020, 9, 25, 9, 6, 10, 795, DateTimeKind.Utc).AddTicks(9490) });

            migrationBuilder.UpdateData(
                table: "RewardRule",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2020, 8, 25, 9, 6, 10, 796, DateTimeKind.Utc).AddTicks(137), new DateTime(2020, 10, 25, 9, 6, 10, 796, DateTimeKind.Utc).AddTicks(148) });

            migrationBuilder.UpdateData(
                table: "RewardRule",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2020, 8, 25, 9, 6, 10, 796, DateTimeKind.Utc).AddTicks(174), new DateTime(2020, 10, 25, 9, 6, 10, 796, DateTimeKind.Utc).AddTicks(175) });

            migrationBuilder.AddForeignKey(
                name: "FK_RewardRule_RewardOperation_OperationId",
                table: "RewardRule",
                column: "OperationId",
                principalTable: "RewardOperation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRewards_RewardRule_RewardRuleId",
                table: "UserRewards",
                column: "RewardRuleId",
                principalTable: "RewardRule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
