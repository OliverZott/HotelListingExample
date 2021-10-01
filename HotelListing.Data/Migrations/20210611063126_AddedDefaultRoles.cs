using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListingExample.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e5c1d643-502d-4ab4-9403-e3f0dc1e22a3", "541ffd9a-2814-46a1-8b97-00a079267912", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "78629074-76f4-4262-9cf0-cddfba65fec3", "dbd3a9fa-df55-4ffb-8350-6cccafcab5e5", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78629074-76f4-4262-9cf0-cddfba65fec3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5c1d643-502d-4ab4-9403-e3f0dc1e22a3");
        }
    }
}
