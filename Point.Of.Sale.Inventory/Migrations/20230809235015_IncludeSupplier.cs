using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Of.Sale.Inventory.Migrations
{
    /// <inheritdoc />
    public partial class IncludeSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Inventories");
        }
    }
}
