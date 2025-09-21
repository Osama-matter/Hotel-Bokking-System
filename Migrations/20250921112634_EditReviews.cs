using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_Bokking_System.Migrations
{
    /// <inheritdoc />
    public partial class EditReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Customers_CustomarID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_CustomarID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CustomarID",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "Userid",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Userid",
                table: "Reviews",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_Userid",
                table: "Reviews",
                column: "Userid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_Userid",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_Userid",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "CustomarID",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomarID",
                table: "Reviews",
                column: "CustomarID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Customers_CustomarID",
                table: "Reviews",
                column: "CustomarID",
                principalTable: "Customers",
                principalColumn: "CustomarID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
