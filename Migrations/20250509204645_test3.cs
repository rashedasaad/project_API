using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projectAPI.Migrations
{
    /// <inheritdoc />
    public partial class test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Products_ProdectFId",
                table: "Order_Items");

            migrationBuilder.DropIndex(
                name: "IX_Order_Items_ProdectFId",
                table: "Order_Items");

            migrationBuilder.DropColumn(
                name: "ProdectFId",
                table: "Order_Items");

            migrationBuilder.RenameColumn(
                name: "Prise",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "paymentMethod",
                table: "Orders",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "Stauts",
                table: "Orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "prise",
                table: "Order_Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Prodectid",
                table: "Order_Items",
                newName: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_ProductId",
                table: "Order_Items",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Products_ProductId",
                table: "Order_Items",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Products_ProductId",
                table: "Order_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Order_Items_ProductId",
                table: "Order_Items");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "Prise");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Orders",
                newName: "paymentMethod");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "Stauts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Order_Items",
                newName: "Prodectid");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Order_Items",
                newName: "prise");

            migrationBuilder.AddColumn<int>(
                name: "ProdectFId",
                table: "Order_Items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_ProdectFId",
                table: "Order_Items",
                column: "ProdectFId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Products_ProdectFId",
                table: "Order_Items",
                column: "ProdectFId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
