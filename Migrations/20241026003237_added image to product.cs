using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class addedimagetoproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9acdefda-e634-42b7-bcf5-dd2c3ed009ac");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e5e63b3f-fd31-4478-b3e9-50ec0d25d5a0", "bbc86f61-9647-473d-87e3-1a495e35dbe7" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5e63b3f-fd31-4478-b3e9-50ec0d25d5a0");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbc86f61-9647-473d-87e3-1a495e35dbe7");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "591c1627-d296-4087-82d1-f6f6c84b061a", null, "Admin", "ADMIN" },
                    { "bd132305-3421-4002-97c1-bf93e3e804e1", null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AgeRange", "ConcurrencyStamp", "CreatedTime", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedTime", "UserName", "UserRole" },
                values: new object[] { "a6680baf-5835-48ab-8a4c-bbe980b26875", 0, null, "20-40", "0d365277-ce72-4045-8696-26102b86933f", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "Ahmad", "Ahmad Korede", 1, "Oseni", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEJO+lbkc+PyUELWqKaE+mUSOP9lQvdSlMiHOskmvcpAPhe+TZoDeYoYAf8qzdGLMRg==", null, false, "", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "591c1627-d296-4087-82d1-f6f6c84b061a", "a6680baf-5835-48ab-8a4c-bbe980b26875" });

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_ProductId",
                table: "Images",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_ProductId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductId",
                table: "Images");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd132305-3421-4002-97c1-bf93e3e804e1");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "591c1627-d296-4087-82d1-f6f6c84b061a", "a6680baf-5835-48ab-8a4c-bbe980b26875" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "591c1627-d296-4087-82d1-f6f6c84b061a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a6680baf-5835-48ab-8a4c-bbe980b26875");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Images");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9acdefda-e634-42b7-bcf5-dd2c3ed009ac", null, "Customer", "CUSTOMER" },
                    { "e5e63b3f-fd31-4478-b3e9-50ec0d25d5a0", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AgeRange", "ConcurrencyStamp", "CreatedTime", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedTime", "UserName", "UserRole" },
                values: new object[] { "bbc86f61-9647-473d-87e3-1a495e35dbe7", 0, null, "20-40", "d6c5398a-24a5-4436-ba71-f1d3737914ac", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "Ahmad", "Ahmad Korede", 1, "Oseni", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEPWUI4RIMEc2ZWGB7sTRN+4+Dlkl2Gpt5lONPrXxbj2x9rP4QAcjEIdbNNbZ9aq6UQ==", null, false, "", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e5e63b3f-fd31-4478-b3e9-50ec0d25d5a0", "bbc86f61-9647-473d-87e3-1a495e35dbe7" });
        }
    }
}
