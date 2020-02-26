using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Development
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    NormalizedName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ConcurrencyStamp = table.Column<string>(unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    UserName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    NormalizedUserName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 256, nullable: false),
                    SecurityStamp = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    ConcurrencyStamp = table.Column<string>(fixedLength: true, maxLength: 36, nullable: false),
                    PhoneNumber = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    IsAdult = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    ClaimType = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    ClaimValue = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    ClaimType = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    ClaimValue = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    UserId = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    RoleId = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    CreatedById = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EditedById = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: true),
                    EditedOn = table.Column<DateTime>(nullable: true),
                    MiddleName = table.Column<string>(maxLength: 100, nullable: false),
                    EGN = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_EditedById",
                        column: x => x.EditedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HotelRooms",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    CreatedById = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EditedById = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: true),
                    EditedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(nullable: false),
                    RoomType = table.Column<int>(nullable: false),
                    IsEmpty = table.Column<bool>(nullable: false),
                    PriceForAdults = table.Column<decimal>(nullable: true),
                    PriceForChildren = table.Column<decimal>(nullable: true),
                    RoomPrice = table.Column<decimal>(nullable: true),
                    FoodPrice = table.Column<decimal>(nullable: false),
                    RoomNumber = table.Column<int>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelRooms_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HotelRooms_AspNetUsers_EditedById",
                        column: x => x.EditedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HotelRoomImages",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    Image = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    HotelRoomId = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRoomImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelRoomImages_HotelRooms_HotelRoomId",
                        column: x => x.HotelRoomId,
                        principalTable: "HotelRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    CreatedById = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EditedById = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: true),
                    EditedOn = table.Column<DateTime>(nullable: true),
                    ReservedRoomId = table.Column<string>(unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    ReservedForDate = table.Column<DateTime>(nullable: false),
                    ReservedUntilDate = table.Column<DateTime>(nullable: false),
                    IncludeFood = table.Column<bool>(nullable: false),
                    AllInclusive = table.Column<bool>(nullable: false),
                    TransactionId = table.Column<decimal>(nullable: true),
                    AuthCode = table.Column<string>(unicode: false, maxLength: 35, nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_AspNetUsers_EditedById",
                        column: x => x.EditedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservations_HotelRooms_ReservedRoomId",
                        column: x => x.ReservedRoomId,
                        principalTable: "HotelRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
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
                unique: true,
                filter: "[EditedById] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelRoomImages_HotelRoomId",
                table: "HotelRoomImages",
                column: "HotelRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelRooms_CreatedById",
                table: "HotelRooms",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_HotelRooms_EditedById",
                table: "HotelRooms",
                column: "EditedById",
                unique: true,
                filter: "[EditedById] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CreatedById",
                table: "Reservations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_EditedById",
                table: "Reservations",
                column: "EditedById",
                unique: true,
                filter: "[EditedById] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservedRoomId",
                table: "Reservations",
                column: "ReservedRoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "HotelRoomImages");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "HotelRooms");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
