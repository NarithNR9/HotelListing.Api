using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "656fc1d5-c39d-4d35-bafe-88d0c9a2ca1e", "a2271a5b-35f5-4001-91bc-2ce8391c9553", "User", "USER" },
                    { "a7340aab-5bcd-46db-bbe3-d0761919c9d6", "d0017a83-fe1d-49e8-bed4-f2a1042e3065", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "656fc1d5-c39d-4d35-bafe-88d0c9a2ca1e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7340aab-5bcd-46db-bbe3-d0761919c9d6");
        }
    }
}
