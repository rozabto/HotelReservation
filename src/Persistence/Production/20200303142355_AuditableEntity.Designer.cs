﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence.Production;

namespace Persistence.Production
{
    [DbContext(typeof(ProductionDbContext))]
    [Migration("20200303142355_AuditableEntity")]
    partial class AuditableEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Domain.Entities.AppRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Domain.Entities.AppRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("ClaimValue")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Domain.Entities.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("character(36)")
                        .IsFixedLength(true)
                        .HasMaxLength(36);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("IsAdult")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Domain.Entities.AppUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("ClaimValue")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Domain.Entities.AppUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Domain.Entities.AppUserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<string>("RoleId")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Domain.Entities.AppUserToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<string>("LoginProvider")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<string>("CreatedById")
                        .IsRequired()
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("EGN")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("EditedById")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<DateTime?>("EditedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("EditedById");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Domain.Entities.HotelRoom", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<string>("CreatedById")
                        .IsRequired()
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EditedById")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<DateTime?>("EditedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("FoodPrice")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsEmpty")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<decimal?>("PriceForAdults")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("PriceForChildren")
                        .HasColumnType("numeric");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("integer");

                    b.Property<decimal?>("RoomPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("RoomType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("EditedById");

                    b.ToTable("HotelRooms");
                });

            modelBuilder.Entity("Domain.Entities.HotelRoomImage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<string>("HotelRoomId")
                        .IsRequired()
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("HotelRoomId");

                    b.ToTable("HotelRoomImages");
                });

            modelBuilder.Entity("Domain.Entities.Reservation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<bool>("AllInclusive")
                        .HasColumnType("boolean");

                    b.Property<string>("AuthCode")
                        .HasColumnType("character varying(35)")
                        .HasMaxLength(35)
                        .IsUnicode(false);

                    b.Property<string>("CreatedById")
                        .IsRequired()
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EditedById")
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<DateTime?>("EditedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IncludeFood")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("ReservedForDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ReservedRoomId")
                        .IsRequired()
                        .HasColumnType("character(32)")
                        .IsFixedLength(true)
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<DateTime>("ReservedUntilDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal?>("TransactionId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("EditedById");

                    b.HasIndex("ReservedRoomId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Domain.Entities.AppRoleClaim", b =>
                {
                    b.HasOne("Domain.Entities.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.AppUserClaim", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.AppUserLogin", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.AppUserRole", b =>
                {
                    b.HasOne("Domain.Entities.AppRole", "Role")
                        .WithMany("UsersRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.AppUser", "User")
                        .WithMany("UsersRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.AppUserToken", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.AppUser", "EditedBy")
                        .WithMany()
                        .HasForeignKey("EditedById")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Domain.Entities.AppUser", "User")
                        .WithOne("Employee")
                        .HasForeignKey("Domain.Entities.Employee", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.HotelRoom", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "CreatedBy")
                        .WithMany("CreatedRooms")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.AppUser", "EditedBy")
                        .WithMany()
                        .HasForeignKey("EditedById")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("Domain.Entities.HotelRoomImage", b =>
                {
                    b.HasOne("Domain.Entities.HotelRoom", "HotelRoom")
                        .WithMany("RoomImages")
                        .HasForeignKey("HotelRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Reservation", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "CreatedBy")
                        .WithMany("Reservations")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.AppUser", "EditedBy")
                        .WithMany()
                        .HasForeignKey("EditedById")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Domain.Entities.HotelRoom", "ReservedRoom")
                        .WithMany("Reservations")
                        .HasForeignKey("ReservedRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
