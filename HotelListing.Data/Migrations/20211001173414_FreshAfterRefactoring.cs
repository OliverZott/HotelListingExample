using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListingExample.Migrations
{
    public partial class FreshAfterRefactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78629074-76f4-4262-9cf0-cddfba65fec3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5c1d643-502d-4ab4-9403-e3f0dc1e22a3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9a76e571-39be-4b1e-ac9e-0cab5a4dd2fa", "6282e77c-c066-4c5d-bc3e-853616f5dfaf", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "211e584f-c052-40eb-82db-f722fc846453", "9736a9e8-4ccc-4156-9109-ffaeeac59677", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CountryId",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "211e584f-c052-40eb-82db-f722fc846453");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a76e571-39be-4b1e-ac9e-0cab5a4dd2fa");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e5c1d643-502d-4ab4-9403-e3f0dc1e22a3", "541ffd9a-2814-46a1-8b97-00a079267912", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "78629074-76f4-4262-9cf0-cddfba65fec3", "dbd3a9fa-df55-4ffb-8350-6cccafcab5e5", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CountryId",
                value: 1);
        }
    }
}
