using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexTradeHub.Migrations
{
    /// <inheritdoc />
    public partial class TimeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TradeOrders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TradeOrders");
        }
    }
}
