
using assignment_3_movievault.DTO;
using Microsoft.EntityFrameworkCore;

public class MovieLocation : IMovieLocation
{
    
    private readonly IRepository<Movie_location> _repository;
    private readonly MovieContext _context;

    public MovieLocation(MovieContext context, IRepository<Movie_location> repository)
    {
        _context = context;
        _repository = repository;
    }

    public async Task<MovieLocationResponseDto> AddMovieLocationAsync(int movieId, AddMovieLocationDto dto, CancellationToken cancellationToken)
    {
        var movieLocation = new Movie_location
        {
            Movie_id = movieId,
            Location_id = dto.Location_id,
            Shooting_days = dto.Shooting_days
        };

        await _repository.AddAsync(movieLocation);
        await _context.SaveChangesAsync(cancellationToken);

        return new MovieLocationResponseDto
        {
            Movie_id = movieLocation.Movie_id,
            Location_id = movieLocation.Location_id,
            Shooting_days = movieLocation.Shooting_days
        };
    }

    public async Task<bool> RemoveMovieLocationAsync(int movieId, int locationId, CancellationToken cancellationToken)
    {
        var movieLocation = await _context.Set<Movie_location>()
        .FirstOrDefaultAsync(ml => ml.Movie_id == movieId && ml.Location_id == locationId, cancellationToken);

        if(movieLocation == null)
        {
            return false;
        }

        _repository.Delete(movieLocation);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}