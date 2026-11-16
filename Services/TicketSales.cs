using assignment_3_movievault.DTO;
using Microsoft.EntityFrameworkCore;

public class TicketSales : ITicketSales
{
    private readonly IRepository<Ticket_sale> _repository;
    private readonly MovieContext _context;

    public TicketSales(MovieContext context, IRepository<Ticket_sale> repository)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<TicketSaleResponseDto>> GetAllTicketsAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<Ticket_sale>()
        .AsNoTracking()
        .Select(s => new TicketSaleResponseDto
        {
            Sale_id = s.Sale_id,
            Quantity = s.Quantity,
            Sale_date_time = s.Sale_date_time,
            Screening_id = s.Screening_id
        })
        .ToListAsync(cancellationToken);
    }
    public async Task<TicketSaleResponseDto?> GetTicketAsync(int id, CancellationToken cancellationToken)
    {
        var sale = await _context.Set<Ticket_sale>()
        .AsNoTracking()
        .Where(s => s.Sale_id == id)
        .Select(s => new TicketSaleResponseDto
        {
            Sale_id = s.Sale_id,
            Quantity = s.Quantity,
            Sale_date_time = s.Sale_date_time,
            Screening_id = s.Screening_id
        })
        .FirstOrDefaultAsync(cancellationToken);

        if(sale == null)
        {
            return null;
        }

        return sale;
    }
    public async Task<TicketSaleResponseDto> AddTicketAsync(CreateTicketSaleDto dto, CancellationToken cancellationToken)
    {
        var tickets = new Ticket_sale
        {
            Quantity = dto.Quantity,
            Sale_date_time = dto.Sale_date_time,
            Screening_id = dto.Screening_id
        };
        await _repository.AddAsync(tickets);
        await _context.SaveChangesAsync(cancellationToken);

        return new TicketSaleResponseDto
        {
            Sale_id = tickets.Sale_id,
            Quantity = tickets.Quantity,
            Sale_date_time = tickets.Sale_date_time,
            Screening_id = tickets.Screening_id
        };
    }
    public async Task<TicketSaleResponseDto?> UpdateTicketAsync(int id, UpdateTicketSaleDto dto, CancellationToken cancellationToken)
    {
        var tickets = await _repository.GetByIdAsync(id, cancellationToken);
        if(tickets == null)
        {
            return null;
        }

        /*
        var screening = await _context.Set<Screening>()
        .Include(s => s.Screen)
        .FirstOrDefaultAsync(s => s.Screening_id == dto.Screening_id);

        if(screening == null)
        {
            return null; 
        }

        int ticketsSold = await _context.Set<Ticket_sale>()
        .Where(t => t.Screening_id == dto.Screening_id && t.Sale_id != id)
        .SumAsync(t => t.Quantity);

        */

        tickets.Quantity = dto.Quantity;
        tickets.Sale_date_time = dto.Sale_date_time;
        tickets.Screening_id = dto.Screening_id;

        return new TicketSaleResponseDto
        {
            Sale_id = tickets.Sale_id,
            Quantity = tickets.Quantity,
            Sale_date_time = tickets.Sale_date_time,
            Screening_id = tickets.Screening_id
        };

    } 
    public async Task<bool> RemoveTicketAsync(int id, CancellationToken cancellationToken)
    {
        var tickets = await _repository.GetByIdAsync(id, cancellationToken);
        if(tickets == null)
        {
            return false;
        }

        _repository.Delete(tickets);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<MovieTicketSalesResponseDto>> GetTicketSalesMovieAsync(int id, CancellationToken cancellationToken)
    {
        var movie = await _context.Set<Movie>()
        .Where(m => m.Movie_id == id)
        .Select(m => new { m.Movie_id, m.Title})
        .FirstOrDefaultAsync(cancellationToken);

        if(movie == null)
        {
            return Enumerable.Empty<MovieTicketSalesResponseDto>();
        }

        var sales = await _context.Set<Ticket_sale>()
        .Where(t => t.Screening.Movie_id == id)
        .Select(t => new 
        {
            t.Quantity,
            t.Screening.Ticket_price
        })
        .ToListAsync(cancellationToken);

        var totalTickets = sales.Sum(t => t.Quantity);
        var totalRevenue = sales.Sum(s => (long)s.Quantity * s.Ticket_price);

        return new[] { new MovieTicketSalesResponseDto
        {
            Movie_id = movie.Movie_id,
            Title = movie.Title,
            Total_tickets_sold = totalTickets, 
            Total_revenue = totalRevenue
        } };
    }
}