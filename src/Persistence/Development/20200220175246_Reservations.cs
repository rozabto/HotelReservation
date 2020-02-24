using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Development
{
    public partial class Reservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_ReservedByUserId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ReservedByUserId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservedByUserId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "SessionToken",
                table: "Reservations");

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

            migrationBuilder.AddColumn<string>(
                name: "ReservedByUserId",
                table: "Reservations",
                type: "char(32)",
                unicode: false,
                fixedLength: true,
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SessionToken",
                table: "Reservations",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservedByUserId",
                table: "Reservations",
                column: "ReservedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_ReservedByUserId",
                table: "Reservations",
                column: "ReservedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}