using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assignment_3_movievault.DTO;

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

    [HttpGet]
    public async Task<IActionResult> GetAllMovies(CancellationToken cancellationToken)
    {
        var movies = await _movieservices.GetAllMovieAsync(cancellationToken);
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovie(int id, CancellationToken cancellationToken)
    {
        var movie = await _movieservices.GetMovieAsync(id, cancellationToken);
        if(movie == null)
        {
            return NotFound("Movie not found");
        }

        return Ok(movie);
    }

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

    [HttpGet("overview")]
    public async Task<IActionResult> GetMovieOverview(CancellationToken cancellationToken)
    {
        var movie = await _movieservices.GetMovieOverview(cancellationToken);
        if(movie == null)
        {
            return NotFound("Movie not found");
        }

        return Ok(movie);
    }


    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetMovieDetails(int id, CancellationToken cancellationToken)
    {
        var movie = await _movieservices.GetMovieDetailsAsync(id, cancellationToken);
        if(movie == null)
        {
            return NotFound("Movie not found");
        }

        return Ok(movie);
    }
}

