using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace assignment_3_movievault.Migrations
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identity_user_id",
                table: "Director",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identity_user_id",
                table: "Actor",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Media_outlet",
                columns: table => new
                {
                    Media_outlet_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Outlet_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media_outlet", x => x.Media_outlet_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Reviewer",
                columns: table => new
                {
                    Reviewer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_outlet_id = table.Column<int>(type: "int", nullable: false),
                    Identity_user_id = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviewer", x => x.Reviewer_id);
                    table.ForeignKey(
                        name: "FK_Reviewer_AspNetUsers_Identity_user_id",
                        column: x => x.Identity_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviewer_Media_outlet_Media_outlet_id",
                        column: x => x.Media_outlet_id,
                        principalTable: "Media_outlet",
                        principalColumn: "Media_outlet_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Review_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Headline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Review_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Published_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Movie_id = table.Column<int>(type: "int", nullable: false),
                    Reviewer_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Review_id);
                    table.CheckConstraint("Review_rating", "[Rating] >= 1 AND [Rating] <= 5");
                    table.ForeignKey(
                        name: "FK_Review_Movie_Movie_id",
                        column: x => x.Movie_id,
                        principalTable: "Movie",
                        principalColumn: "Movie_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_Reviewer_Reviewer_id",
                        column: x => x.Reviewer_id,
                        principalTable: "Reviewer",
                        principalColumn: "Reviewer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Actor",
                keyColumn: "Actor_id",
                keyValue: 1,
                column: "Identity_user_id",
                value: "10000000-0000-0000-0000-000000000001");

            migrationBuilder.UpdateData(
                table: "Actor",
                keyColumn: "Actor_id",
                keyValue: 2,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Actor",
                keyColumn: "Actor_id",
                keyValue: 3,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Actor",
                keyColumn: "Actor_id",
                keyValue: 4,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Actor",
                keyColumn: "Actor_id",
                keyValue: 5,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Actor",
                keyColumn: "Actor_id",
                keyValue: 6,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Actor",
                keyColumn: "Actor_id",
                keyValue: 7,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Actor",
                keyColumn: "Actor_id",
                keyValue: 8,
                column: "Identity_user_id",
                value: null);

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
                table: "Director",
                keyColumn: "Director_id",
                keyValue: 2,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "Director",
                keyColumn: "Director_id",
                keyValue: 3,
                column: "Identity_user_id",
                value: null);

            migrationBuilder.InsertData(
                table: "Media_outlet",
                columns: new[] { "Media_outlet_id", "Name", "Outlet_type", "Website" },
                values: new object[,]
                {
                    { 1, "Movie Weekly", "Magazine", "https://movieweekly.example" },
                    { 2, "Screen Scene", "Newspaper", "https://screenscene.example" },
                    { 3, "Cinema Today", "Newspaper", "https://cinematoday.example" }
                });

            migrationBuilder.InsertData(
                table: "Reviewer",
                columns: new[] { "Reviewer_id", "Identity_user_id", "Media_outlet_id", "Name" },
                values: new object[,]
                {
                    { 1, "10000000-0000-0000-0000-000000000003", 1, "Alex Morgan" },
                    { 2, "10000000-0000-0000-0000-000000000004", 2, "Jamie Taylor" },
                    { 3, "10000000-0000-0000-0000-000000000005", 3, "Sam Rivera" }
                });

            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "Review_id", "Headline", "Movie_id", "Published_at", "Rating", "Review_text", "Reviewer_id" },
                values: new object[,]
                {
                    { 1, "A brilliant puzzle", 1, new DateTime(2026, 9, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 5, "Inception combines striking visuals with an inventive story.", 1 },
                    { 2, "A journey through space and time", 2, new DateTime(2026, 9, 13, 11, 30, 0, 0, DateTimeKind.Unspecified), 5, "Interstellar balances large-scale spectacle with an emotional story.", 1 },
                    { 3, "A striking character study", 3, new DateTime(2026, 9, 14, 14, 0, 0, 0, DateTimeKind.Unspecified), 4, "Joker delivers a focused performance in a distinctive, unsettling world.", 2 },
                    { 4, "A sweeping science-fiction epic", 4, new DateTime(2026, 9, 15, 9, 45, 0, 0, DateTimeKind.Unspecified), 4, "Dune brings its expansive world to the screen with memorable imagery.", 3 },
                    { 5, "An impressive continuation", 5, new DateTime(2026, 9, 16, 16, 15, 0, 0, DateTimeKind.Unspecified), 5, "Dune Part Two builds on the first film with larger set pieces and strong performances.", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Director_Identity_user_id",
                table: "Director",
                column: "Identity_user_id",
                unique: true,
                filter: "[Identity_user_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Actor_Identity_user_id",
                table: "Actor",
                column: "Identity_user_id",
                unique: true,
                filter: "[Identity_user_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Review_Movie_id",
                table: "Review",
                column: "Movie_id");

            migrationBuilder.CreateIndex(
                name: "IX_Review_Reviewer_id",
                table: "Review",
                column: "Reviewer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviewer_Identity_user_id",
                table: "Reviewer",
                column: "Identity_user_id",
                unique: true,
                filter: "[Identity_user_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reviewer_Media_outlet_id",
                table: "Reviewer",
                column: "Media_outlet_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Actor_AspNetUsers_Identity_user_id",
                table: "Actor",
                column: "Identity_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Director_AspNetUsers_Identity_user_id",
                table: "Director",
                column: "Identity_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actor_AspNetUsers_Identity_user_id",
                table: "Actor");

            migrationBuilder.DropForeignKey(
                name: "FK_Director_AspNetUsers_Identity_user_id",
                table: "Director");

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
                name: "Review");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Reviewer");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Media_outlet");

            migrationBuilder.DropIndex(
                name: "IX_Director_Identity_user_id",
                table: "Director");

            migrationBuilder.DropIndex(
                name: "IX_Actor_Identity_user_id",
                table: "Actor");

            migrationBuilder.DropColumn(
                name: "Identity_user_id",
                table: "Director");

            migrationBuilder.DropColumn(
                name: "Identity_user_id",
                table: "Actor");
        }
    }
}
