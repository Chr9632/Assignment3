using assignment_3_movievault.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class MovieLocationController : ControllerBase
{
    private readonly MovieLocation _movielocation;

    public MovieLocationController(MovieLocation movielocation)
    {
        _movielocation = movielocation;
    }

    [HttpPost("/movies/{movieId}/locations")]
    public async Task<IActionResult> AddMovieLocation(int movieId, AddMovieLocationDto dto, CancellationToken cancellationToken)
    {
        // Same issue as MovieController.cs
        /*
        // Kontrollér at filmen findes
        if (!await db.Set<Movie>().AnyAsync(m => m.Movie_id == movieId))
            return NotFound("Movie not found.");

        // Kontrollér at optagelsesstedet findes
        if (!await db.Set<Filming_location>()
            .AnyAsync(l => l.Location_id == dto.Location_id))
            return NotFound("Filming location not found.");

        // Kontrollér at optagelsesstedet ikke allerede er tilføjet
        bool exists = await db.Set<Movie_location>().AnyAsync(ml =>
            ml.Movie_id == movieId &&
            ml.Location_id == dto.Location_id);

        if (exists)
            return Conflict("Filming location is already assigned to this movie.");
        */

        var movieLocation = await _movielocation.AddMovieLocationAsync(movieId, dto, cancellationToken);
        return CreatedAtAction(nameof(AddMovieLocation), new { movieId }, movieLocation);
    }

    [HttpDelete("/movies/{movieId}/locations/{locationId}")]
    public async Task<IActionResult> RemoveMovieLocation(int movieId, int locationId, CancellationToken cancellationToken)
    {
        var movieLocation = await _movielocation.RemoveMovieLocationAsync(movieId, locationId, cancellationToken);
        if(!movieLocation)
        {
            return NotFound("Filming location is not assigned to this movie");
        }

        return NoContent();
    }
}
