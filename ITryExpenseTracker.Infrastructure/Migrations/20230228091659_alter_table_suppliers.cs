using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITryExpenseTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class altertablesuppliers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Suppliers",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ApplicationUserId",
                table: "Suppliers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AspNetUsers_ApplicationUserId",
                table: "Suppliers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_ApplicationUserId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_ApplicationUserId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Suppliers");
        }
    }
}
