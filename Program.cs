using assignment_3_movievault.DTO;
using Microsoft.EntityFrameworkCore;
using assignment_3_movievault.Services;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

var mongoConnectionString = builder.Configuration["MongoDB:ConnectionString"];
var mongoCollectionName = builder.Configuration["MongoDB:CollectionName"];

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.MongoDBBson(
        mongoConnectionString!,
        collectionName: mongoCollectionName!)
    .CreateLogger();

builder.Host.UseSerilog();

Log.Information("MovieVault API - Serilog is working!");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MovieContext>(option =>
{
   option.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]); 
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<CinemaRevenue>();
builder.Services.AddScoped<MovieActor>();
builder.Services.AddScoped<MovieLocation>();
builder.Services.AddScoped<MovieServices>();
builder.Services.AddScoped<Screenings>();
builder.Services.AddScoped<TicketSales>();
builder.Services.AddSingleton<LogService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("username",
            httpContext.User.Identity?.Name ?? "anonymous");

        diagnosticContext.Set("timestamp", DateTime.UtcNow);

        diagnosticContext.Set("requestType",
            httpContext.Request.Method);

        diagnosticContext.Set("route",
            httpContext.Request.Path.ToString());

        diagnosticContext.Set("statusCode",
            httpContext.Response.StatusCode);
    };
});
using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

IRepository<Actor> repository = new Repository<Actor>(context);
var actors = await repository.GetAsyncAll();

