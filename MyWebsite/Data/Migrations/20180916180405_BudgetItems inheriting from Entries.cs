using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyWebsite.Data.Migrations
{
    public partial class BudgetItemsinheritingfromEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId1",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId2",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId3",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId4",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId5",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "BudgetItems");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Entries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Entries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId",
                table: "Entries",
                column: "BudgetItemsId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId1",
                table: "Entries",
                column: "BudgetItemsId1",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId2",
                table: "Entries",
                column: "BudgetItemsId2",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId3",
                table: "Entries",
                column: "BudgetItemsId3",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId4",
                table: "Entries",
                column: "BudgetItemsId4",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId5",
                table: "Entries",
                column: "BudgetItemsId5",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId1",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId2",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId3",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId4",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Entries_BudgetItemsId5",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Entries");

            migrationBuilder.CreateTable(
                name: "BudgetItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetItems", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId",
                table: "Entries",
                column: "BudgetItemsId",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId1",
                table: "Entries",
                column: "BudgetItemsId1",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId2",
                table: "Entries",
                column: "BudgetItemsId2",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId3",
                table: "Entries",
                column: "BudgetItemsId3",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId4",
                table: "Entries",
                column: "BudgetItemsId4",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_BudgetItems_BudgetItemsId5",
                table: "Entries",
                column: "BudgetItemsId5",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
