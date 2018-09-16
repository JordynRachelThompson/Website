using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyWebsite.Data.Migrations
{
    public partial class Consoliteddb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetLimit");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.CreateTable(
                name: "BudgetEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillsLimit = table.Column<int>(nullable: false),
                    Cost = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EntertainmentLimit = table.Column<int>(nullable: false),
                    GasLimit = table.Column<int>(nullable: false),
                    GroceryLimit = table.Column<int>(nullable: false),
                    MiscLimit = table.Column<int>(nullable: false),
                    RentLimit = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetEntries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetEntries");

            migrationBuilder.CreateTable(
                name: "BudgetLimit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillsLimit = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EntertainmentLimit = table.Column<int>(nullable: false),
                    GasLimit = table.Column<int>(nullable: false),
                    GroceryLimit = table.Column<int>(nullable: false),
                    MiscLimit = table.Column<int>(nullable: false),
                    RentLimit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLimit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetItemsId = table.Column<int>(nullable: true),
                    BudgetItemsId1 = table.Column<int>(nullable: true),
                    BudgetItemsId2 = table.Column<int>(nullable: true),
                    BudgetItemsId3 = table.Column<int>(nullable: true),
                    BudgetItemsId4 = table.Column<int>(nullable: true),
                    BudgetItemsId5 = table.Column<int>(nullable: true),
                    Cost = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entries_Entries_BudgetItemsId",
                        column: x => x.BudgetItemsId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_Entries_BudgetItemsId1",
                        column: x => x.BudgetItemsId1,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_Entries_BudgetItemsId2",
                        column: x => x.BudgetItemsId2,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_Entries_BudgetItemsId3",
                        column: x => x.BudgetItemsId3,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_Entries_BudgetItemsId4",
                        column: x => x.BudgetItemsId4,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_Entries_BudgetItemsId5",
                        column: x => x.BudgetItemsId5,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BudgetItemsId",
                table: "Entries",
                column: "BudgetItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BudgetItemsId1",
                table: "Entries",
                column: "BudgetItemsId1");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BudgetItemsId2",
                table: "Entries",
                column: "BudgetItemsId2");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BudgetItemsId3",
                table: "Entries",
                column: "BudgetItemsId3");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BudgetItemsId4",
                table: "Entries",
                column: "BudgetItemsId4");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BudgetItemsId5",
                table: "Entries",
                column: "BudgetItemsId5");
        }
    }
}
