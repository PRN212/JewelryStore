using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class seedSellOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Name", "Phone" },
                values: new object[] { 1, "3 Nam Ky Khoi Nghia", "John Doe", "0123456789" });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedDate", "CustomerId", "PaymentMethod", "Status", "TotalPrice", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 10, 18, 38, 55, 940, DateTimeKind.Local).AddTicks(6462), 1, "CreditCard", "Pending", 1000m, "Sell", 1 },
                    { 2, new DateTime(2024, 7, 10, 18, 38, 55, 940, DateTimeKind.Local).AddTicks(6474), 1, "Cash", "Completed", 2000m, "Sell", 1 },
                    { 3, new DateTime(2024, 7, 10, 18, 38, 55, 940, DateTimeKind.Local).AddTicks(6476), 1, "Cash", "Shipped", 3000m, "Sell", 1 }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderId", "ProductId", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 500m, 2 },
                    { 2, 2, 700m, 3 },
                    { 3, 3, 900m, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
