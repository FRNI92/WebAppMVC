using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "PostalCode", "StreetName", "StreetNumber" },
                values: new object[,]
                {
                    { 1, "Stockholm", "12345", "Testgatan", "1" },
                    { 2, "Göteborg", "54321", "Exempelvägen", "2" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "ClientName" },
                values: new object[,]
                {
                    { 1, "IKEA" },
                    { 2, "GitLab Inc." }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "AddressId", "AvatarUrl", "DateOfBirth", "Email", "FirstName", "JobTitle", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, 1, "/images/fredrik.png", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "fredrik@domain.com", "Fredrik", "Developer", "Nilsson", "070-123 45 67" },
                    { 2, 2, "/images/elin.png", new DateTime(1992, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "elin@domain.com", "Elin", "Designer", "Andersson", "070-987 65 43" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
