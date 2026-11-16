using assignment_3_movievault.DTO;
using Microsoft.AspNetCore.Mvc;

// From Line 414

[ApiController]
[Route("[controller]")]
public class MovieActorController : ControllerBase
{
    private readonly MovieActor _movieactor;

    public MovieActorController(MovieActor movieactor)
    {
        _movieactor = movieactor;
    }

    
    [HttpPost("/movies/{movieId}/actors")]
    public async Task<IActionResult> AddMovieActor(int movieId, AddMovieActorDto dto, CancellationToken cancellationToken)
    {
        // Same issue as MovieController.cs
        /*
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
            return Conflict("Actor is already assigned to this movie.");
        */

        var movieActor = await _movieactor.AddMovieActorAsync(movieId, dto, cancellationToken);
        return CreatedAtAction(nameof(AddMovieActor), new { movieId }, movieActor);
        
    }

    [HttpDelete("/movies/{movieId}/actors/{actorId}")]
    public async Task<IActionResult> RemoveMovieActor(int movieId, int actorId, CancellationToken cancellationToken)
    {
        var movieActor = await _movieactor.RemoveMovieActorAsync(movieId, actorId, cancellationToken);
        if(!movieActor)
        {
            return NotFound("Actor is not assigned to this movie");
        }

        return NoContent();
    }

    [HttpGet("/actors/{id}/movie-performance")]
    public async Task<IActionResult> GetMoviePerformance(int id, CancellationToken cancellationToken)
    {
        var actorMovie = await _movieactor.GetMoviePerformanceAsync(id, cancellationToken);
        if(actorMovie == null)
        {
            return NotFound("Actor not found");
        }
        
        return Ok(actorMovie);
    }
}