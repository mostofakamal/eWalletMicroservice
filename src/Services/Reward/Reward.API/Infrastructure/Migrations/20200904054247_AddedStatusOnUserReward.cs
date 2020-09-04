using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reward.API.Infrastructure.Migrations
{
    public partial class AddedStatusOnUserReward : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RewardGuid",
                table: "UserRewards",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "UserRewards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserRewardStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRewardStatus", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "RewardRules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2020, 9, 4, 5, 42, 46, 748, DateTimeKind.Utc).AddTicks(8153), new DateTime(2020, 10, 4, 5, 42, 46, 748, DateTimeKind.Utc).AddTicks(9090) });

            migrationBuilder.UpdateData(
                table: "RewardRules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2020, 9, 4, 5, 42, 46, 748, DateTimeKind.Utc).AddTicks(9996), new DateTime(2020, 11, 4, 5, 42, 46, 749, DateTimeKind.Utc).AddTicks(7) });

            migrationBuilder.UpdateData(
                table: "RewardRules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2020, 9, 4, 5, 42, 46, 749, DateTimeKind.Utc).AddTicks(27), new DateTime(2020, 11, 4, 5, 42, 46, 749, DateTimeKind.Utc).AddTicks(28) });

            migrationBuilder.InsertData(
                table: "UserRewardStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "PaidOut" },
                    { 3, "TransactionFailed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_StatusId",
                table: "UserRewards",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRewards_UserRewardStatus_StatusId",
                table: "UserRewards",
                column: "StatusId",
                principalTable: "UserRewardStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRewards_UserRewardStatus_StatusId",
                table: "UserRewards");

            migrationBuilder.DropTable(
                name: "UserRewardStatus");

            migrationBuilder.DropIndex(
                name: "IX_UserRewards_StatusId",
                table: "UserRewards");

            migrationBuilder.DropColumn(
                name: "RewardGuid",
                table: "UserRewards");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "UserRewards");

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
        }
    }
}
