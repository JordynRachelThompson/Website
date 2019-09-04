using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PortfolioWebsite.Migrations
{
    public partial class initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlertsEmail",
                table: "Weather",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BookWeatherDay",
                table: "Weather",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GoodWeatherDay",
                table: "Weather",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RainyDay",
                table: "Weather",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SevereWeatherDay",
                table: "Weather",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SnowyDay",
                table: "Weather",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlertsEmail",
                table: "Weather");

            migrationBuilder.DropColumn(
                name: "BookWeatherDay",
                table: "Weather");

            migrationBuilder.DropColumn(
                name: "GoodWeatherDay",
                table: "Weather");

            migrationBuilder.DropColumn(
                name: "RainyDay",
                table: "Weather");

            migrationBuilder.DropColumn(
                name: "SevereWeatherDay",
                table: "Weather");

            migrationBuilder.DropColumn(
                name: "SnowyDay",
                table: "Weather");
        }
    }
}
