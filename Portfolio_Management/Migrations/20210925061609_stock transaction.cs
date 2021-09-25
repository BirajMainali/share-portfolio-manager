﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Portfolio_Management.Migrations
{
    public partial class stocktransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Stock");

            migrationBuilder.EnsureSchema(
                name: "stock");

            migrationBuilder.CreateTable(
                name: "stock",
                schema: "Stock",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StockName = table.Column<string>(type: "text", nullable: true),
                    Prefix = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    OpeningRate = table.Column<decimal>(type: "numeric", nullable: false),
                    ClosingRate = table.Column<decimal>(type: "numeric", nullable: true),
                    AuditLog = table.Column<string>(type: "text", nullable: true),
                    RecDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stock_transaction",
                schema: "stock",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AuditLog = table.Column<string>(type: "text", nullable: true),
                    RecDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stock_transaction_stock_StockId",
                        column: x => x.StockId,
                        principalSchema: "Stock",
                        principalTable: "stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_stock_transaction_StockId",
                schema: "stock",
                table: "stock_transaction",
                column: "StockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stock_transaction",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "stock",
                schema: "Stock");
        }
    }
}
