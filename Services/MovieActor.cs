using assignment_3_movievault.DTO;
using Microsoft.EntityFrameworkCore;

public class MovieActor : IMovieActor
{
    private readonly IRepository<Movie_actor> _repository;
    private readonly MovieContext _context;

    public MovieActor(MovieContext context, IRepository<Movie_actor> repository)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<MovieActorResponseDto> AddMovieActorAsync(int movieId, AddMovieActorDto dto, CancellationToken cancellationToken)
    {
        var movieActor = new Movie_actor
        {
            Movie_id = movieId, 
            Actor_id = dto.Actor_id, 
            Role = dto.Role,
            Character_name = dto.Character_name
        };

        await _repository.AddAsync(movieActor);
        await _context.SaveChangesAsync(cancellationToken);

        return new MovieActorResponseDto
        {
            Movie_id = movieActor.Movie_id, 
            Actor_id = movieActor.Actor_id, 
            Role = movieActor.Role,
            Character_name = movieActor.Character_name
        };
    }
    public async Task<bool> RemoveMovieActorAsync(int movieId, int actorId, CancellationToken cancellationToken)
    {
        var movieActor = await _context.Set<Movie_actor>()
        .FirstOrDefaultAsync(ma => ma.Movie_id == movieId && ma.Actor_id == actorId, cancellationToken);

        if(movieActor == null)
        {
            return false;
        }

        _repository.Delete(movieActor);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IEnumerable<ActorMoviePerformanceResponseDto>> GetMoviePerformanceAsync(int id, CancellationToken cancellationToken)
    {
        var actor = await _context.Set<Actor>()
            .Where(a => a.Actor_id == id)
            .Select(a => new { a.Actor_id, a.Name })
            .FirstOrDefaultAsync(cancellationToken);

        if (actor == null)
        {
            return Enumerable.Empty<ActorMoviePerformanceResponseDto>();
        }

        var movies = await _context.Set<Movie_actor>()
            .Where(ma => ma.Actor_id == id)
            .Select(ma => new ActorMoviePerformanceMovieDto
            {
                Movie_id = ma.Movie_id,
                Movie_title = ma.Movie.Title,
                Tickets_sold = _context.Set<Ticket_sale>()
                    .Where(t => t.Screening.Movie_id == ma.Movie_id)
                    .Sum(t => (int?)t.Quantity) ?? 0
            })
            .ToListAsync(cancellationToken);

        var totalTickets = movies.Sum(m => m.Tickets_sold);

        return new[]
        {
            new ActorMoviePerformanceResponseDto
            {
                Actor_id = actor.Actor_id,
                Name = actor.Name,
                Movies = movies,
                Total_tickets_sold = totalTickets
            }
        };
    }
}