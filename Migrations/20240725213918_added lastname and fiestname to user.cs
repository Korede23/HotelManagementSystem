using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class addedlastnameandfiestnametouser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a57794a-8902-48ab-ba69-eb86061dba5d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f547126a-68ab-4a70-b19e-ea4987752692", "2f1e9519-dcf2-4ba9-9c52-63b03c268733" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f547126a-68ab-4a70-b19e-ea4987752692");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2f1e9519-dcf2-4ba9-9c52-63b03c268733");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "672f53aa-2f02-4e98-be74-aee1657798e4", null, "Admin", "ADMIN" },
                    { "bf6a9043-d510-48ff-a63b-374ab1dc3965", null, "Customer", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AgeRange", "ConcurrencyStamp", "CreatedTime", "DateOfBirth", "Discriminator", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedTime", "UserName", "UserRole" },
                values: new object[] { "fde3a93f-32d8-4a03-9d04-dd0db1efa14f", 0, null, "20-40", "495cc9e2-a3d8-4e39-9aa1-5b8cd62aeea6", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "admin@gmail.com", true, "Ahmad", 1, "oseni", false, null, "Ahmad Korede", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEKY6IGorvskhw8Cfp0n+f7MUQfimwd3iUkvrxVfvc/v5K4zByf7dv7mw6msaaKOYjA==", null, false, "", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "672f53aa-2f02-4e98-be74-aee1657798e4", "fde3a93f-32d8-4a03-9d04-dd0db1efa14f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf6a9043-d510-48ff-a63b-374ab1dc3965");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "672f53aa-2f02-4e98-be74-aee1657798e4", "fde3a93f-32d8-4a03-9d04-dd0db1efa14f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672f53aa-2f02-4e98-be74-aee1657798e4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fde3a93f-32d8-4a03-9d04-dd0db1efa14f");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8a57794a-8902-48ab-ba69-eb86061dba5d", null, "Customer", "Customer" },
                    { "f547126a-68ab-4a70-b19e-ea4987752692", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AgeRange", "ConcurrencyStamp", "CreatedTime", "DateOfBirth", "Discriminator", "Email", "EmailConfirmed", "Gender", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedTime", "UserName", "UserRole" },
                values: new object[] { "2f1e9519-dcf2-4ba9-9c52-63b03c268733", 0, null, "20-40", "104f0c94-54d2-4e59-a145-287c7e32d67d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "admin@gmail.com", true, 1, false, null, "Ahmad Korede", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEDmwv/Cv1xCc/0UOUcngrYwdxK6zjZ+Spw43ocT80At1JobzAGHPD7mHwHaGfa2m+Q==", null, false, "", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f547126a-68ab-4a70-b19e-ea4987752692", "2f1e9519-dcf2-4ba9-9c52-63b03c268733" });
        }
    }
}
