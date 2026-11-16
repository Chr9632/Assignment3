using assignment_3_movievault.DTO;
using Microsoft.EntityFrameworkCore;

namespace assignment_3_movievault.Services;
public class CinemaRevenue : ICinemaRevenue
{
    private readonly MovieContext _context;

    public CinemaRevenue(MovieContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<CinemaRevenueResponseDto>> GetCinemaRevenueAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<Cinema>()
        .Select(c => new CinemaRevenueResponseDto
        {
            Cinema_id = c.Cinema_id,
            Name = c.Name, 
            City = c.City, 

            Total_tickets_sold = _context.Set<Ticket_sale>()
            .Where(t => t.Screening.Cinema_id == c.Cinema_id)
            .Sum(t => (int?)t.Quantity) ?? 0,

            Total_revenue = _context.Set<Ticket_sale>()
            .Where(t => t.Screening.Cinema_id == c.Cinema_id)
            .Sum(t => (long?)t.Quantity * t.Screening.Ticket_price) ?? 0L
        })
        .ToListAsync();

    }
}