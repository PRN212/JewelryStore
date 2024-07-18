using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                columns: new[] { "OrderId", "ProductId" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Name", "Phone" },
                values: new object[] { 1, "3 Nam Ky Khoi Nghia", "John Doe", "0123456789" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Dob", "Email", "Gender", "ImgUrl", "Name", "Password", "Phone", "Role", "Status", "Username" },
                values: new object[] { -1, new DateOnly(2000, 1, 1), "<email>", "M", null, "<name>", "1", "1", "Manager", true, "1" });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedDate", "CustomerId", "PaymentMethod", "Status", "TotalPrice", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 10, 18, 53, 33, 294, DateTimeKind.Local), 1, "Credit Card", "Pending", 1000m, "Sell", 1 },
                    { 2, new DateTime(2024, 7, 10, 18, 53, 33, 294, DateTimeKind.Local), 1, "Cash", "Completed", 2000m, "Sell", 1 },
                    { 3, new DateTime(2024, 7, 10, 18, 53, 33, 294, DateTimeKind.Local), 1, "Cash", "Shipped", 3000m, "Sell", 1 }
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

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails");

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
                table: "Users",
                keyColumn: "Id",
                keyValue: -1);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                columns: new[] { "ProductId", "OrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");
        }
    }
}
