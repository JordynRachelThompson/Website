using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyWebsite.Data.Migrations
{
    public partial class Addedclassesbudgetlimitandbudgetitems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetItemsId = table.Column<int>(nullable: true),
                    BudgetItemsId1 = table.Column<int>(nullable: true),
                    BudgetItemsId2 = table.Column<int>(nullable: true),
                    BudgetItemsId3 = table.Column<int>(nullable: true),
                    BudgetItemsId4 = table.Column<int>(nullable: true),
                    BudgetItemsId5 = table.Column<int>(nullable: true),
                    BudgetItemsId6 = table.Column<int>(nullable: true),
                    Cost = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entries_BudgetItems_BudgetItemsId",
                        column: x => x.BudgetItemsId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_BudgetItems_BudgetItemsId1",
                        column: x => x.BudgetItemsId1,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_BudgetItems_BudgetItemsId2",
                        column: x => x.BudgetItemsId2,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_BudgetItems_BudgetItemsId3",
                        column: x => x.BudgetItemsId3,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_BudgetItems_BudgetItemsId4",
                        column: x => x.BudgetItemsId4,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_BudgetItems_BudgetItemsId5",
                        column: x => x.BudgetItemsId5,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_BudgetItems_BudgetItemsId6",
                        column: x => x.BudgetItemsId6,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BudgetItemsId6",
                table: "Entries",
                column: "BudgetItemsId6");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BudgetLimit");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "BudgetItems");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
