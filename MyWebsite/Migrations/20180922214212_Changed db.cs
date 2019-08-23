using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PortfolioWebsite.Migrations
{
    public partial class Changeddb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Budget",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillsLimit = table.Column<float>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EntLimit = table.Column<float>(nullable: false),
                    GasLimit = table.Column<float>(nullable: false),
                    GroceryLimit = table.Column<float>(nullable: false),
                    HousingLimit = table.Column<float>(nullable: false),
                    MiscLimit = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budget", x => x.TransactionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Budget");
        }
    }
}
