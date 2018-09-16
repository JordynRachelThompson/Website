using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyWebsite.Data.Migrations
{
    public partial class AddedtobudgetItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId6",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_BudgetItemsId6",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "BudgetItemsId6",
                table: "Entries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetItemsId6",
                table: "Entries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BudgetItemsId6",
                table: "Entries",
                column: "BudgetItemsId6");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId6",
                table: "Entries",
                column: "BudgetItemsId6",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
