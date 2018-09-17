using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyWebsite.Migrations
{
    public partial class InitialAddedKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "BudgetEntries");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BudgetEntries");

            migrationBuilder.DropColumn(
                name: "TypeOfBudget",
                table: "BudgetEntries");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BudgetEntries",
                newName: "BudgetId");

            migrationBuilder.CreateTable(
                name: "BudgetTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetEntriesBudgetId = table.Column<int>(nullable: true),
                    BudgetRefId = table.Column<int>(nullable: false),
                    Cost = table.Column<float>(nullable: false),
                    Description = table.Column<string>(nullable: true),
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetTransactions");

            migrationBuilder.RenameColumn(
                name: "BudgetId",
                table: "BudgetEntries",
                newName: "Id");

            migrationBuilder.AddColumn<float>(
                name: "Cost",
                table: "BudgetEntries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BudgetEntries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeOfBudget",
                table: "BudgetEntries",
                nullable: false,
                defaultValue: 0);
        }
    }
}
