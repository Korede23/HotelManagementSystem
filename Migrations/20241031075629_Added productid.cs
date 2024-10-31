using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Addedproductid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "005972df-3e39-4766-a8e7-58f88aabecb3");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1e40fb6b-3ee4-44b5-896f-3466834ddce3", "aad0dc99-c5b3-4770-8719-8dc4247716a0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e40fb6b-3ee4-44b5-896f-3466834ddce3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aad0dc99-c5b3-4770-8719-8dc4247716a0");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b045c9e5-d0df-4bb7-aa2f-f801d14e6f38", null, "Admin", "ADMIN" },
                    { "e882fd3c-cff5-4a3e-932a-9085e1645274", null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AgeRange", "ConcurrencyStamp", "CreatedTime", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedTime", "UserName", "UserRole" },
                values: new object[] { "8c2c547b-b44f-418d-966e-4026e0d173cb", 0, "Lagos", "20-40", "23c0ddd6-f1be-459a-a7a7-04a3e847abbe", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "Ahmad", "Ahmad Korede", 1, "Oseni", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEJZW45yHBWmtH61q8+/M4unX8LrkvEJzXQzwEGifbTO/eyayVJoNardxhCS4zFMY+w==", "09068041575", false, "", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b045c9e5-d0df-4bb7-aa2f-f801d14e6f38", "8c2c547b-b44f-418d-966e-4026e0d173cb" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e882fd3c-cff5-4a3e-932a-9085e1645274");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b045c9e5-d0df-4bb7-aa2f-f801d14e6f38", "8c2c547b-b44f-418d-966e-4026e0d173cb" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b045c9e5-d0df-4bb7-aa2f-f801d14e6f38");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8c2c547b-b44f-418d-966e-4026e0d173cb");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "005972df-3e39-4766-a8e7-58f88aabecb3", null, "Customer", "CUSTOMER" },
                    { "1e40fb6b-3ee4-44b5-896f-3466834ddce3", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AgeRange", "ConcurrencyStamp", "CreatedTime", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedTime", "UserName", "UserRole" },
                values: new object[] { "aad0dc99-c5b3-4770-8719-8dc4247716a0", 0, "Lagos", "20-40", "d81fb057-4d83-402c-9fc4-6ab481fb61b1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "Ahmad", "Ahmad Korede", 1, "Oseni", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAECLdB+mJ0aB6AdbDAEb7TvR3cTQw3a9zeB7y84vIBZy7JdM6RQQ1Q1c5ipSJ5fzn8Q==", "09068041575", false, "", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1e40fb6b-3ee4-44b5-896f-3466834ddce3", "aad0dc99-c5b3-4770-8719-8dc4247716a0" });
        }
    }
}
