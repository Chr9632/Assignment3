using System.Security.Claims;
using assignment_3_movievault.DTO;
using assignment_3_movievault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class ScreeningController : ControllerBase
{
    private readonly Screenings _screenings;
    private readonly MovieContext _context;

    public ScreeningController(Screenings screenings, MovieContext context)
    {
        _screenings = screenings;
        _context = context;
    }

    private async Task<List<int>?> GetSpecificScreeningsAsync(CancellationToken cancellationToken)
    {
        if(User.IsInRole("Reviewer") || User.IsInRole("Admin"))
        {
            return null;
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(string.IsNullOrEmpty(userId))
        {
            return new List<int>();
        }

        if(User.IsInRole("Actor"))
        {
            return await _context.Movie_actors.Where(ma => ma.Actor.Identity_user_id == userId)
            .Select(ma => ma.Movie_id)
            .ToListAsync(cancellationToken);
        }

        if(User.IsInRole("Director"))
        {
            return await _context.Movies.Where(m => m.Director.Identity_user_id == userId)
            .Select(m => m.Movie_id)
            .ToListAsync(cancellationToken);
        }

        return new List<int>();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("/screenings")]
    public async Task<IActionResult> AddScreening(CreateScreeningDto dto, CancellationToken cancellationToken)
    {
        // Same issue as MovieController.cs
        /*
        // Kontrollér at filmen findes
        if (!await db.Set<Movie>().AnyAsync(m => m.Movie_id == dto.Movie_id))
            return BadRequest("Movie does not exist.");

        // Kontrollér at biografsalen findes
        if (!await db.Set<Screen>().AnyAsync(s =>
            s.Screen_number == dto.Screen_number &&
            s.Cinema_id == dto.Cinema_id))
            return BadRequest("Screen does not exist.");

        // Kontrollér billetprisen
        if (dto.Ticket_price < 0)
            return BadRequest("Ticket price cannot be negative.");

        // Kontrollér dato og starttidspunkt
        if (dto.Date == default || dto.Start_time == default)
            return BadRequest("Date and start time are required.");

        // Kontrollér at dato og starttidspunkt er samme dag
        if (dto.Date.Date != dto.Start_time.Date)
            return BadRequest("Date and start time must be on the same day.");
        */

        var screening = await _screenings.AddScreeningAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(UpdateScreening), new { id = screening.Screening_id }, screening);
    }

    [Authorize(Roles = "Actor, Director, Reviewer, Admin")]
    [HttpGet("/movies/{movieId}/screenings")]
    public async Task<IActionResult> GetScreeningMovies(int movieId, CancellationToken cancellationToken)
    {
        // Same issue as MovieController.cs
        /*
        // Kontrollér at filmen findes
        if (!await db.Set<Movie>().AnyAsync(m => m.Movie_id == movieId))
            return NotFound("Movie not found.");
        */

        var screenings = await _screenings.GetScreeningMoviesAsync(movieId, cancellationToken);

        var Ids = await GetSpecificScreeningsAsync(cancellationToken);

        if(Ids != null && !Ids.Contains(movieId))
        {
            return Forbid();
        }

        return Ok(screenings);
    }

    [Authorize(Roles = "Actor, Director, Reviewer, Admin")]
    [HttpGet("/cinemas/{cinemaId}/screenings")]
    public async Task<IActionResult> GetScreeningCinemas(int cinemaId, CancellationToken cancellationToken)
    {
        // Same issue as MovieController.cs
        /*
        // Kontrollér at biografen findes
        if (!await db.Set<Cinema>().AnyAsync(c => c.Cinema_id == cinemaId))
            return NotFound("Cinema not found.");
        */

        var screenings = await _screenings.GetScreeningCinemasAsync(cinemaId, cancellationToken);
        var Ids = await GetSpecificScreeningsAsync(cancellationToken);

        if(Ids != null)
        {
            screenings = screenings.Where(s => Ids.Contains(s.Movie_id)).ToList();
        }

        return Ok(screenings);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("/screenings/{id}")]
    public async Task<IActionResult> UpdateScreening(int id, UpdateScreeningDto dto, CancellationToken cancellationToken)
    {
        // Same issue as MovieController.cs
        /*
        // Kontrollér dato og tidspunkt
        if (dto.Date == default || dto.Start_time == default)
            return BadRequest("Date and start time are required.");

        if (dto.Date.Date != dto.Start_time.Date)
            return BadRequest("Date and start time must refer to the same day.");

        // Kontrollér billetprisen
        if (dto.Ticket_price < 0)
            return BadRequest("Ticket price cannot be negative.");

        // Kontrollér at filmen findes
        if (!await db.Set<Movie>().AnyAsync(m => m.Movie_id == dto.Movie_id))
            return BadRequest("Movie does not exist.");

        // Kontrollér at biografsalen findes
        if (!await db.Set<Screen>().AnyAsync(s =>
            s.Screen_number == dto.Screen_number &&
            s.Cinema_id == dto.Cinema_id))
            return BadRequest("Screen does not exist.");
        */

        var screening = await _screenings.UpdateScreeningAsync(id, dto, cancellationToken);
        if(screening == null)
        {
            return NotFound("Screening not found.");
        }

        return Ok(screening);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("/screenings/{id}")]
    public async Task<IActionResult> RemoveScreening(int id, CancellationToken cancellationToken)
    {
        // Same issue as MovieController.cs
        /*
        // Undgå at slette forestillinger med billetsalg
        bool hasTicketSales = await db.Set<Ticket_sale>()
            .AnyAsync(t => t.Screening_id == id);

        if (hasTicketSales)
            return Conflict("Cannot delete a screening with ticket sales.");
        */

        var screening = await _screenings.RemoveScreeningAsync(id, cancellationToken);
        if(!screening)
        {
            return NotFound("Screening not found.");
        }

        return NoContent();
    }

    [Authorize(Roles = "Actor, Director, Reviewer, Admin")]
    [HttpGet("/screenings/upcoming")]
    public async Task<IActionResult> ShowUpcomingScreening(CancellationToken cancellationToken)
    {
        var screenings = await _screenings.ShowUpcomingScreeningAsync(cancellationToken);

        var Ids = await GetSpecificScreeningsAsync(cancellationToken);

        if(Ids != null)
        {
            var titles = await _context.Movies.Where(m => Ids.Contains(m.Movie_id))
            .Select(m => m.Title)
            .ToListAsync(cancellationToken);

            screenings = screenings.Where(s => titles.Contains(s.Movie_title)).ToList();
        }
        
        return Ok(screenings);
    }
}
