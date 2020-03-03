using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Production
{
    public partial class AuditableEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_EditedById",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_HotelRooms_EditedById",
                table: "HotelRooms");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EditedById",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_EditedById",
                table: "Reservations",
                column: "EditedById");

            migrationBuilder.CreateIndex(
                name: "IX_HotelRooms_EditedById",
                table: "HotelRooms",
                column: "EditedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EditedById",
                table: "Employees",
                column: "EditedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_EditedById",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_HotelRooms_EditedById",
                table: "HotelRooms");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EditedById",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_EditedById",
                table: "Reservations",
                column: "EditedById",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelRooms_EditedById",
                table: "HotelRooms",
                column: "EditedById",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees",
                column: "CreatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EditedById",
                table: "Employees",
                column: "EditedById",
                unique: true);
        }
    }
}
