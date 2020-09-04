using Microsoft.EntityFrameworkCore.Migrations;

namespace wallet.sagas.core.Migrations
{
    public partial class AddedFailedReasonForRewardTransactionState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionFailedReason",
                table: "RewardTransactionStateData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionFailedReason",
                table: "RewardTransactionStateData");
        }
    }
}
