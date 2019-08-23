using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PortfolioWebsite.Migrations
{
    public partial class Deletedsometables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetTransactions");

            migrationBuilder.DropTable(
                name: "BudgetEntries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetEntries",
                columns: table => new
                {
                    BudgetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillsLimit = table.Column<float>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EntertainmentLimit = table.Column<float>(nullable: false),
                    GasLimit = table.Column<float>(nullable: false),
                    GroceryLimit = table.Column<float>(nullable: false),
                    MiscLimit = table.Column<float>(nullable: false),
                    RentLimit = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetEntries", x => x.BudgetId);
                });

            migrationBuilder.CreateTable(
                name: "BudgetTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetEntriesBudgetId = table.Column<int>(nullable: true),
                    Cost = table.Column<float>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    TypeOfBudget = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_BudgetTransactions_BudgetEntries_BudgetEntriesBudgetId",
                        column: x => x.BudgetEntriesBudgetId,
                        principalTable: "BudgetEntries",
                        principalColumn: "BudgetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTransactions_BudgetEntriesBudgetId",
                table: "BudgetTransactions",
                column: "BudgetEntriesBudgetId");
        }
    }
}
