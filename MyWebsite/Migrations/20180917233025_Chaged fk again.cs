using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyWebsite.Migrations
{
    public partial class Chagedfkagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetTransactions_BudgetEntries_BudgetRefId",
                table: "BudgetTransactions");

            migrationBuilder.RenameColumn(
                name: "BudgetRefId",
                table: "BudgetTransactions",
                newName: "BudgetId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetTransactions_BudgetRefId",
                table: "BudgetTransactions",
                newName: "IX_BudgetTransactions_BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetTransactions_BudgetEntries_BudgetId",
                table: "BudgetTransactions",
                column: "BudgetId",
                principalTable: "BudgetEntries",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetTransactions_BudgetEntries_BudgetId",
                table: "BudgetTransactions");

            migrationBuilder.RenameColumn(
                name: "BudgetId",
                table: "BudgetTransactions",
                newName: "BudgetRefId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetTransactions_BudgetId",
                table: "BudgetTransactions",
                newName: "IX_BudgetTransactions_BudgetRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetTransactions_BudgetEntries_BudgetRefId",
                table: "BudgetTransactions",
                column: "BudgetRefId",
                principalTable: "BudgetEntries",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
