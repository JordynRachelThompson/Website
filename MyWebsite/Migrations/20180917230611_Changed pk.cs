using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PortfolioWebsite.Migrations
{
    public partial class Changedpk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetTransactions_BudgetEntries_BudgetEntriesBudgetId",
                table: "BudgetTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BudgetTransactions_BudgetEntriesBudgetId",
                table: "BudgetTransactions");

            migrationBuilder.DropColumn(
                name: "BudgetEntriesBudgetId",
                table: "BudgetTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTransactions_BudgetRefId",
                table: "BudgetTransactions",
                column: "BudgetRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetTransactions_BudgetEntries_BudgetRefId",
                table: "BudgetTransactions",
                column: "BudgetRefId",
                principalTable: "BudgetEntries",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetTransactions_BudgetEntries_BudgetRefId",
                table: "BudgetTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BudgetTransactions_BudgetRefId",
                table: "BudgetTransactions");

            migrationBuilder.AddColumn<int>(
                name: "BudgetEntriesBudgetId",
                table: "BudgetTransactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTransactions_BudgetEntriesBudgetId",
                table: "BudgetTransactions",
                column: "BudgetEntriesBudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetTransactions_BudgetEntries_BudgetEntriesBudgetId",
                table: "BudgetTransactions",
                column: "BudgetEntriesBudgetId",
                principalTable: "BudgetEntries",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
