using assignment_3_movievault.DTO;
using Microsoft.EntityFrameworkCore;

public class MovieServices : IMovieService
{
    private readonly IRepository<Movie> _repository;
    private readonly MovieContext _context;

    public MovieServices(MovieContext context, IRepository<Movie> repository)
    {
        _context = context;
        _repository = repository;
    }

    public async Task<IEnumerable<MovieResponseDto>> GetAllMovieAsync(CancellationToken cancellationToken)
    {
        return await _context.Movies
        .AsNoTracking()
        .Select(m => new MovieResponseDto
        {
            Movie_id = m.Movie_id,
            Title = m.Title,
            Release_date = m.Release_date,
            Runtime = m.Runtime,
            Age_rating = m.Age_rating,
            Status = m.Status,
            Director_id = m.Director_id,
            Company_id = m.Company_id
        })
        .ToListAsync(cancellationToken);
    }

    public async Task<MovieResponseDto?> GetMovieAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Movies
        .AsNoTracking()
        .Where(m => m.Movie_id == id)
        .Select(m => new MovieResponseDto
        {
            Movie_id = m.Movie_id,
            Title = m.Title,
            Release_date = m.Release_date,
            Runtime = m.Runtime,
            Age_rating = m.Age_rating,
            Status = m.Status,
            Director_id = m.Director_id,
            Company_id = m.Company_id
        })
        .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> MovieExistsAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Movies.AnyAsync(m => m.Movie_id == id, cancellationToken);
    }

    public async Task<MovieResponseDto> AddMovieAsync(CreateMovieDto dto, CancellationToken cancellationToken)
    {
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

        await _repository.AddAsync(movie);
        await _context.SaveChangesAsync(cancellationToken);

        return new MovieResponseDto
        {
            Movie_id = movie.Movie_id,
            Title = movie.Title,
            Release_date = movie.Release_date,
            Runtime = movie.Runtime,
            Age_rating = movie.Age_rating,
            Status = movie.Status,
            Director_id = movie.Director_id,
            Company_id = movie.Company_id
        };
    }
    public async Task<MovieResponseDto?> UpdateMovieAsync(int id, UpdateMovieDto dto, CancellationToken cancellationToken)
    {
        var movie = await _repository.GetByIdAsync(id, cancellationToken);
        
        if(movie == null)
        {
            return null;
        }
        
        movie.Title = dto.Title;
        movie.Release_date = dto.Release_date;
        movie.Runtime = dto.Runtime;
        movie.Age_rating = dto.Age_rating;
        movie.Status = dto.Status;
        movie.Director_id = dto.Director_id;
        movie.Company_id = dto.Company_id;
        await _context.SaveChangesAsync(cancellationToken);

        return new MovieResponseDto
        {
            Movie_id = movie.Movie_id,
            Title = movie.Title,
            Release_date = movie.Release_date,
            Runtime = movie.Runtime,
            Age_rating = movie.Age_rating,
            Status = movie.Status,
            Director_id = movie.Director_id,
            Company_id = movie.Company_id
        };
    }
    public async Task<bool> RemoveMovieAsync(int id, CancellationToken cancellationToken)
    {
        var movie = await _repository.GetByIdAsync(id, cancellationToken);
        if(movie == null)
        {
            return false;
        }
        _repository.Delete(movie);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IEnumerable<MovieOverviewResponseDto>> GetMovieOverview(CancellationToken cancellationToken)
    {
        return await _context.Set<Movie>()
        .AsNoTracking()
        .Select(m => new MovieOverviewResponseDto
        {
            Movie_id = m.Movie_id,
            Title = m.Title,
            Release_date = m.Release_date,
            Runtime = m.Runtime,
            Age_rating = m.Age_rating,
            Status = m.Status,

            Director = m.Director.Name,
            Production_company = m.Production_company.Name,
            Production_budget = m.Production_Record.Production_budget
        })
        .ToListAsync();
    }

    public async Task<IEnumerable<MovieDetailsResponseDto>> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
    {
        var movie = await _context.Set<Movie>()
        .AsNoTracking()
        .Where(m => m.Movie_id == id)
        .Select(m => new MovieDetailsResponseDto
        {
            Movie_id = m.Movie_id,
            Title = m.Title,
            Release_date = m.Release_date,
            Runtime = m.Runtime,
            Age_rating = m.Age_rating, 
            Status = m.Status,

            Director = m.Director.Name,
            Production_company = m.Production_company.Name,
            Production_budget = m.Production_Record.Production_budget,

            Actors = m.Movie_actor.Select(ma => new MovieDetailsActorDto
            {
                Actor_id = ma.Actor_id, 
                Actor_name = ma.Actor.Name,
                Role = ma.Role,
                Character_name = ma.Character_name
            }).ToList(),

            Filming_locations = m.Movie_location.Select(ml => new MovieDetailsLocationDto
            {
                Location_id = ml.Location_id,
                Location_name = ml.Filming_location.Name, 
                City = ml.Filming_location.City,
                Country = ml.Filming_location.Country,
                Location_type = ml.Filming_location.Location_type,
                Shooting_days = ml.Shooting_days
            }).ToList()
        })
        .FirstOrDefaultAsync(cancellationToken);

        if(movie == null)
        {
            return Enumerable.Empty<MovieDetailsResponseDto>();
        }

        return new[]
        {
            new MovieDetailsResponseDto
            {
                Movie_id = movie.Movie_id,
                Title = movie.Title, 
                Release_date = movie.Release_date,
                Runtime = movie.Runtime,
                Age_rating = movie.Age_rating,
                Status = movie.Status,
                Director = movie.Director,
                Production_company = movie.Production_company,
                Production_budget = movie.Production_budget,
                Actors = movie.Actors,
                Filming_locations = movie.Filming_locations
            }
        };
    }
}