using assignment_3_movievault.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// From Line 414

[ApiController]
[Route("[controller]")]
public class MovieActorController : ControllerBase
{
    private readonly MovieActor _movieactor;
    private readonly MovieContext _context;

    public MovieActorController(MovieActor movieactor, MovieContext context)
    {
        _movieactor = movieactor;
        _context = context;
    }

    private async Task<List<int>?> GetSpecificActorsAsync(CancellationToken cancellationToken)
    {
        if (User.IsInRole("Admin"))
        {
            return null;
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new List<int>();
        }

        return await _context.Actors
            .Where(a => a.Identity_user_id == userId)
            .Select(a => a.Actor_id)
            .ToListAsync(cancellationToken);
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Actor,Admin")]
    [HttpGet("/actors/{id}/movie-performance")]
    public async Task<IActionResult> GetMoviePerformance(int id, CancellationToken cancellationToken)
    {
        var actorExists = await _context.Actors.AnyAsync(a => a.Actor_id == id, cancellationToken);
        if (!actorExists)
        {
            return NotFound("Actor not found");
        }

        var Ids = await GetSpecificActorsAsync(cancellationToken);
        if (Ids != null && !Ids.Contains(id))
        {
            return Forbid();
        }

        var actorMovie = await _movieactor.GetMoviePerformanceAsync(id, cancellationToken);
        if(actorMovie == null)
        {
            return NotFound("Actor not found");
        }
        
        return Ok(actorMovie);
    }

}