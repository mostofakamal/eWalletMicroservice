using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transaction.API.Infrastructure.Migrations
{
    public partial class AddedPendingtransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 7, 6, 13, 43, 643, DateTimeKind.Utc).AddTicks(3159),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 8, 19, 7, 6, 56, 178, DateTimeKind.Utc).AddTicks(5942));

            migrationBuilder.CreateTable(
                name: "PendingTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SenderUserId = table.Column<int>(nullable: false),
                    ReceiverUserId = table.Column<int>(nullable: false),
                    TransactionTypeId = table.Column<int>(nullable: false),
                    CorrelationId = table.Column<Guid>(nullable: true),
                    ScheduledOn = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 9, 7, 6, 13, 43, 676, DateTimeKind.Utc).AddTicks(470)),
                    HandledOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingTransactions_Users_ReceiverUserId",
                        column: x => x.ReceiverUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PendingTransactions_Users_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PendingTransactions_TransactionType_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PendingTransactions_ReceiverUserId",
                table: "PendingTransactions",
                column: "ReceiverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingTransactions_SenderUserId",
                table: "PendingTransactions",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingTransactions_TransactionTypeId",
                table: "PendingTransactions",
                column: "TransactionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingTransactions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 8, 19, 7, 6, 56, 178, DateTimeKind.Utc).AddTicks(5942),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 7, 6, 13, 43, 643, DateTimeKind.Utc).AddTicks(3159));
        }
    }
}
