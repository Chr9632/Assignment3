
using Microsoft.EntityFrameworkCore;

public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {}

    public DbSet<Actor> Actors {get; set; }
    public DbSet<Cinema> Cinemas {get; set; }
    public DbSet<Director> Directors {get; set; }
    public DbSet<Filming_location> Filming_locations {get; set; } 
    public DbSet<Movie_actor> Movie_actors {get; set; }
    public DbSet<Movie_location> Movie_locations {get; set; }
    public DbSet<Movie> Movies {get; set; } 
    public DbSet<Production_company> Production_Companies {get; set; }
    public DbSet<Production_record> Production_Records {get; set; }
    public DbSet<Screen> Screens {get; set; }
    public DbSet<Screening> Screenings {get; set; }
    public DbSet<Studio> Studios {get; set; }
    public DbSet<Ticket_sale> Ticket_sales {get; set; }

    /* 
        - .Property => Individual properties/columns

        - .ToTable => Table level configuration such as Check constraints

        - .HasKey => Primary key

    */

    // Migrations creating: dotnet ef migrations add "MigrationName, e.g. InitialData"
    // Migrations update: dotnet ef database update
    // Migrations reverting to previous migration: dotnet ef database update "PreviousMigrationName"
    // To undo action 1, run: ef migrations remove. 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*
            actor_id INT NOT NULL PRIMARY KEY,
            name VARCHAR(100) NOT NULL,
            date_birth DATETIME NOT NULL,
            nationality VARCHAR(100) NOT NULL,
            gender CHAR(1) NOT NULL, 
            CONSTRAINT actor_datebirth CHECK (date_birth < GETDATE()),
            CONSTRAINT actor_gender CHECK(gender IN ('M', 'F'))
        */
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(a => a.Actor_id);
            entity.Property(a => a.Name);
            entity.Property(a => a.Date_birth);
            entity.Property(a => a.Nationality); 
            entity.Property(a => a.Gender);
            entity.ToTable("Actor", table =>
            {
                table.HasCheckConstraint("Actor_datebirth", "[Date_birth] < GETDATE()");
                table.HasCheckConstraint("Actor_gender", "[Gender] IN ('M', 'F')");
            });

            /*
                INSERT INTO Actor(actor_id, name, date_birth, nationality, gender)
                VALUES
                (1, 'Leonardo DiCaprio', '1974-11-11', 'American', 'M'),
                (2, 'Joseph Gordon-Levitt', '1981-02-17', 'American', 'M'),
                (3, 'Ellen Page', '1987-02-21', 'Canadian', 'F'),
                (4, 'Matthew McConaughey', '1969-11-04', 'American', 'M'),
                (5, 'Anne Hathaway', '1982-11-12', 'American', 'F'),
                (6, 'Joaquin Phoenix', '1974-10-28', 'American', 'M'),
                (7, 'Timothee Chalamet', '1995-12-27', 'American', 'M'),
                (8, 'Zendaya', '1996-09-01', 'American', 'F');
            */

            entity.HasData(
                new Actor
                {
                    Actor_id = 1, 
                    Name = "Leonardo DiCaprio", 
                    Date_birth = new DateTime(1974, 11, 11),
                    Nationality="American",
                    Gender='M'
                }, 
                new Actor
                {
                    Actor_id = 2,
                    Name="Joseph Gordon-Levitt", 
                    Date_birth = new DateTime(1981, 02, 17), 
                    Nationality="American",
                    Gender='M'
                },
                new Actor
                {
                    Actor_id = 3,
                    Name="Ellen Page",
                    Date_birth = new DateTime(1987,02,21),
                    Nationality="Canadian", 
                    Gender='F'
                }, 
                new Actor
                {
                    Actor_id = 4, 
                    Name="Matthew McConaughey", 
                    Date_birth = new DateTime(1969,11,04),
                    Nationality="American", 
                    Gender='M'
                }, 
                new Actor
                {
                    Actor_id = 5,
                    Name = "Anne Hathaway",
                    Date_birth = new DateTime(1982, 11, 12),
                    Nationality = "American",
                    Gender = 'F'
                },
                new Actor
                {
                    Actor_id = 6,
                    Name = "Joaquin Phoenix",
                    Date_birth = new DateTime(1974, 10, 28),
                    Nationality = "American",
                    Gender = 'M'
                },
                new Actor
                {
                    Actor_id = 7,
                    Name = "Timothee Chalamet",
                    Date_birth = new DateTime(1995, 12, 27),
                    Nationality = "American",
                    Gender = 'M'
                },
                new Actor
                {
                    Actor_id = 8,
                    Name = "Zendaya",
                    Date_birth = new DateTime(1996, 9, 1),
                    Nationality = "American",
                    Gender = 'F'
                }
            );
            
        });

        /*
            cinema_id INT NOT NULL PRIMARY KEY,
            name VARCHAR(100) NOT NULL, 
            city VARCHAR(100) NOT NULL, 
            street_address VARCHAR(100) NOT NULL,
            opening_year INT NOT NULL CHECK(opening_year <= YEAR(GETDATE()))
        */

        modelBuilder.Entity<Cinema>(entity =>
        { 
            entity.HasMany(s => s.Screen).WithOne(c => c.Cinema);
            entity.HasKey(a => a.Cinema_id);
            entity.Property(a => a.Name);
            entity.Property(a => a.City);
            entity.Property(a => a.Street_address);
            entity.Property(a => a.Opening_year);
            entity.ToTable("Cinema", table =>
            {
                table.HasCheckConstraint("Cinema_opening_year", "[Opening_year] < YEAR(GETDATE())");
            });

            /*
                INSERT INTO Cinema(cinema_id, name, city, street_address, opening_year)
                VALUES
                (1, 'Nordisk Film Biografer', 'Randers', 'Thors Bakke 2', 2009),
                (2, 'Bio', 'Aarhus', 'Sankt Knuds Torv 15', 1971),
                (3, 'Biografen MegaScope Horsens', 'Horsens', 'Holmboes Alle 13B', 2005);
            */

            entity.HasData(
                new Cinema
                {
                    Cinema_id = 1,
                    Name = "Nordisk Film Biografer",
                    City = "Randers",
                    Street_address = "Thors Bakke 2",
                    Opening_year = 2009
                },
                new Cinema
                {
                    Cinema_id = 2,
                    Name = "Bio",
                    City = "Aarhus",
                    Street_address = "Sankt Knuds Torv 15",
                    Opening_year = 1971
                },
                new Cinema
                {
                    Cinema_id = 3,
                    Name = "Biografen MegaScope Horsens",
                    City = "Horsens",
                    Street_address = "Holmboes Alle 13B",
                    Opening_year = 2005
                }
            );
        });

        /*
            director_id INT NOT NULL PRIMARY KEY,
            name VARCHAR(100) NOT NULL,
            date_birth DATETIME NOT NULL,
            nationality VARCHAR(100) NOT NULL,
            CONSTRAINT director_datebirth CHECK (date_birth < GETDATE())
        */

        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasMany(m => m.Movie).WithOne(d => d.Director);
            entity.HasKey(a => a.Director_id);
            entity.Property(a => a.Name);
            entity.Property(a => a.Date_birth);
            entity.Property(a => a.Nationality);
            entity.ToTable("Director", table =>
            {
               table.HasCheckConstraint("Director_datebirth", "[Date_birth] < GETDATE()"); 
            });

            /*
                INSERT INTO Director(director_id, name, date_birth, nationality) 
                VALUES 
                (1, 'Christopher Nolan', '1970-07-30', 'British'),
                (2, 'Greta Gerwig', '1983-08-04', 'American'),
                (3, 'Denis Villeneuve', '1967-10-03', 'Canadian');
            */

            entity.HasData(
                new Director
                {
                    Director_id = 1,
                    Name = "Christopher Nolan",
                    Date_birth = new DateTime(1970, 7, 30),
                    Nationality = "British"
                },
                new Director
                {
                    Director_id = 2,
                    Name = "Greta Gerwig",
                    Date_birth = new DateTime(1983, 8, 4),
                    Nationality = "American"
                },
                new Director
                {
                    Director_id = 3,
                    Name = "Denis Villeneuve",
                    Date_birth = new DateTime(1967, 10, 3),
                    Nationality = "Canadian"
                }
            );
        });

        /*
            location_id INT NOT NULL PRIMARY KEY, 
            name VARCHAR(100) NOT NULL,
            city VARCHAR(100) NOT NULL, 
            country VARCHAR(100) NOT NULL, 
            location_type VARCHAR(100) NOT NULL,
            CONSTRAINT filming_location_type CHECK (location_type IN ('street', 'building', 'landscape', 'set'))
        */

        modelBuilder.Entity<Filming_location>(entity =>
        { 
            entity.HasKey(a => a.Location_id);
            entity.Property(a => a.Name); 
            entity.Property(a => a.City);
            entity.Property(a => a.Country); 
            entity.Property(a => a.Location_type);
            entity.ToTable("Filming_location", table =>
            {
               table.HasCheckConstraint("Filming_location_type", "[Location_type] IN ('street', 'building', 'landscape', 'set')"); 
            });

            /*
                INSERT INTO Filming_location(location_id, name, city, country, location_type)
                VALUES
                (1, 'Downtown Los Angeles', 'Los Angeles', 'USA', 'street'),
                (2, 'Warner Bros. Studio', 'Burbank', 'USA', 'set'),
                (3, 'Wadi Rum', 'Aqaba', 'Jordan', 'landscape'),
                (4, 'Paris City Center', 'Paris', 'France', 'street'),
                (5, 'Iceland Highlands', 'Reykjavik', 'Iceland', 'landscape');
            */

            entity.HasData(
                new Filming_location
                {
                    Location_id = 1,
                    Name = "Downtown Los Angeles",
                    City = "Los Angeles",
                    Country = "USA",
                    Location_type = "street"
                },
                new Filming_location
                {
                    Location_id = 2,
                    Name = "Warner Bros. Studio",
                    City = "Burbank",
                    Country = "USA",
                    Location_type = "set"
                },
                new Filming_location
                {
                    Location_id = 3,
                    Name = "Wadi Rum",
                    City = "Aqaba",
                    Country = "Jordan",
                    Location_type = "landscape"
                },
                new Filming_location
                {
                    Location_id = 4,
                    Name = "Paris City Center",
                    City = "Paris",
                    Country = "France",
                    Location_type = "street"
                },
                new Filming_location
                {
                    Location_id = 5,
                    Name = "Iceland Highlands",
                    City = "Reykjavik",
                    Country = "Iceland",
                    Location_type = "landscape"
                }
            );
        });

        /* 
            Movie_actor: 
            actor_id INT NOT NULL, 
            movie_id INT NOT NULL,
            role VARCHAR(100) NOT NULL,
            character_name VARCHAR(100) NOT NULL,
            PRIMARY KEY (actor_id, movie_id), 
            FOREIGN KEY (actor_id) REFERENCES Actor(actor_id) ON UPDATE CASCADE ON DELETE CASCADE,
            FOREIGN KEY (movie_id) REFERENCES Movie(movie_id) ON UPDATE CASCADE ON DELETE CASCADE,
            CONSTRAINT actor_role CHECK (role IN ('lead', 'supporting', 'cameo'))
        */

        modelBuilder.Entity<Movie_actor>(entity =>
        {
           entity.HasOne(a => a.Actor).WithMany(ma => ma.Movie_actor).HasForeignKey(a => a.Actor_id).OnDelete(DeleteBehavior.Cascade); 
           entity.HasOne(m => m.Movie).WithMany(ma => ma.Movie_actor).HasForeignKey(m => m.Movie_id).OnDelete(DeleteBehavior.Cascade);
           entity.HasKey(a => new {a.Actor_id, a.Movie_id});
           entity.Property(a => a.Role);
           entity.Property(a => a.Character_name);
           entity.ToTable("Movie_actor", table =>
           {
              table.HasCheckConstraint("Actor_role", "[Role] IN ('lead', 'supporting', 'cameo')"); 
           });

           /*
                INSERT INTO Movie_actor(actor_id, movie_id, role, character_name)
                VALUES

                // Inception
                (1, 1, 'lead', 'Cobb'),
                (2, 1, 'supporting', 'Arthur'),
                (3, 1, 'supporting', 'Ariadne'),
                (4, 1, 'supporting', 'Eames'),
                (5, 1, 'supporting', 'Mal'),

                // Interstellar
                (4, 2, 'lead', 'Cooper'),
                (5, 2, 'supporting', 'Brand'),

                // Joker
                (6, 3, 'lead', 'Arthur Fleck'),

                // Dune
                (7, 4, 'lead', 'Paul Atreides'),
                (8, 4, 'supporting', 'Chani'),

                // Dune Part Two
                (7, 5, 'lead', 'Paul Atreides'),
                (8, 5, 'supporting', 'Chani');
           */

           entity.HasData(
                // Inception
                new Movie_actor
                {
                    Actor_id = 1,
                    Movie_id = 1,
                    Role = "lead",
                    Character_name = "Cobb"
                },
                new Movie_actor
                {
                    Actor_id = 2,
                    Movie_id = 1,
                    Role = "supporting",
                    Character_name = "Arthur"
                },
                new Movie_actor
                {
                    Actor_id = 3,
                    Movie_id = 1,
                    Role = "supporting",
                    Character_name = "Ariadne"
                },
                new Movie_actor
                {
                    Actor_id = 4,
                    Movie_id = 1,
                    Role = "supporting",
                    Character_name = "Eames"
                },
                new Movie_actor
                {
                    Actor_id = 5,
                    Movie_id = 1,
                    Role = "supporting",
                    Character_name = "Mal"
                },

                // Interstellar
                new Movie_actor
                {
                    Actor_id = 4,
                    Movie_id = 2,
                    Role = "lead",
                    Character_name = "Cooper"
                },
                new Movie_actor
                {
                    Actor_id = 5,
                    Movie_id = 2,
                    Role = "supporting",
                    Character_name = "Brand"
                },

                // Joker
                new Movie_actor
                {
                    Actor_id = 6,
                    Movie_id = 3,
                    Role = "lead",
                    Character_name = "Arthur Fleck"
                },

                // Dune
                new Movie_actor
                {
                    Actor_id = 7,
                    Movie_id = 4,
                    Role = "lead",
                    Character_name = "Paul Atreides"
                },
                new Movie_actor
                {
                    Actor_id = 8,
                    Movie_id = 4,
                    Role = "supporting",
                    Character_name = "Chani"
                },

                // Dune Part Two
                new Movie_actor
                {
                    Actor_id = 7,
                    Movie_id = 5,
                    Role = "lead",
                    Character_name = "Paul Atreides"
                },
                new Movie_actor
                {
                    Actor_id = 8,
                    Movie_id = 5,
                    Role = "supporting",
                    Character_name = "Chani"
                }
            );
        });

        /*
            Movie_location: 
            movie_id INT NOT NULL, 
            location_id INT NOT NULL,
            shooting_days INT NOT NULL CHECK (shooting_days >= 0), 
            PRIMARY KEY (movie_id, location_id),
            FOREIGN KEY (movie_id) REFERENCES Movie(movie_id) ON UPDATE CASCADE ON DELETE CASCADE,
            FOREIGN KEY (location_id) REFERENCES Filming_location(location_id) ON UPDATE CASCADE ON DELETE CASCADE
        */

        modelBuilder.Entity<Movie_location>(entity =>
        {
           entity.HasOne(m => m.Movie).WithMany(ml => ml.Movie_location).HasForeignKey(m => m.Movie_id).OnDelete(DeleteBehavior.Cascade);
           entity.HasOne(f => f.Filming_location).WithMany(ml => ml.Movie_location).HasForeignKey(f => f.Location_id).OnDelete(DeleteBehavior.Cascade);
           entity.HasKey(a => new {a.Movie_id, a.Location_id});
           entity.Property(a => a.Shooting_days);
           entity.ToTable("Movie_location", table =>
           {
              table.HasCheckConstraint("Actor_location_shooting_days", "[Shooting_days] > 0"); 
           });

           /*
                INSERT INTO Movie_location(movie_id, location_id, shooting_days)
                VALUES

                // Inception uses multiple locations 
                (1, 1, 15),
                (1, 4, 10),

                // Interstellar using one location
                (2, 5, 20),

                // Joker using multiple locations 
                (3, 1, 25),
                (3, 2, 10), 

                // Dune using multiple locations 
                (4, 3, 30),
                (4, 5, 15),

                // Dune part two using multiple locations 
                (5, 3, 35), 
                (5, 2, 20);
           */

           entity.HasData(
                new Movie_location
                {
                    Movie_id = 1,
                    Location_id = 1,
                    Shooting_days = 15
                },
                new Movie_location
                {
                    Movie_id = 1,
                    Location_id = 4,
                    Shooting_days = 10
                },

                // Interstellar
                new Movie_location
                {
                    Movie_id = 2,
                    Location_id = 5,
                    Shooting_days = 20
                },

                // Joker
                new Movie_location
                {
                    Movie_id = 3,
                    Location_id = 1,
                    Shooting_days = 25
                },
                new Movie_location
                {
                    Movie_id = 3,
                    Location_id = 2,
                    Shooting_days = 10
                },

                // Dune
                new Movie_location
                {
                    Movie_id = 4,
                    Location_id = 3,
                    Shooting_days = 30
                },
                new Movie_location
                {
                    Movie_id = 4,
                    Location_id = 5,
                    Shooting_days = 15
                },

                // Dune Part Two
                new Movie_location
                {
                    Movie_id = 5,
                    Location_id = 3,
                    Shooting_days = 35
                },
                new Movie_location
                {
                    Movie_id = 5,
                    Location_id = 2,
                    Shooting_days = 20
                }
            );
        });

        /*
            movie_id INT NOT NULL PRIMARY KEY,
            release_date DATETIME NOT NULL, 
            runtime INT NOT NULL CHECK (runtime > 0), 
            age_rating INT NOT NULL, 
            status VARCHAR(100) NOT NULL, 
            title VARCHAR(100) NOT NULL,
            director_id INT NOT NULL,
            company_id INT NOT NULL,
            FOREIGN KEY (director_id) REFERENCES Director(director_id) ON UPDATE CASCADE ON DELETE CASCADE,
            FOREIGN KEY (company_id) REFERENCES Production_company(company_id) ON UPDATE CASCADE ON DELETE CASCADE,
            CONSTRAINT movie_status CHECK(status IN ('planned', 'in production', 'released', 'archived'))
        */

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasOne(d => d.Director).WithMany(m => m.Movie).HasForeignKey(d => d.Director_id).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pr => pr.Production_Record).WithOne(m => m.Movie);
            entity.HasMany(s => s.Screening).WithOne(m => m.Movie);
            entity.HasOne(pc => pc.Production_company).WithMany(m => m.Movie).HasForeignKey(pc => pc.Company_id).OnDelete(DeleteBehavior.Cascade);
            entity.HasKey(a => a.Movie_id);

            entity.Property(m => m.Movie_id).UseIdentityColumn();
            
            entity.Property(a => a.Release_date); 
            entity.Property(a => a.Runtime);
            entity.Property(a => a.Age_rating);
            entity.Property(a => a.Status);
            entity.Property(a => a.Title); 
            entity.ToTable("Movie", table =>
            {
               table.HasCheckConstraint("Movie_status", "[Status] IN ('planned', 'in production', 'released', 'archived')"); 
            });

            /*
                INSERT INTO Movie(movie_id, release_date, runtime, age_rating, status, title, director_id, company_id)
                VALUES
                (1, '2010-07-16', 148, 13, 'released', 'Inception', 1, 1),
                (2, '2014-11-07', 169, 13, 'released', 'Interstellar', 1, 1),
                (3, '2019-10-04', 122, 13, 'released', 'Joker', 2, 2),
                (4, '2021-10-22', 155, 13, 'released', 'Dune', 3, 3),
                (5, '2024-02-15', 166, 13, 'released', 'Dune Part Two', 3, 3);
            */

            entity.HasData(
                new Movie
                {
                    Movie_id = 1,
                    Release_date = new DateTime(2010, 7, 16),
                    Runtime = 148,
                    Age_rating = 13,
                    Status = "released",
                    Title = "Inception",
                    Director_id = 1,
                    Company_id = 1
                },
                new Movie
                {
                    Movie_id = 2,
                    Release_date = new DateTime(2014, 11, 7),
                    Runtime = 169,
                    Age_rating = 13,
                    Status = "released",
                    Title = "Interstellar",
                    Director_id = 1,
                    Company_id = 1
                },
                new Movie
                {
                    Movie_id = 3,
                    Release_date = new DateTime(2019, 10, 4),
                    Runtime = 122,
                    Age_rating = 13,
                    Status = "released",
                    Title = "Joker",
                    Director_id = 2,
                    Company_id = 2
                },
                new Movie
                {
                    Movie_id = 4,
                    Release_date = new DateTime(2021, 10, 22),
                    Runtime = 155,
                    Age_rating = 13,
                    Status = "released",
                    Title = "Dune",
                    Director_id = 3,
                    Company_id = 3
                },
                new Movie
                {
                    Movie_id = 5,
                    Release_date = new DateTime(2024, 2, 15),
                    Runtime = 166,
                    Age_rating = 13,
                    Status = "released",
                    Title = "Dune Part Two",
                    Director_id = 3,
                    Company_id = 3
                }
            );
        });

        /* 
            company_id INT NOT NULL PRIMARY KEY, 
            name VARCHAR(100) NOT NULL, 
            founding_year INT NOT NULL, 
            country VARCHAR(100), 
            CONSTRAINT company_founding_year CHECK (founding_year BETWEEN 1900 AND YEAR(GETDATE()))
        */

        modelBuilder.Entity<Production_company>(entity =>
        {
            entity.HasMany(m => m.Movie).WithOne(p => p.Production_company);
            entity.HasMany(s => s.Studio).WithOne(p => p.Production_company);
            entity.HasKey(a => a.Company_id);
            entity.Property(a => a.Name);
            entity.Property(a => a.Founding_year);
            entity.Property(a => a.Country);
            entity.ToTable("Production_company", table =>
            {
               table.HasCheckConstraint("Company_founding_year", "[Founding_year] BETWEEN 1900 AND YEAR(GETDATE())"); 
            });

            /*
                INSERT INTO Production_company(company_id, name, founding_year, country)
                VALUES
                (1, 'Warner Bros. Pictures', 1923, 'USA'),
                (2, 'Universal Pictures', 1912, 'USA'),
                (3, 'Legendary Pictures', 2000, 'USA');
            */

            entity.HasData(
                new Production_company
                {
                    Company_id = 1,
                    Name = "Warner Bros. Pictures",
                    Founding_year = 1923,
                    Country = "USA"
                },
                new Production_company
                {
                    Company_id = 2,
                    Name = "Universal Pictures",
                    Founding_year = 1912,
                    Country = "USA"
                },
                new Production_company
                {
                    Company_id = 3,
                    Name = "Legendary Pictures",
                    Founding_year = 2000,
                    Country = "USA"
                }
            );
        });

        /*
            Production_record: 
            movie_id INT NOT NULL, 
            production_budget INT NOT NULL CHECK(production_budget >= 0), 
            shooting_start_date DATETIME NOT NULL, 
            shooting_end_date DATETIME NOT NULL,
            PRIMARY KEY (movie_id),
            FOREIGN KEY (movie_id) REFERENCES Movie(movie_id) ON UPDATE CASCADE ON DELETE CASCADE,
            CONSTRAINT shooting_dates CHECK(shooting_start_date <= shooting_end_date)
        */

        modelBuilder.Entity<Production_record>(entity =>
        {
            entity.HasKey(a => a.Movie_id);


            entity.Property(pr => pr.Movie_id).ValueGeneratedNever();


            entity.HasOne(pr => pr.Movie).WithOne(m => m.Production_Record).HasForeignKey<Production_record>(pr => pr.Movie_id);

            entity.Property(a => a.Production_budget);
            entity.Property(a => a.Shooting_start_date);
            entity.Property(a => a.Shooting_end_date);
            entity.ToTable("Production_record", table =>
            {
               table.HasCheckConstraint("Production_record_budget", "[Production_budget] >= 0");
               table.HasCheckConstraint("Shooting_dates", "[Shooting_start_date] <= [Shooting_end_date]"); 
            });

            /*
                INSERT INTO Production_record(movie_id, production_budget, shooting_start_date, shooting_end_date)
                VALUES
                (1, 160000000, '2009-06-01', '2009-11-20'),
                (2, 165000000, '2013-08-01', '2013-12-20'),
                (3, 55000000, '2018-09-01', '2018-12-20'),
                (4, 165000000, '2020-03-01', '2020-07-15'),
                (5, 190000000, '2022-07-01', '2022-12-15');
            */

            entity.HasData(
                new Production_record
                {
                    Movie_id = 1,
                    Production_budget = 160000000,
                    Shooting_start_date = new DateTime(2009, 6, 1),
                    Shooting_end_date = new DateTime(2009, 11, 20)
                },
                new Production_record
                {
                    Movie_id = 2,
                    Production_budget = 165000000,
                    Shooting_start_date = new DateTime(2013, 8, 1),
                    Shooting_end_date = new DateTime(2013, 12, 20)
                },
                new Production_record
                {
                    Movie_id = 3,
                    Production_budget = 55000000,
                    Shooting_start_date = new DateTime(2018, 9, 1),
                    Shooting_end_date = new DateTime(2018, 12, 20)
                },
                new Production_record
                {
                    Movie_id = 4,
                    Production_budget = 165000000,
                    Shooting_start_date = new DateTime(2020, 3, 1),
                    Shooting_end_date = new DateTime(2020, 7, 15)
                },
                new Production_record
                {
                    Movie_id = 5,
                    Production_budget = 190000000,
                    Shooting_start_date = new DateTime(2022, 7, 1),
                    Shooting_end_date = new DateTime(2022, 12, 15)
                }
            );
        });

        /*
            Screen: 
            screen_number INT NOT NULL CHECK(screen_number > 0),
            cinema_id INT NOT NULL, 
            name VARCHAR(100) NOT NULL,
            screen_type VARCHAR(100) NOT NULL, 
            seating_capacity INT NOT NULL CHECK(seating_capacity > 0),
            PRIMARY KEY (screen_number, cinema_id),
            FOREIGN KEY (cinema_id) REFERENCES Cinema(cinema_id) ON UPDATE CASCADE ON DELETE CASCADE
        */

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasOne(c => c.Cinema).WithMany(s => s.Screen).HasForeignKey(c => c.Cinema_id).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(sc => sc.Screening).WithOne(s => s.Screen); 
            entity.HasKey(a => new {a.Screen_number, a.Cinema_id});
            entity.Property(a => a.Name);
            entity.Property(a => a.Screen_type);
            entity.Property(a => a.Seating_capacity);
            entity.ToTable("Screen", table =>
            {
               table.HasCheckConstraint("Screen_seating_check", "[Seating_capacity] > 0"); 
            });

            /*
                INSERT INTO Screen(screen_number, cinema_id, name, screen_type, seating_capacity)
                VALUES
                (1, 1, 'Screen 1', 'IMAX', 300),
                (2, 1, 'Screen 2', 'Standard', 180),

                (1, 2, 'Screen 1', 'IMAX', 350),
                (2, 2, 'Screen 2', 'Standard', 200),

                (1, 3, 'Screen 1', 'Standard', 150),
                (2, 3, 'Screen 2', 'Premium', 120);
            */

            entity.HasData(
                new Screen
                {
                    Screen_number = 1,
                    Cinema_id = 1,
                    Name = "Screen 1",
                    Screen_type = "IMAX",
                    Seating_capacity = 300
                },
                new Screen
                {
                    Screen_number = 2,
                    Cinema_id = 1,
                    Name = "Screen 2",
                    Screen_type = "Standard",
                    Seating_capacity = 180
                },
                new Screen
                {
                    Screen_number = 1,
                    Cinema_id = 2,
                    Name = "Screen 1",
                    Screen_type = "IMAX",
                    Seating_capacity = 350
                },
                new Screen
                {
                    Screen_number = 2,
                    Cinema_id = 2,
                    Name = "Screen 2",
                    Screen_type = "Standard",
                    Seating_capacity = 200
                },
                new Screen
                {
                    Screen_number = 1,
                    Cinema_id = 3,
                    Name = "Screen 1",
                    Screen_type = "Standard",
                    Seating_capacity = 150
                },
                new Screen
                {
                    Screen_number = 2,
                    Cinema_id = 3,
                    Name = "Screen 2",
                    Screen_type = "Premium",
                    Seating_capacity = 120
                }
            );
        });

        /*
            screening_id INT NOT NULL PRIMARY KEY,
            date DATETIME NOT NULL, 
            start_time DATETIME NOT NULL, 
            ticket_price INT NOT NULL CHECK (ticket_price >= 0), 
            movie_id INT NOT NULL,
            screen_number INT NOT NULL,
            cinema_id INT NOT NULL,
            FOREIGN KEY (movie_id) REFERENCES Movie(movie_id) ON UPDATE CASCADE ON DELETE CASCADE,
            FOREIGN KEY (screen_number, cinema_id) REFERENCES Screen(screen_number, cinema_id) ON UPDATE CASCADE ON DELETE
        */

        modelBuilder.Entity<Screening>(entity =>
        {
            entity.HasOne(s => s.Screen).WithMany(sc => sc.Screening).HasForeignKey(s => new {s.Screen_number, s.Cinema_id}).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(m => m.Movie).WithMany(sc => sc.Screening).HasForeignKey(m => m.Movie_id).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(t => t.Ticket_sale).WithOne(sc => sc.Screening);
            entity.HasKey(a => a.Screening_id);
            entity.Property(a => a.Date);
            entity.Property(a => a.Start_time);
            entity.Property(a => a.Ticket_price);

            /*
                INSERT INTO Screening(screening_id, date, start_time, ticket_price, movie_id, screen_number, cinema_id)
                VALUES
                (1, '2026-10-01', '2026-10-01 18:00:00', 120, 1, 1, 1),
                (2, '2026-10-01', '2026-10-01 21:00:00', 120, 1, 2, 1),

                (3, '2026-10-02', '2026-10-02 18:30:00', 110, 1, 1, 2),

                (4, '2026-10-03', '2026-10-03 19:00:00', 115, 2, 1, 1),
                (5, '2026-10-04', '2026-10-04 20:00:00', 115, 2, 1, 2),

                (6, '2026-10-05', '2026-10-05 18:00:00', 100, 3, 2, 1),

                (7, '2026-10-06', '2026-10-06 19:30:00', 130, 4, 2, 2),
                (8, '2026-10-07', '2026-10-07 20:00:00', 130, 4, 1, 3),

                (9, '2026-10-08', '2026-10-08 18:00:00', 135, 5, 2, 2),
                (10, '2026-10-09', '2026-10-09 20:30:00', 135, 5, 2, 3);
            */

            entity.HasData(
                new Screening
                {
                    Screening_id = 1,
                    Date = new DateTime(2026, 10, 1),
                    Start_time = new DateTime(2026, 10, 1, 18, 0, 0),
                    Ticket_price = 120,
                    Movie_id = 1,
                    Screen_number = 1,
                    Cinema_id = 1
                },
                new Screening
                {
                    Screening_id = 2,
                    Date = new DateTime(2026, 10, 1),
                    Start_time = new DateTime(2026, 10, 1, 21, 0, 0),
                    Ticket_price = 120,
                    Movie_id = 1,
                    Screen_number = 2,
                    Cinema_id = 1
                },
                new Screening
                {
                    Screening_id = 3,
                    Date = new DateTime(2026, 10, 2),
                    Start_time = new DateTime(2026, 10, 2, 18, 30, 0),
                    Ticket_price = 110,
                    Movie_id = 1,
                    Screen_number = 1,
                    Cinema_id = 2
                },
                new Screening
                {
                    Screening_id = 4,
                    Date = new DateTime(2026, 10, 3),
                    Start_time = new DateTime(2026, 10, 3, 19, 0, 0),
                    Ticket_price = 115,
                    Movie_id = 2,
                    Screen_number = 1,
                    Cinema_id = 1
                },
                new Screening
                {
                    Screening_id = 5,
                    Date = new DateTime(2026, 10, 4),
                    Start_time = new DateTime(2026, 10, 4, 20, 0, 0),
                    Ticket_price = 115,
                    Movie_id = 2,
                    Screen_number = 1,
                    Cinema_id = 2
                },
                new Screening
                {
                    Screening_id = 6,
                    Date = new DateTime(2026, 10, 5),
                    Start_time = new DateTime(2026, 10, 5, 18, 0, 0),
                    Ticket_price = 100,
                    Movie_id = 3,
                    Screen_number = 2,
                    Cinema_id = 1
                },
                new Screening
                {
                    Screening_id = 7,
                    Date = new DateTime(2026, 10, 6),
                    Start_time = new DateTime(2026, 10, 6, 19, 30, 0),
                    Ticket_price = 130,
                    Movie_id = 4,
                    Screen_number = 2,
                    Cinema_id = 2
                },
                new Screening
                {
                    Screening_id = 8,
                    Date = new DateTime(2026, 10, 7),
                    Start_time = new DateTime(2026, 10, 7, 20, 0, 0),
                    Ticket_price = 130,
                    Movie_id = 4,
                    Screen_number = 1,
                    Cinema_id = 3
                },
                new Screening
                {
                    Screening_id = 9,
                    Date = new DateTime(2026, 10, 8),
                    Start_time = new DateTime(2026, 10, 8, 18, 0, 0),
                    Ticket_price = 135,
                    Movie_id = 5,
                    Screen_number = 2,
                    Cinema_id = 2
                },
                new Screening
                {
                    Screening_id = 10,
                    Date = new DateTime(2026, 10, 9),
                    Start_time = new DateTime(2026, 10, 9, 20, 30, 0),
                    Ticket_price = 135,
                    Movie_id = 5,
                    Screen_number = 2,
                    Cinema_id = 3
                }
            );
        });

        /* 
            studio_id INT NOT NULL PRIMARY KEY,
            name VARCHAR(100) NOT NULL, 
            city VARCHAR(100) NOT NULL, 
            country VARCHAR(100) NOT NULL, 
            number_sound_stages INT NOT NULL CHECK(number_sound_stages > 0), 
            max_stage_capacity INT NOT NULL CHECK(max_stage_capacity > 0),
            company_id INT NOT NULL, 
            FOREIGN KEY (company_id) REFERENCES Production_company(company_id) ON UPDATE CASCADE ON DELETE CASCADE
        */

        modelBuilder.Entity<Studio>(entity =>
        {
            entity.HasOne(p => p.Production_company).WithMany(s => s.Studio).HasForeignKey(p => p.Company_id).OnDelete(DeleteBehavior.Cascade);
            entity.HasKey(a => a.Studio_id);
            entity.Property(a => a.Name);
            entity.Property(a => a.City);
            entity.Property(a => a.Country);
            entity.Property(a => a.Number_sound_stages);
            entity.Property(a => a.Max_stage_capacity);
            entity.ToTable("Studio", table =>
            {
                table.HasCheckConstraint("Studio_sound_stages", "[Number_sound_stages] > 0");
                table.HasCheckConstraint("Studio_stage_capacity", "[Max_stage_capacity] > 0");
            });

            /*
                INSERT INTO Studio(studio_id, name, city, country, number_sound_stages, max_stage_capacity, company_id)
                VALUES
                (1, 'Warner Bros. Stage 1', 'Burbank', 'USA', 12, 5000, 1),
                (2, 'Warner Bros. Stage 2', 'Burbank', 'USA', 10, 4500, 1),
                (3, 'Universal Stage 1', 'Los Angeles', 'USA', 8, 3500, 2),
                (4, 'Legendary Stage 1', 'Los Angeles', 'USA', 6, 3000, 3);
            */

            entity.HasData(
                new Studio
                {
                    Studio_id = 1,
                    Name = "Warner Bros. Stage 1",
                    City = "Burbank",
                    Country = "USA",
                    Number_sound_stages = 12,
                    Max_stage_capacity = 5000,
                    Company_id = 1
                },
                new Studio
                {
                    Studio_id = 2,
                    Name = "Warner Bros. Stage 2",
                    City = "Burbank",
                    Country = "USA",
                    Number_sound_stages = 10,
                    Max_stage_capacity = 4500,
                    Company_id = 1
                },
                new Studio
                {
                    Studio_id = 3,
                    Name = "Universal Stage 1",
                    City = "Los Angeles",
                    Country = "USA",
                    Number_sound_stages = 8,
                    Max_stage_capacity = 3500,
                    Company_id = 2
                },
                new Studio
                {
                    Studio_id = 4,
                    Name = "Legendary Stage 1",
                    City = "Los Angeles",
                    Country = "USA",
                    Number_sound_stages = 6,
                    Max_stage_capacity = 3000,
                    Company_id = 3
                }
            );
        });

        /*
            sale_id INT NOT NULL PRIMARY KEY, 
            quantity INT NOT NULL CHECK (quantity > 0), 
            sale_date_time DATETIME NOT NULL CHECK(sale_date_time < GETDATE()),
            screening_id INT NOT NULL,
            FOREIGN KEY (screening_id) REFERENCES Screening(screening_id)
        */

        modelBuilder.Entity<Ticket_sale>(entity =>
        {
            entity.HasOne(sc => sc.Screening).WithMany(t => t.Ticket_sale).HasForeignKey(sc => sc.Screening_id).OnDelete(DeleteBehavior.Cascade);
            entity.HasKey(a => a.Sale_id); 
            entity.Property(a => a.Quantity);
            entity.Property(a => a.Sale_date_time);
            entity.ToTable("Ticket_sale", table =>
            {
               table.HasCheckConstraint("Studio_quantity", "[Quantity] > 0");
               table.HasCheckConstraint("Studio_sale_datetime", "[Sale_date_time] < GETDATE()"); 
            });

            /*
                INSERT INTO Ticket_sale(sale_id, quantity, sale_date_time, screening_id)
                VALUES
                (1, 2, '2026-09-10 10:15:00', 1),
                (2, 3, '2026-09-11 14:20:00', 1),

                (3, 1, '2026-09-10 16:30:00', 2),
                (4, 4, '2026-09-11 11:00:00', 2),

                (5, 2, '2026-09-11 13:15:00', 3),

                (6, 3, '2026-09-12 09:30:00', 4),
                (7, 2, '2026-09-12 17:45:00', 5),

                (8, 5, '2026-09-13 12:00:00', 6),

                (9, 2, '2026-09-13 14:30:00', 7),
                (10, 3, '2026-09-14 10:00:00', 7),

                (11, 1, '2026-09-14 16:00:00', 8),

                (12, 4, '2026-09-15 09:00:00', 9),
                (13, 2, '2026-09-15 15:00:00', 9),

                (14, 3, '2026-09-16 11:30:00', 10),
                (15, 2, '2026-09-16 18:00:00', 10);
            */

            entity.HasData(
                new Ticket_sale
                {
                    Sale_id = 1,
                    Quantity = 2,
                    Sale_date_time = new DateTime(2026, 9, 10, 10, 15, 0),
                    Screening_id = 1
                },
                new Ticket_sale
                {
                    Sale_id = 2,
                    Quantity = 3,
                    Sale_date_time = new DateTime(2026, 9, 11, 14, 20, 0),
                    Screening_id = 1
                },
                new Ticket_sale
                {
                    Sale_id = 3,
                    Quantity = 1,
                    Sale_date_time = new DateTime(2026, 9, 10, 16, 30, 0),
                    Screening_id = 2
                },
                new Ticket_sale
                {
                    Sale_id = 4,
                    Quantity = 4,
                    Sale_date_time = new DateTime(2026, 9, 11, 11, 0, 0),
                    Screening_id = 2
                },
                new Ticket_sale
                {
                    Sale_id = 5,
                    Quantity = 2,
                    Sale_date_time = new DateTime(2026, 9, 11, 13, 15, 0),
                    Screening_id = 3
                },
                new Ticket_sale
                {
                    Sale_id = 6,
                    Quantity = 3,
                    Sale_date_time = new DateTime(2026, 9, 12, 9, 30, 0),
                    Screening_id = 4
                },
                new Ticket_sale
                {
                    Sale_id = 7,
                    Quantity = 2,
                    Sale_date_time = new DateTime(2026, 9, 12, 17, 45, 0),
                    Screening_id = 5
                },
                new Ticket_sale
                {
                    Sale_id = 8,
                    Quantity = 5,
                    Sale_date_time = new DateTime(2026, 9, 13, 12, 0, 0),
                    Screening_id = 6
                },
                new Ticket_sale
                {
                    Sale_id = 9,
                    Quantity = 2,
                    Sale_date_time = new DateTime(2026, 9, 13, 14, 30, 0),
                    Screening_id = 7
                },
                new Ticket_sale
                {
                    Sale_id = 10,
                    Quantity = 3,
                    Sale_date_time = new DateTime(2026, 9, 14, 10, 0, 0),
                    Screening_id = 7
                },
                new Ticket_sale
                {
                    Sale_id = 11,
                    Quantity = 1,
                    Sale_date_time = new DateTime(2026, 9, 14, 16, 0, 0),
                    Screening_id = 8
                },
                new Ticket_sale
                {
                    Sale_id = 12,
                    Quantity = 4,
                    Sale_date_time = new DateTime(2026, 9, 15, 9, 0, 0),
                    Screening_id = 9
                },
                new Ticket_sale
                {
                    Sale_id = 13,
                    Quantity = 2,
                    Sale_date_time = new DateTime(2026, 9, 15, 15, 0, 0),
                    Screening_id = 9
                },
                new Ticket_sale
                {
                    Sale_id = 14,
                    Quantity = 3,
                    Sale_date_time = new DateTime(2026, 9, 16, 11, 30, 0),
                    Screening_id = 10
                },
                new Ticket_sale
                {
                    Sale_id = 15,
                    Quantity = 2,
                    Sale_date_time = new DateTime(2026, 9, 16, 18, 0, 0),
                    Screening_id = 10
                }
            );
        });

        base.OnModelCreating(modelBuilder);
    }
    
}