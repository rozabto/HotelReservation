using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Development
{
    public partial class ReaddingCheckout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasCompleted",
                table: "Reservations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCompleted",
                table: "Reservations");
        }
    }
}
