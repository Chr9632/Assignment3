using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace assignment_3_movievault.Migrations
{
    /// <inheritdoc />
    public partial class InitialTestDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actor",
                columns: table => new
                {
                    Actor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actor", x => x.Actor_id);
                    table.CheckConstraint("Actor_datebirth", "[Date_birth] < GETDATE()");
                    table.CheckConstraint("Actor_gender", "[Gender] IN ('M', 'F')");
                });

            migrationBuilder.CreateTable(
                name: "Cinema",
                columns: table => new
                {
                    Cinema_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opening_year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinema", x => x.Cinema_id);
                    table.CheckConstraint("Cinema_opening_year", "[Opening_year] < YEAR(GETDATE())");
                });

            migrationBuilder.CreateTable(
                name: "Director",
                columns: table => new
                {
                    Director_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Director", x => x.Director_id);
                    table.CheckConstraint("Director_datebirth", "[Date_birth] < GETDATE()");
                });

            migrationBuilder.CreateTable(
                name: "Filming_location",
                columns: table => new
                {
                    Location_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location_type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filming_location", x => x.Location_id);
                    table.CheckConstraint("Filming_location_type", "[Location_type] IN ('street', 'building', 'landscape', 'set')");
                });

            migrationBuilder.CreateTable(
                name: "Production_company",
                columns: table => new
                {
                    Company_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Founding_year = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_company", x => x.Company_id);
                    table.CheckConstraint("Company_founding_year", "[Founding_year] BETWEEN 1900 AND YEAR(GETDATE())");
                });

            migrationBuilder.CreateTable(
                name: "Screen",
                columns: table => new
                {
                    Screen_number = table.Column<int>(type: "int", nullable: false),
                    Cinema_id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Screen_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seating_capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screen", x => new { x.Screen_number, x.Cinema_id });
                    table.CheckConstraint("Screen_seating_check", "[Seating_capacity] > 0");
                    table.ForeignKey(
                        name: "FK_Screen_Cinema_Cinema_id",
                        column: x => x.Cinema_id,
                        principalTable: "Cinema",
                        principalColumn: "Cinema_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Movie_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Release_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Runtime = table.Column<int>(type: "int", nullable: false),
                    Age_rating = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Director_id = table.Column<int>(type: "int", nullable: false),
                    Company_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Movie_id);
                    table.CheckConstraint("Movie_status", "[Status] IN ('planned', 'in production', 'released', 'archived')");
                    table.ForeignKey(
                        name: "FK_Movie_Director_Director_id",
                        column: x => x.Director_id,
                        principalTable: "Director",
                        principalColumn: "Director_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movie_Production_company_Company_id",
                        column: x => x.Company_id,
                        principalTable: "Production_company",
                        principalColumn: "Company_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Studio",
                columns: table => new
                {
                    Studio_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number_sound_stages = table.Column<int>(type: "int", nullable: false),
                    Max_stage_capacity = table.Column<int>(type: "int", nullable: false),
                    Company_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studio", x => x.Studio_id);
                    table.CheckConstraint("Studio_sound_stages", "[Number_sound_stages] > 0");
                    table.CheckConstraint("Studio_stage_capacity", "[Max_stage_capacity] > 0");
                    table.ForeignKey(
                        name: "FK_Studio_Production_company_Company_id",
                        column: x => x.Company_id,
                        principalTable: "Production_company",
                        principalColumn: "Company_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movie_actor",
                columns: table => new
                {
                    Actor_id = table.Column<int>(type: "int", nullable: false),
                    Movie_id = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Character_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie_actor", x => new { x.Actor_id, x.Movie_id });
                    table.CheckConstraint("Actor_role", "[Role] IN ('lead', 'supporting', 'cameo')");
                    table.ForeignKey(
                        name: "FK_Movie_actor_Actor_Actor_id",
                        column: x => x.Actor_id,
                        principalTable: "Actor",
                        principalColumn: "Actor_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movie_actor_Movie_Movie_id",
                        column: x => x.Movie_id,
                        principalTable: "Movie",
                        principalColumn: "Movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movie_location",
                columns: table => new
                {
                    Movie_id = table.Column<int>(type: "int", nullable: false),
                    Location_id = table.Column<int>(type: "int", nullable: false),
                    Shooting_days = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie_location", x => new { x.Movie_id, x.Location_id });
                    table.CheckConstraint("Actor_location_shooting_days", "[Shooting_days] > 0");
                    table.ForeignKey(
                        name: "FK_Movie_location_Filming_location_Location_id",
                        column: x => x.Location_id,
                        principalTable: "Filming_location",
                        principalColumn: "Location_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movie_location_Movie_Movie_id",
                        column: x => x.Movie_id,
                        principalTable: "Movie",
                        principalColumn: "Movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production_record",
                columns: table => new
                {
                    Movie_id = table.Column<int>(type: "int", nullable: false),
                    Production_budget = table.Column<int>(type: "int", nullable: false),
                    Shooting_start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Shooting_end_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_record", x => x.Movie_id);
                    table.CheckConstraint("Production_record_budget", "[Production_budget] >= 0");
                    table.CheckConstraint("Shooting_dates", "[Shooting_start_date] <= [Shooting_end_date]");
                    table.ForeignKey(
                        name: "FK_Production_record_Movie_Movie_id",
                        column: x => x.Movie_id,
                        principalTable: "Movie",
                        principalColumn: "Movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Screenings",
                columns: table => new
                {
                    Screening_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ticket_price = table.Column<int>(type: "int", nullable: false),
                    Movie_id = table.Column<int>(type: "int", nullable: false),
                    Screen_number = table.Column<int>(type: "int", nullable: false),
                    Cinema_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenings", x => x.Screening_id);
                    table.ForeignKey(
                        name: "FK_Screenings_Movie_Movie_id",
                        column: x => x.Movie_id,
                        principalTable: "Movie",
                        principalColumn: "Movie_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Screenings_Screen_Screen_number_Cinema_id",
                        columns: x => new { x.Screen_number, x.Cinema_id },
                        principalTable: "Screen",
                        principalColumns: new[] { "Screen_number", "Cinema_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ticket_sale",
                columns: table => new
                {
                    Sale_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Sale_date_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Screening_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket_sale", x => x.Sale_id);
                    table.CheckConstraint("Studio_quantity", "[Quantity] > 0");
                    table.CheckConstraint("Studio_sale_datetime", "[Sale_date_time] < GETDATE()");
                    table.ForeignKey(
                        name: "FK_Ticket_sale_Screenings_Screening_id",
                        column: x => x.Screening_id,
                        principalTable: "Screenings",
                        principalColumn: "Screening_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Actor",
                columns: new[] { "Actor_id", "Date_birth", "Gender", "Name", "Nationality" },
                values: new object[,]
                {
                    { 1, new DateTime(1974, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "M", "Leonardo DiCaprio", "American" },
                    { 2, new DateTime(1981, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "M", "Joseph Gordon-Levitt", "American" },
                    { 3, new DateTime(1987, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "F", "Ellen Page", "Canadian" },
                    { 4, new DateTime(1969, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "M", "Matthew McConaughey", "American" },
                    { 5, new DateTime(1982, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "F", "Anne Hathaway", "American" },
                    { 6, new DateTime(1974, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "M", "Joaquin Phoenix", "American" },
                    { 7, new DateTime(1995, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "M", "Timothee Chalamet", "American" },
                    { 8, new DateTime(1996, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "F", "Zendaya", "American" }
                });

            migrationBuilder.InsertData(
                table: "Cinema",
                columns: new[] { "Cinema_id", "City", "Name", "Opening_year", "Street_address" },
                values: new object[,]
                {
                    { 1, "Randers", "Nordisk Film Biografer", 2009, "Thors Bakke 2" },
                    { 2, "Aarhus", "Bio", 1971, "Sankt Knuds Torv 15" },
                    { 3, "Horsens", "Biografen MegaScope Horsens", 2005, "Holmboes Alle 13B" }
                });

            migrationBuilder.InsertData(
                table: "Director",
                columns: new[] { "Director_id", "Date_birth", "Name", "Nationality" },
                values: new object[,]
                {
                    { 1, new DateTime(1970, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christopher Nolan", "British" },
                    { 2, new DateTime(1983, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Greta Gerwig", "American" },
                    { 3, new DateTime(1967, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Denis Villeneuve", "Canadian" }
                });

            migrationBuilder.InsertData(
                table: "Filming_location",
                columns: new[] { "Location_id", "City", "Country", "Location_type", "Name" },
                values: new object[,]
                {
                    { 1, "Los Angeles", "USA", "street", "Downtown Los Angeles" },
                    { 2, "Burbank", "USA", "set", "Warner Bros. Studio" },
                    { 3, "Aqaba", "Jordan", "landscape", "Wadi Rum" },
                    { 4, "Paris", "France", "street", "Paris City Center" },
                    { 5, "Reykjavik", "Iceland", "landscape", "Iceland Highlands" }
                });

            migrationBuilder.InsertData(
                table: "Production_company",
                columns: new[] { "Company_id", "Country", "Founding_year", "Name" },
                values: new object[,]
                {
                    { 1, "USA", 1923, "Warner Bros. Pictures" },
                    { 2, "USA", 1912, "Universal Pictures" },
                    { 3, "USA", 2000, "Legendary Pictures" }
                });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Movie_id", "Age_rating", "Company_id", "Director_id", "Release_date", "Runtime", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 13, 1, 1, new DateTime(2010, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 148, "released", "Inception" },
                    { 2, 13, 1, 1, new DateTime(2014, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 169, "released", "Interstellar" },
                    { 3, 13, 2, 2, new DateTime(2019, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 122, "released", "Joker" },
                    { 4, 13, 3, 3, new DateTime(2021, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 155, "released", "Dune" },
                    { 5, 13, 3, 3, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 166, "released", "Dune Part Two" }
                });

            migrationBuilder.InsertData(
                table: "Screen",
                columns: new[] { "Cinema_id", "Screen_number", "Name", "Screen_type", "Seating_capacity" },
                values: new object[,]
                {
                    { 1, 1, "Screen 1", "IMAX", 300 },
                    { 2, 1, "Screen 1", "IMAX", 350 },
                    { 3, 1, "Screen 1", "Standard", 150 },
                    { 1, 2, "Screen 2", "Standard", 180 },
                    { 2, 2, "Screen 2", "Standard", 200 },
                    { 3, 2, "Screen 2", "Premium", 120 }
                });

            migrationBuilder.InsertData(
                table: "Studio",
                columns: new[] { "Studio_id", "City", "Company_id", "Country", "Max_stage_capacity", "Name", "Number_sound_stages" },
                values: new object[,]
                {
                    { 1, "Burbank", 1, "USA", 5000, "Warner Bros. Stage 1", 12 },
                    { 2, "Burbank", 1, "USA", 4500, "Warner Bros. Stage 2", 10 },
                    { 3, "Los Angeles", 2, "USA", 3500, "Universal Stage 1", 8 },
                    { 4, "Los Angeles", 3, "USA", 3000, "Legendary Stage 1", 6 }
                });

            migrationBuilder.InsertData(
                table: "Movie_actor",
                columns: new[] { "Actor_id", "Movie_id", "Character_name", "Role" },
                values: new object[,]
                {
                    { 1, 1, "Cobb", "lead" },
                    { 2, 1, "Arthur", "supporting" },
                    { 3, 1, "Ariadne", "supporting" },
                    { 4, 1, "Eames", "supporting" },
                    { 4, 2, "Cooper", "lead" },
                    { 5, 1, "Mal", "supporting" },
                    { 5, 2, "Brand", "supporting" },
                    { 6, 3, "Arthur Fleck", "lead" },
                    { 7, 4, "Paul Atreides", "lead" },
                    { 7, 5, "Paul Atreides", "lead" },
                    { 8, 4, "Chani", "supporting" },
                    { 8, 5, "Chani", "supporting" }
                });

            migrationBuilder.InsertData(
                table: "Movie_location",
                columns: new[] { "Location_id", "Movie_id", "Shooting_days" },
                values: new object[,]
                {
                    { 1, 1, 15 },
                    { 4, 1, 10 },
                    { 5, 2, 20 },
                    { 1, 3, 25 },
                    { 2, 3, 10 },
                    { 3, 4, 30 },
                    { 5, 4, 15 },
                    { 2, 5, 20 },
                    { 3, 5, 35 }
                });

            migrationBuilder.InsertData(
                table: "Production_record",
                columns: new[] { "Movie_id", "Production_budget", "Shooting_end_date", "Shooting_start_date" },
                values: new object[,]
                {
                    { 1, 160000000, new DateTime(2009, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2009, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 165000000, new DateTime(2013, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2013, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 55000000, new DateTime(2018, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 165000000, new DateTime(2020, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 190000000, new DateTime(2022, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Screenings",
                columns: new[] { "Screening_id", "Cinema_id", "Date", "Movie_id", "Screen_number", "Start_time", "Ticket_price" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2026, 10, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), 120 },
                    { 2, 1, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, new DateTime(2026, 10, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), 120 },
                    { 3, 2, new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2026, 10, 2, 18, 30, 0, 0, DateTimeKind.Unspecified), 110 },
                    { 4, 1, new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, new DateTime(2026, 10, 3, 19, 0, 0, 0, DateTimeKind.Unspecified), 115 },
                    { 5, 2, new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, new DateTime(2026, 10, 4, 20, 0, 0, 0, DateTimeKind.Unspecified), 115 },
                    { 6, 1, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, new DateTime(2026, 10, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 7, 2, new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2, new DateTime(2026, 10, 6, 19, 30, 0, 0, DateTimeKind.Unspecified), 130 },
                    { 8, 3, new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, new DateTime(2026, 10, 7, 20, 0, 0, 0, DateTimeKind.Unspecified), 130 },
                    { 9, 2, new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, new DateTime(2026, 10, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), 135 },
                    { 10, 3, new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, new DateTime(2026, 10, 9, 20, 30, 0, 0, DateTimeKind.Unspecified), 135 }
                });

            migrationBuilder.InsertData(
                table: "Ticket_sale",
                columns: new[] { "Sale_id", "Quantity", "Sale_date_time", "Screening_id" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2026, 9, 10, 10, 15, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 3, new DateTime(2026, 9, 11, 14, 20, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 1, new DateTime(2026, 9, 10, 16, 30, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 4, 4, new DateTime(2026, 9, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 5, 2, new DateTime(2026, 9, 11, 13, 15, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 6, 3, new DateTime(2026, 9, 12, 9, 30, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 7, 2, new DateTime(2026, 9, 12, 17, 45, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 8, 5, new DateTime(2026, 9, 13, 12, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 9, 2, new DateTime(2026, 9, 13, 14, 30, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 10, 3, new DateTime(2026, 9, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 11, 1, new DateTime(2026, 9, 14, 16, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 12, 4, new DateTime(2026, 9, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 13, 2, new DateTime(2026, 9, 15, 15, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 14, 3, new DateTime(2026, 9, 16, 11, 30, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 15, 2, new DateTime(2026, 9, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Company_id",
                table: "Movie",
                column: "Company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Director_id",
                table: "Movie",
                column: "Director_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_actor_Movie_id",
                table: "Movie_actor",
                column: "Movie_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_location_Location_id",
                table: "Movie_location",
                column: "Location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Screen_Cinema_id",
                table: "Screen",
                column: "Cinema_id");

            migrationBuilder.CreateIndex(
                name: "IX_Screenings_Movie_id",
                table: "Screenings",
                column: "Movie_id");

            migrationBuilder.CreateIndex(
                name: "IX_Screenings_Screen_number_Cinema_id",
                table: "Screenings",
                columns: new[] { "Screen_number", "Cinema_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Studio_Company_id",
                table: "Studio",
                column: "Company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_sale_Screening_id",
                table: "Ticket_sale",
                column: "Screening_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie_actor");

            migrationBuilder.DropTable(
                name: "Movie_location");

            migrationBuilder.DropTable(
                name: "Production_record");

            migrationBuilder.DropTable(
                name: "Studio");

            migrationBuilder.DropTable(
                name: "Ticket_sale");

            migrationBuilder.DropTable(
                name: "Actor");

            migrationBuilder.DropTable(
                name: "Filming_location");

            migrationBuilder.DropTable(
                name: "Screenings");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "Screen");

            migrationBuilder.DropTable(
                name: "Director");

            migrationBuilder.DropTable(
                name: "Production_company");

            migrationBuilder.DropTable(
                name: "Cinema");
        }
    }
}
