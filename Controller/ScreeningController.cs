using assignment_3_movievault.DTO;
using assignment_3_movievault.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ScreeningController : ControllerBase
{
    private readonly Screenings _screenings;

    public ScreeningController(Screenings screenings)
    {
        _screenings = screenings;
    }

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
        return Ok(screenings);
    }

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
        return Ok(screenings);
    }

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

    [HttpGet("/screenings/upcoming")]
    public async Task<IActionResult> ShowUpcomingScreening(CancellationToken cancellationToken)
    {
        var screenings = await _screenings.ShowUpcomingScreeningAsync(cancellationToken);
        return Ok(screenings);
    }
}
