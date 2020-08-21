using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transaction.API.Infrastructure.IntegrationEventLogMigrations
{
    public partial class AddedIntegrationDataLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegrationDataLogs",
                columns: table => new
                {
                    IntegrationDataId = table.Column<Guid>(nullable: false),
                    DataTypeName = table.Column<string>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    IntegrationDataType = table.Column<int>(nullable: false),
                    TimesSent = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    TransactionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationDataLogs", x => x.IntegrationDataId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegrationDataLogs");
        }
    }
}
