using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assignment_3_movievault.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace assignment_3_movievault.Controller;


[ApiController]
[Route("[controller]")]

public class MovieController : ControllerBase
{
    private readonly MovieServices _movieservices;
    private readonly MovieContext _context;
    
    public MovieController(MovieServices movieservices, MovieContext context)
    {
        _movieservices = movieservices;
        _context = context;
    }

    private async Task<List<int>?> GetSpecificMovieIdAsync(CancellationToken cancellationToken)
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllMovies(CancellationToken cancellationToken)
    {
        var movies = await _movieservices.GetAllMovieAsync(cancellationToken);
        var Ids = await GetSpecificMovieIdAsync(cancellationToken);

        if(Ids != null)
        {
            movies = movies.Where(m => Ids.Contains(m.Movie_id)).ToList();
        }

        return Ok(movies);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovie(int id, CancellationToken cancellationToken)
    {
        var movie = await _movieservices.GetMovieAsync(id, cancellationToken);
        var Ids = await GetSpecificMovieIdAsync(cancellationToken);
        if(movie == null)
        {
            return NotFound("Movie not found");
        }

        if(Ids != null && !Ids.Contains(id))
        {
            return Forbid();
        }

        return Ok(movie);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddMovie(CreateMovieDto dto, CancellationToken cancellationToken)
    {
        // Kontroller DTO'ens valideringsregler
    var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
    var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

    bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
        dto, validationContext, validationResults, true);

    if (!isValid)
    {
        return BadRequest(validationResults.Select(v => v.ErrorMessage));
    }

    // Kontroller filmens status
    string[] validStatuses = { "planned", "in production", "released", "archived" };

    if (!validStatuses.Contains(dto.Status))
    {
        return BadRequest("Invalid movie status.");
    }

    // Kontroller at instruktør og produktionsselskab findes
    if (!await _context.Directors.AnyAsync(d => d.Director_id == dto.Director_id))
    {
        return BadRequest("Director does not exist.");
    }


    // Dette skal på en måde integreres til MovieServices, sådan at
    // MovieContext med ikke bliver "exposed" af controlleren
    /*
    if (!await _context.Production_Companies.AnyAsync(c => c.Company_id == dto.Company_id))
    {
        return BadRequest("Production company does not exist.");
    }
    */


        var movie = await _movieservices.AddMovieAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetMovie), new {id = movie.Movie_id}, movie);
    } 

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, UpdateMovieDto dto, CancellationToken cancellationToken)
    {
        // Kontroller DTO'ens valideringsregler
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

        bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
            dto, validationContext, validationResults, true);

        if (!isValid)
        {
            return BadRequest(validationResults.Select(v => v.ErrorMessage));
        }

        // Kontroller filmens status
        string[] validStatuses = { "planned", "in production", "released", "archived" };

        if (!validStatuses.Contains(dto.Status))
        {
            return BadRequest("Invalid movie status.");
        }

        // Kontroller at instruktør og produktionsselskab findes
        if (!await _context.Directors.AnyAsync(d => d.Director_id == dto.Director_id))
        {
            return BadRequest("Director does not exist.");
        }

        if (!await _context.Production_Companies.AnyAsync(c => c.Company_id == dto.Company_id))
        {
            return BadRequest("Production company does not exist.");
        }

        var movie = await _movieservices.UpdateMovieAsync(id, dto, cancellationToken);
        
        if(movie == null)
        {
            return NotFound("Movie not found");
        }

        return Ok(movie);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id, CancellationToken cancellationToken)
    {
        var movie = await _movieservices.RemoveMovieAsync(id, cancellationToken);

        if(!movie)
        {
            return NotFound("Movie not found");
        }

        return NoContent();
    }

    [Authorize(Roles = "Actor,Director,Reviewer,Admin")]
    [HttpGet("overview")]
    public async Task<IActionResult> GetMovieOverview(CancellationToken cancellationToken)
    {
        var movie = await _movieservices.GetMovieOverview(cancellationToken);
        var Ids = await GetSpecificMovieIdAsync(cancellationToken);
        if(movie == null)
        {
            return NotFound("Movie not found");
        }

        if(Ids != null)
        {
            movie = movie.Where(m => Ids.Contains(m.Movie_id)).ToList();
        }

        return Ok(movie);
    }

    [Authorize(Roles = "Actor,Director,Reviewer,Admin")]
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetMovieDetails(int id, CancellationToken cancellationToken)
    {
        var movie = await _movieservices.GetMovieDetailsAsync(id, cancellationToken);
        var Ids = await GetSpecificMovieIdAsync(cancellationToken);

        if(movie == null)
        {
            return NotFound("Movie not found");
        }

        if(Ids != null && !Ids.Contains(id))
        {
            return Forbid();
        }

        return Ok(movie);
    }
}

