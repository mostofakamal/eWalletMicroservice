using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace wallet.sagas.core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RewardTransactionStateData",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(nullable: false),
                    CurrentState = table.Column<string>(maxLength: 80, nullable: true),
                    RequestStartedOn = table.Column<DateTime>(nullable: false),
                    RewardSenderWalletUserId = table.Column<Guid>(nullable: false),
                    RewardReceiverUserId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    RequestCancelledOn = table.Column<DateTime>(nullable: true),
                    RequestCompletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardTransactionStateData", x => x.CorrelationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RewardTransactionStateData");
        }
    }
}
