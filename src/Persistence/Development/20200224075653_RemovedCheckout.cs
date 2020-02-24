using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Development
{
    public partial class RemovedCheckout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCompleted",
                table: "Reservations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasCompleted",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}