foreach(var actor in actors)
{
    Console.WriteLine($"Actors: , {actor.Name}, Nationality: , {actor.Nationality}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}

app.MapControllers();


/*
//app.UseHttpsRedirection();
app.MapGet("/movies", async (MovieContext db) =>
{
    var movies = await db.Movies
        .AsNoTracking()
        .Select(m => new
        {
            m.Movie_id,
            m.Title,
            m.Release_date,
            m.Runtime,
            m.Age_rating,
            m.Status,
            m.Director_id,
            m.Company_id
        })
        .ToListAsync();

    return Results.Ok(movies);
});


app.MapGet("/movies/{id:int}", async (int id, MovieContext db) =>
{
    var movie = await db.Movies
        .AsNoTracking()
        .Where(m => m.Movie_id == id)
        .Select(m => new
        {
            m.Movie_id,
            m.Title,
            m.Release_date,
            m.Runtime,
            m.Age_rating,
            m.Status,
            m.Director_id,
            m.Company_id
        })
        .FirstOrDefaultAsync();

    if (movie == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(movie);
});


app.MapPost("/movies", async (CreateMovieDto dto, MovieContext db) =>
{
    // Kontroller DTO'ens valideringsregler
    var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
    var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

    bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
        dto, validationContext, validationResults, true);

    if (!isValid)
    {
        return Results.BadRequest(validationResults.Select(v => v.ErrorMessage));
    }

    // Kontroller filmens status
    string[] validStatuses = { "planned", "in production", "released", "archived" };

    if (!validStatuses.Contains(dto.Status))
    {
        return Results.BadRequest("Invalid movie status.");
    }

    // Kontroller at instruktør og produktionsselskab findes
    if (!await db.Directors.AnyAsync(d => d.Director_id == dto.Director_id))
    {
        return Results.BadRequest("Director does not exist.");
    }

    if (!await db.Production_Companies.AnyAsync(c => c.Company_id == dto.Company_id))
    {
        return Results.BadRequest("Production company does not exist.");
    }

    // Opret filmen
    var movie = new Movie
    {
        Title = dto.Title,
        Release_date = dto.Release_date,
        Runtime = dto.Runtime,
        Age_rating = dto.Age_rating,
        Status = dto.Status,
        Director_id = dto.Director_id,
        Company_id = dto.Company_id
    };

    db.Movies.Add(movie);
    await db.SaveChangesAsync();

    return Results.Created($"/movies/{movie.Movie_id}", new
    {
        movie.Movie_id,
        movie.Title,
        movie.Release_date,
        movie.Runtime,
        movie.Age_rating,
        movie.Status,
        movie.Director_id,
        movie.Company_id
    });
});


app.MapPut("/movies/{id:int}", async (int id, UpdateMovieDto dto, MovieContext db) =>
{
    // Find filmen i databasen
    var movie = await db.Movies.FindAsync(id);

    if (movie == null)
    {
        return Results.NotFound("Movie not found.");
    }

    // Kontroller DTO'ens valideringsregler
    var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
    var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

    bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
        dto, validationContext, validationResults, true);

    if (!isValid)
    {
        return Results.BadRequest(validationResults.Select(v => v.ErrorMessage));
    }

    // Kontroller filmens status
    string[] validStatuses = { "planned", "in production", "released", "archived" };

    if (!validStatuses.Contains(dto.Status))
    {
        return Results.BadRequest("Invalid movie status.");
    }

    // Kontroller at instruktør og produktionsselskab findes
    if (!await db.Directors.AnyAsync(d => d.Director_id == dto.Director_id))
    {
        return Results.BadRequest("Director does not exist.");
    }

    if (!await db.Production_Companies.AnyAsync(c => c.Company_id == dto.Company_id))
    {
        return Results.BadRequest("Production company does not exist.");
    }

    // Opdater filmens oplysninger
    movie.Title = dto.Title;
    movie.Release_date = dto.Release_date;
    movie.Runtime = dto.Runtime;
    movie.Age_rating = dto.Age_rating;
    movie.Status = dto.Status;
    movie.Director_id = dto.Director_id;
    movie.Company_id = dto.Company_id;

    await db.SaveChangesAsync();

    return Results.Ok(movie);
});



app.MapDelete("/movies/{id:int}", async (int id, MovieContext db) =>
{
    // Find filmen i databasen
    var movie = await db.Movies.FindAsync(id);

    if (movie == null)
    {
        return Results.NotFound("Movie not found.");
    }

    // Slet filmen
    db.Movies.Remove(movie);
    await db.SaveChangesAsync();

    return Results.NoContent();
});


app.MapPost("/ticket-sales", async (CreateTicketSaleDto dto, MovieContext db) =>
{
    // Kontrollér antal billetter
    if (dto.Quantity <= 0)
        return Results.BadRequest("Quantity must be greater than 0.");

    // Kontrollér salgsdato
    if (dto.Sale_date_time == default ||
        dto.Sale_date_time >= DateTime.Now)
        return Results.BadRequest("Sale date must be in the past.");

    // Find forestillingen og dens biografsal
    var screening = await db.Set<Screening>()
        .Include(s => s.Screen)
        .FirstOrDefaultAsync(s => s.Screening_id == dto.Screening_id);

    if (screening == null)
        return Results.BadRequest("Screening does not exist.");

    // Find antal billetter, der allerede er solgt
    int ticketsSold = await db.Set<Ticket_sale>()
        .Where(t => t.Screening_id == dto.Screening_id)
        .SumAsync(t => t.Quantity);

    // Find biografsalens kapacitet
    int capacity = screening.Screen.Seating_capacity;

    // Kontrollér om der er plads til de nye billetter
    if ((long)ticketsSold + dto.Quantity > capacity)
        return Results.Conflict(
            "Not enough available seats for this screening.");

    // Opret billetsalget
    var sale = new Ticket_sale
    {
        Quantity = dto.Quantity,
        Sale_date_time = dto.Sale_date_time,
        Screening_id = dto.Screening_id
    };

    db.Set<Ticket_sale>().Add(sale);
    await db.SaveChangesAsync();

    return Results.Created($"/ticket-sales/{sale.Sale_id}", new
    {
        sale.Sale_id,
        sale.Quantity,
        sale.Sale_date_time,
        sale.Screening_id
    });
});



app.MapGet("/ticket-sales", async (MovieContext db) =>
{
    var sales = await db.Set<Ticket_sale>()
        .AsNoTracking()
        .Select(s => new
        {
            s.Sale_id,
            s.Quantity,
            s.Sale_date_time,
            s.Screening_id
        })
        .ToListAsync();

    return Results.Ok(sales);
});



app.MapGet("/ticket-sales/{id:int}", async (int id, MovieContext db) =>
{
    var sale = await db.Set<Ticket_sale>()
        .AsNoTracking()
        .Where(s => s.Sale_id == id)
        .Select(s => new
        {
            s.Sale_id,
            s.Quantity,
            s.Sale_date_time,
            s.Screening_id
        })
        .FirstOrDefaultAsync();

    if (sale == null)
    {
        return Results.NotFound("Ticket sale not found.");
    }

    return Results.Ok(sale);
});



app.MapPut("/ticket-sales/{id:int}", async (
    int id,
    UpdateTicketSaleDto dto,
    MovieContext db) =>
{
    // Find billetsalget
    var sale = await db.Set<Ticket_sale>().FindAsync(id);

    if (sale == null)
        return Results.NotFound("Ticket sale not found.");

    // Kontrollér antal billetter
    if (dto.Quantity <= 0)
        return Results.BadRequest("Quantity must be greater than 0.");

    // Kontrollér salgsdato
    if (dto.Sale_date_time == default ||
        dto.Sale_date_time >= DateTime.Now)
        return Results.BadRequest("Sale date must be in the past.");

    // Find forestillingen og dens biografsal
    var screening = await db.Set<Screening>()
        .Include(s => s.Screen)
        .FirstOrDefaultAsync(s =>
            s.Screening_id == dto.Screening_id);

    if (screening == null)
        return Results.BadRequest("Screening does not exist.");

    // Find antal billetter solgt til forestillingen,
    // men undlad det billetsalg, vi er ved at opdatere
    int ticketsSold = await db.Set<Ticket_sale>()
        .Where(t =>
            t.Screening_id == dto.Screening_id &&
            t.Sale_id != id)
        .SumAsync(t => t.Quantity);

    // Find biografsalens kapacitet
    int capacity = screening.Screen.Seating_capacity;

    // Kontrollér om der er plads til det opdaterede antal
    if ((long)ticketsSold + dto.Quantity > capacity)
        return Results.Conflict(
            "Not enough available seats for this screening.");

    // Opdater billetsalget
    sale.Quantity = dto.Quantity;
    sale.Sale_date_time = dto.Sale_date_time;
    sale.Screening_id = dto.Screening_id;

    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        sale.Sale_id,
        sale.Quantity,
        sale.Sale_date_time,
        sale.Screening_id
    });
});


app.MapDelete("/ticket-sales/{id:int}", async (int id, MovieContext db) =>
{
    // Find billetsalget
    var sale = await db.Set<Ticket_sale>().FindAsync(id);

    if (sale == null)
    {
        return Results.NotFound("Ticket sale not found.");
    }

    // Slet billetsalget
    db.Set<Ticket_sale>().Remove(sale);
    await db.SaveChangesAsync();

    return Results.NoContent();
});


app.MapPost("/movies/{movieId:int}/actors", async (
    int movieId,
    AddMovieActorDto dto,
    MovieContext db) =>
{
    // Kontrollér at filmen findes
    if (!await db.Set<Movie>().AnyAsync(m => m.Movie_id == movieId))
        return Results.NotFound("Movie not found.");

    // Kontrollér at skuespilleren findes
    if (!await db.Set<Actor>().AnyAsync(a => a.Actor_id == dto.Actor_id))
        return Results.NotFound("Actor not found.");

    // Kontrollér at skuespilleren ikke allerede er tilføjet
    bool exists = await db.Set<Movie_actor>().AnyAsync(ma =>
        ma.Movie_id == movieId && ma.Actor_id == dto.Actor_id);

    if (exists)
        return Results.Conflict("Actor is already assigned to this movie.");

    // Opret forbindelsen mellem film og skuespiller
    var movieActor = new Movie_actor
    {
        Movie_id = movieId,
        Actor_id = dto.Actor_id,
        Role = dto.Role,
        Character_name = dto.Character_name
    };

    db.Set<Movie_actor>().Add(movieActor);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/movies/{movieId}/actors/{dto.Actor_id}",
        new
        {
            movieActor.Movie_id,
            movieActor.Actor_id,
            movieActor.Role,
            movieActor.Character_name
        });
});



app.MapDelete("/movies/{movieId:int}/actors/{actorId:int}",
    async (int movieId, int actorId, MovieContext db) =>
    {
        // Find forbindelsen mellem film og skuespiller
        var movieActor = await db.Set<Movie_actor>()
            .FirstOrDefaultAsync(ma =>
                ma.Movie_id == movieId &&
                ma.Actor_id == actorId);

        if (movieActor == null)
            return Results.NotFound("Actor is not assigned to this movie.");

        // Fjern forbindelsen
        db.Set<Movie_actor>().Remove(movieActor);
        await db.SaveChangesAsync();

        return Results.NoContent();
    });


app.MapPost("/movies/{movieId:int}/locations", async (
    int movieId,
    AddMovieLocationDto dto,
    MovieContext db) =>
{
    // Kontrollér at filmen findes
    if (!await db.Set<Movie>().AnyAsync(m => m.Movie_id == movieId))
        return Results.NotFound("Movie not found.");

    // Kontrollér at optagelsesstedet findes
    if (!await db.Set<Filming_location>()
        .AnyAsync(l => l.Location_id == dto.Location_id))
        return Results.NotFound("Filming location not found.");

    // Kontrollér at optagelsesstedet ikke allerede er tilføjet
    bool exists = await db.Set<Movie_location>().AnyAsync(ml =>
        ml.Movie_id == movieId &&
        ml.Location_id == dto.Location_id);

    if (exists)
        return Results.Conflict(
            "Filming location is already assigned to this movie.");

    // Opret forbindelsen
    var movieLocation = new Movie_location
    {
        Movie_id = movieId,
        Location_id = dto.Location_id,
        Shooting_days = dto.Shooting_days
    };

    db.Set<Movie_location>().Add(movieLocation);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/movies/{movieId}/locations/{dto.Location_id}",
        new
        {
            movieLocation.Movie_id,
            movieLocation.Location_id,
            movieLocation.Shooting_days
        });
});


app.MapDelete("/movies/{movieId:int}/locations/{locationId:int}",
    async (int movieId, int locationId, MovieContext db) =>
    {
        // Find forbindelsen mellem film og optagelsessted
        var movieLocation = await db.Set<Movie_location>()
            .FirstOrDefaultAsync(ml =>
                ml.Movie_id == movieId &&
                ml.Location_id == locationId);

        if (movieLocation == null)
            return Results.NotFound(
                "Filming location is not assigned to this movie.");

        // Fjern forbindelsen
        db.Set<Movie_location>().Remove(movieLocation);
        await db.SaveChangesAsync();

        return Results.NoContent();
    });


app.MapPost("/screenings", async (
    CreateScreeningDto dto,
    MovieContext db) =>
{
    // Kontrollér at filmen findes
    if (!await db.Set<Movie>()
        .AnyAsync(m => m.Movie_id == dto.Movie_id))
        return Results.BadRequest("Movie does not exist.");

    // Kontrollér at biografsalen findes
    if (!await db.Set<Screen>()
        .AnyAsync(s =>
            s.Screen_number == dto.Screen_number &&
            s.Cinema_id == dto.Cinema_id))
        return Results.BadRequest("Screen does not exist.");

    // Kontrollér billetprisen
    if (dto.Ticket_price < 0)
        return Results.BadRequest("Ticket price cannot be negative.");

    // Kontrollér dato og starttidspunkt
    if (dto.Date == default || dto.Start_time == default)
        return Results.BadRequest("Date and start time are required.");

    // Opret forestillingen
    var screening = new Screening
    {
        Date = dto.Date,
        Start_time = dto.Start_time,
        Ticket_price = dto.Ticket_price,
        Movie_id = dto.Movie_id,
        Screen_number = dto.Screen_number,
        Cinema_id = dto.Cinema_id
    };

    db.Set<Screening>().Add(screening);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/screenings/{screening.Screening_id}",
        new
        {
            screening.Screening_id,
            screening.Date,
            screening.Start_time,
            screening.Ticket_price,
            screening.Movie_id,
            screening.Screen_number,
            screening.Cinema_id
        });
});


app.MapGet("/movies/{movieId:int}/screenings",
    async (int movieId, MovieContext db) =>
    {
        // Kontrollér at filmen findes
        if (!await db.Set<Movie>()
            .AnyAsync(m => m.Movie_id == movieId))
            return Results.NotFound("Movie not found.");

        // Hent forestillinger for filmen
        var screenings = await db.Set<Screening>()
            .AsNoTracking()
            .Where(s => s.Movie_id == movieId)
            .OrderBy(s => s.Start_time)
            .Select(s => new
            {
                s.Screening_id,
                s.Date,
                s.Start_time,
                s.Ticket_price,
                s.Movie_id,
                s.Screen_number,
                s.Cinema_id
            })
            .ToListAsync();

        return Results.Ok(screenings);
    });


app.MapGet("/cinemas/{cinemaId:int}/screenings",
    async (int cinemaId, MovieContext db) =>
    {
        // Kontrollér at biografen findes
        if (!await db.Set<Cinema>()
            .AnyAsync(c => c.Cinema_id == cinemaId))
            return Results.NotFound("Cinema not found.");

        // Hent alle forestillinger i biografen
        var screenings = await db.Set<Screening>()
            .AsNoTracking()
            .Where(s => s.Cinema_id == cinemaId)
            .OrderBy(s => s.Start_time)
            .Select(s => new
            {
                s.Screening_id,
                s.Date,
                s.Start_time,
                s.Ticket_price,
                s.Movie_id,
                s.Screen_number,
                s.Cinema_id
            })
            .ToListAsync();

        return Results.Ok(screenings);
    });


app.MapPut("/screenings/{id:int}", async (
    int id,
    UpdateScreeningDto dto,
    MovieContext db) =>
{
    // Find forestillingen
    var screening = await db.Set<Screening>().FindAsync(id);

    if (screening == null)
        return Results.NotFound("Screening not found.");

    // Kontrollér dato og tidspunkt
    if (dto.Date == default || dto.Start_time == default)
        return Results.BadRequest("Date and start time are required.");

    if (dto.Date.Date != dto.Start_time.Date)
        return Results.BadRequest(
            "Date and start time must refer to the same day.");

    // Kontrollér billetprisen
    if (dto.Ticket_price < 0)
        return Results.BadRequest("Ticket price cannot be negative.");

    // Kontrollér at filmen findes
    if (!await db.Set<Movie>()
        .AnyAsync(m => m.Movie_id == dto.Movie_id))
        return Results.BadRequest("Movie does not exist.");

    // Kontrollér at biografsalen findes
    var screen = await db.Set<Screen>()
        .FirstOrDefaultAsync(s =>
            s.Screen_number == dto.Screen_number &&
            s.Cinema_id == dto.Cinema_id);

    if (screen == null)
        return Results.BadRequest("Screen does not exist.");

    // Opdater forestillingen
    screening.Date = dto.Date;
    screening.Start_time = dto.Start_time;
    screening.Ticket_price = dto.Ticket_price;
    screening.Movie_id = dto.Movie_id;
    screening.Screen_number = dto.Screen_number;
    screening.Cinema_id = dto.Cinema_id;

    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        screening.Screening_id,
        screening.Date,
        screening.Start_time,
        screening.Ticket_price,
        screening.Movie_id,
        screening.Screen_number,
        screening.Cinema_id
    });
});


app.MapDelete("/screenings/{id:int}",
    async (int id, MovieContext db) =>
    {
        // Find forestillingen
        var screening = await db.Set<Screening>().FindAsync(id);

        if (screening == null)
            return Results.NotFound("Screening not found.");

        // Undgå at slette forestillinger med billetsalg
        bool hasTicketSales = await db.Set<Ticket_sale>()
            .AnyAsync(t => t.Screening_id == id);

        if (hasTicketSales)
            return Results.Conflict(
                "Cannot delete a screening with ticket sales.");

        // Slet forestillingen
        db.Set<Screening>().Remove(screening);
        await db.SaveChangesAsync();

        return Results.NoContent();
    });

app.MapGet("/movies/overview", async (MovieContext db) =>
{
    var movies = await db.Set<Movie>()
        .AsNoTracking()
        .Select(m => new
        {
            m.Movie_id,
            m.Title,
            m.Release_date,
            m.Runtime,
            m.Age_rating,
            m.Status,

            Director = m.Director.Name,
            Production_company = m.Production_company.Name,
            Production_budget = m.Production_Record.Production_budget
        })
        .ToListAsync();

    return Results.Ok(movies);
});


app.MapGet("/movies/{id:int}/details", async (int id, MovieContext db) =>
{
    var movie = await db.Set<Movie>()
        .AsNoTracking()
        .Where(m => m.Movie_id == id)
        .Select(m => new
        {
            m.Movie_id,
            m.Title,
            m.Release_date,
            m.Runtime,
            m.Age_rating,
            m.Status,

            Director = m.Director.Name,
            Production_company = m.Production_company.Name,
            Production_budget = m.Production_Record.Production_budget,

            Actors = m.Movie_actor.Select(ma => new
            {
                ma.Actor_id,
                Actor_name = ma.Actor.Name,
                ma.Role,
                ma.Character_name
            }).ToList(),

            Filming_locations = m.Movie_location.Select(ml => new
            {
                ml.Location_id,
                Location_name = ml.Filming_location.Name,
                ml.Filming_location.City,
                ml.Filming_location.Country,
                ml.Filming_location.Location_type,
                ml.Shooting_days
            }).ToList()
        })
        .FirstOrDefaultAsync();

    if (movie == null)
        return Results.NotFound("Movie not found.");

    return Results.Ok(movie);
});


app.MapGet("/screenings/upcoming", async (MovieContext db) =>
{
    var now = DateTime.Now;

    var screenings = await db.Set<Screening>()
        .AsNoTracking()
        .Where(s => s.Start_time > now)
        .OrderBy(s => s.Start_time)
        .Select(s => new
        {
            s.Screening_id,
            Movie_title = s.Movie.Title,
            Cinema_name = s.Screen.Cinema.Name,
            Cinema_city = s.Screen.Cinema.City,
            s.Screen_number,
            Screen_name = s.Screen.Name,
            s.Date,
            s.Start_time,
            s.Ticket_price
        })
        .ToListAsync();

    return Results.Ok(screenings);
});


app.MapGet("/movies/{id:int}/ticket-sales", async (int id, MovieContext db) =>
{
    // Kontroller at filmen findes
    var movie = await db.Set<Movie>()
        .Where(m => m.Movie_id == id)
        .Select(m => new { m.Movie_id, m.Title })
        .FirstOrDefaultAsync();

    if (movie == null)
        return Results.NotFound("Movie not found.");

    // Find alle billetsalg til filmens forestillinger
    var sales = await db.Set<Ticket_sale>()
        .Where(t => t.Screening.Movie_id == id)
        .Select(t => new
        {
            t.Quantity,
            t.Screening.Ticket_price
        })
        .ToListAsync();

    // Beregn antal solgte billetter og omsætning
    var totalTickets = sales.Sum(s => s.Quantity);
    var totalRevenue = sales.Sum(s => (long)s.Quantity * s.Ticket_price);

    return Results.Ok(new
    {
        movie.Movie_id,
        movie.Title,
        total_tickets_sold = totalTickets,
        total_revenue = totalRevenue
    });
});


app.MapGet("/cinemas/revenue", async (MovieContext db) =>
{
    var cinemas = await db.Set<Cinema>()
        .Select(c => new
        {
            c.Cinema_id,
            c.Name,
            c.City,

            total_tickets_sold = db.Set<Ticket_sale>()
                .Where(t => t.Screening.Cinema_id == c.Cinema_id)
                .Sum(t => (int?)t.Quantity) ?? 0,

            total_revenue = db.Set<Ticket_sale>()
                .Where(t => t.Screening.Cinema_id == c.Cinema_id)
                .Sum(t => (long?)t.Quantity * t.Screening.Ticket_price) ?? 0L
        })
        .ToListAsync();

    return Results.Ok(cinemas);
});


app.MapGet("/actors/{id:int}/movie-performance", async (int id, MovieContext db) =>
{
    // Find skuespilleren
    var actor = await db.Set<Actor>()
        .Where(a => a.Actor_id == id)
        .Select(a => new { a.Actor_id, a.Name })
        .FirstOrDefaultAsync();

    if (actor == null)
        return Results.NotFound("Actor not found.");

    // Find skuespillerens film
    var movies = await db.Set<Movie_actor>()
        .Where(ma => ma.Actor_id == id)
        .Select(ma => new
        {
            ma.Movie_id,
            movie_title = ma.Movie.Title,

            tickets_sold = db.Set<Ticket_sale>()
                .Where(t => t.Screening.Movie_id == ma.Movie_id)
                .Sum(t => (int?)t.Quantity) ?? 0
        })
        .ToListAsync();

    // Beregn det samlede billetsalg
    var totalTickets = movies.Sum(m => m.tickets_sold);

    return Results.Ok(new
    {
        actor.Actor_id,
        actor.Name,
        movies,
        total_tickets_sold = totalTickets
    });
});

*/

//////////////////////////////////

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})




.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
