using assignment_3_movievault.DTO;
using Microsoft.EntityFrameworkCore;

namespace assignment_3_movievault.Services;
public class Screenings : IScreening
{
    private readonly IRepository<Screening> _repository;
    private readonly MovieContext _context;
    public Screenings(MovieContext context, IRepository<Screening> repository)
    {
        _repository = repository;
        _context = context;
    }
    public async Task<IEnumerable<ScreeningResponseDto>> GetScreeningMoviesAsync(int movieId, CancellationToken cancellationToken)
    {
        return await _context.Set<Screening>()
        .AsNoTracking()
        .Where(s => s.Movie_id == movieId)
        .Select(s => new ScreeningResponseDto
        {
            Screening_id = s.Screening_id,
            Date = s.Date,
            Start_time = s.Start_time,
            Ticket_price = s.Ticket_price,
            Movie_id = s.Movie_id,
            Screen_number = s.Screen_number,
            Cinema_id = s.Cinema_id
        })
        .ToListAsync(cancellationToken);
    }
    public async Task<ScreeningResponseDto> AddScreeningAsync(CreateScreeningDto dto, CancellationToken cancellationToken)
    {
        var screening = new Screening
        {
            Date = dto.Date,
            Start_time = dto.Start_time,
            Ticket_price = dto.Ticket_price, 
            Movie_id = dto.Movie_id,
            Screen_number = dto.Screen_number,
            Cinema_id = dto.Cinema_id
        };

        await _repository.AddAsync(screening);
        await _context.SaveChangesAsync(cancellationToken);

        return new ScreeningResponseDto
        {
            Screening_id = screening.Screening_id,
            Date = screening.Date,
            Start_time = screening.Start_time,
            Ticket_price = screening.Ticket_price,
            Movie_id = screening.Movie_id,
            Screen_number = screening.Screen_number,
            Cinema_id = screening.Cinema_id
        };
    }
    public async Task<IEnumerable<ScreeningResponseDto>> GetScreeningCinemasAsync(int cinemaId, CancellationToken cancellationToken)
    {
         return await _context.Set<Screening>()
        .AsNoTracking()
        .Where(s => s.Cinema_id == cinemaId)
        .Select(s => new ScreeningResponseDto
        {
            Screening_id = s.Screening_id,
            Date = s.Date,
            Start_time = s.Start_time,
            Ticket_price = s.Ticket_price,
            Movie_id = s.Movie_id,
            Screen_number = s.Screen_number,
            Cinema_id = s.Cinema_id
        })
        .ToListAsync(cancellationToken);
    }
    public async Task<ScreeningResponseDto?> UpdateScreeningAsync(int id, UpdateScreeningDto dto, CancellationToken cancellationToken)
    {
        var screening = await _repository.GetByIdAsync(id, cancellationToken);
        if(screening == null)
        {
            return null;
        }

        var screen = await _context.Set<Screen>()
        .FirstOrDefaultAsync(s => s.Screen_number == dto.Screen_number && s.Cinema_id == dto.Cinema_id, cancellationToken);

        if(screen == null)
        {
            return null;
        }

        // Opdater forestillingen
        screening.Date = dto.Date;
        screening.Start_time = dto.Start_time;
        screening.Ticket_price = dto.Ticket_price;
        screening.Movie_id = dto.Movie_id;
        screening.Screen_number = dto.Screen_number;
        screening.Cinema_id = dto.Cinema_id;

        await _context.SaveChangesAsync(cancellationToken);

        return new ScreeningResponseDto
        {
            Screening_id = screening.Screening_id,
            Date = screening.Date,
            Start_time = screening.Start_time,
            Ticket_price = screening.Ticket_price,
            Movie_id = screening.Movie_id,
            Screen_number = screening.Screen_number,
            Cinema_id = screening.Cinema_id
        };
    }
    public async Task<bool> RemoveScreeningAsync(int id, CancellationToken cancellationToken)
    {
        var screening = await _repository.GetByIdAsync(id, cancellationToken);
        if(screening == null)
        {
            return false;
        }

        _repository.Delete(screening);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IEnumerable<UpcomingScreeningResponseDto>> ShowUpcomingScreeningAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.Now;

        return await _context.Set<Screening>()
        .AsNoTracking()
        .Where(s => s.Start_time > now)
        .OrderBy(s => s.Start_time)
        .Select(s => new UpcomingScreeningResponseDto
        {
            Screening_id = s.Screening_id,
            Movie_title = s.Movie.Title,
            Cinema_name = s.Screen.Cinema.Name,
            Cinema_city = s.Screen.Cinema.City,
            Screen_number = s.Screen_number,
            Screen_name = s.Screen.Name,
            Date = s.Date,
            Start_time = s.Start_time,
            Ticket_price = s.Ticket_price
        })
        .ToListAsync(cancellationToken);
    }
}