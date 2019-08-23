using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PortfolioWebsite.Migrations
{
    public partial class Addedemailtotransactiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_BudgetTransactions_BudgetEntries",
                table: "BudgetTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BudgetTransactions_BudgetId",
                table: "BudgetTransactions");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "BudgetTransactions");

            migrationBuilder.AddColumn<int>(
                name: "BudgetEntriesBudgetId",
                table: "BudgetTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Email",
                table: "BudgetTransactions");

            migrationBuilder.AddColumn<int>(
                name: "BudgetId",
                table: "BudgetTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTransactions_BudgetId",
                table: "BudgetTransactions",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_BudgetTransactions_BudgetEntries",
                table: "BudgetTransactions",
                column: "BudgetId",
                principalTable: "BudgetEntries",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
