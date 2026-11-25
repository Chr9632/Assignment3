using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace assignment_3_movievault.Migrations
{
    /// <inheritdoc />
    public partial class update_system : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Actor",
                keyColumn: "Actor_id",
                keyValue: 1,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Director",
                keyColumn: "Director_id",
                keyValue: 1,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviewer",
                keyColumn: "Reviewer_id",
                keyValue: 1,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviewer",
                keyColumn: "Reviewer_id",
                keyValue: 2,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviewer",
                keyColumn: "Reviewer_id",
                keyValue: 3,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "10000000-0000-0000-0000-000000000001");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "10000000-0000-0000-0000-000000000002");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "10000000-0000-0000-0000-000000000003");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "10000000-0000-0000-0000-000000000004");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "10000000-0000-0000-0000-000000000005");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Actor",
                keyColumn: "Actor_id",
                keyValue: 1,
                column: "Identity_user_id",
                value: "10000000-0000-0000-0000-000000000001");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "10000000-0000-0000-0000-000000000001", 0, "8eae7664-fa8c-4679-b7f9-c713d62d6b42", "LD@example.com", false, false, null, "Leonardo DiCaprio", null, null, null, null, false, "6a8cd45e-a8fa-4f02-b563-d9c1bce3823d", false, null },
                    { "10000000-0000-0000-0000-000000000002", 0, "f959265c-1962-4a82-bf97-558cd70cff48", "CN@example.com", false, false, null, "Christopher Nolan", null, null, null, null, false, "33e79cb0-c41f-499d-abb2-5ad2f123ca78", false, null },
                    { "10000000-0000-0000-0000-000000000003", 0, "d82ec14d-fa48-47c7-bd59-729fe4e1a787", "AM@example.com", false, false, null, "Alex Morgan", null, null, null, null, false, "d6cf9f91-fd74-4ee3-a78c-c7315354fb9e", false, null },
                    { "10000000-0000-0000-0000-000000000004", 0, "4ed58d0a-dc1d-4f07-9feb-8b26db1ffd54", "JT@example.com", false, false, null, "Jamie Taylor", null, null, null, null, false, "e5621a0e-9bf6-4352-a2e9-5c4e51e0b564", false, null },
                    { "10000000-0000-0000-0000-000000000005", 0, "dc3edd47-8856-4cb4-9f02-d09d061a5112", "SR@example.com", false, false, null, "Sam Rivera", null, null, null, null, false, "d40d9c96-05ba-431a-ae4f-a2865054aea7", false, null }
                });

            migrationBuilder.UpdateData(
                table: "Director",
                keyColumn: "Director_id",
                keyValue: 1,
                column: "Identity_user_id",
                value: "10000000-0000-0000-0000-000000000002");

            migrationBuilder.UpdateData(
                table: "Reviewer",
                keyColumn: "Reviewer_id",
                keyValue: 1,
                column: "Identity_user_id",
                value: "10000000-0000-0000-0000-000000000003");

            migrationBuilder.UpdateData(
                table: "Reviewer",
                keyColumn: "Reviewer_id",
                keyValue: 2,
                column: "Identity_user_id",
                value: "10000000-0000-0000-0000-000000000004");

            migrationBuilder.UpdateData(
                table: "Reviewer",
                keyColumn: "Reviewer_id",
                keyValue: 3,
                column: "Identity_user_id",
                value: "10000000-0000-0000-0000-000000000005");
        }
    }
}
