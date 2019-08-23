using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PortfolioWebsite.Migrations
{
    public partial class Chagedrel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetTransactions_BudgetEntries_BudgetId",
                table: "BudgetTransactions");

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_BudgetTransactions_BudgetEntries",
                table: "BudgetTransactions",
                column: "BudgetId",
                principalTable: "BudgetEntries",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_BudgetTransactions_BudgetEntries",
                table: "BudgetTransactions");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetTransactions_BudgetEntries_BudgetId",
                table: "BudgetTransactions",
                column: "BudgetId",
                principalTable: "BudgetEntries",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
