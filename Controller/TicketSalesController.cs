using assignment_3_movievault.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TicketSalesController : ControllerBase
{
    private readonly TicketSales _ticketsales;

    public TicketSalesController(TicketSales ticketsales)
    {
        _ticketsales = ticketsales;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTickets(CancellationToken cancellationToken)
    {
        var tickets = await _ticketsales.GetAllTicketsAsync(cancellationToken);
        if(tickets == null)
        {
            return NotFound("Tickets not found");
        }

        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(int id, CancellationToken cancellationToken)
    {
        var ticket = await _ticketsales.GetTicketAsync(id, cancellationToken);
        if(ticket == null)
        {
            return NotFound("Ticket sale not found");
        }

        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> AddTicket(CreateTicketSaleDto dto, CancellationToken cancellationToken)
    {
        
        // Kontrollér antal billetter
        if (dto.Quantity <= 0)
            return BadRequest("Quantity must be greater than 0.");

        // Kontrollér salgsdato
        if (dto.Sale_date_time == default ||
            dto.Sale_date_time >= DateTime.Now)
            return BadRequest("Sale date must be in the past.");


        // Samme sag som MovieController.cs
        /*
        // Find forestillingen og dens biografsal
        var screening = await db.Set<Screening>()
            .Include(s => s.Screen)
            .FirstOrDefaultAsync(s => s.Screening_id == dto.Screening_id);

        if (screening == null)
            return BadRequest("Screening does not exist.");

        // Find antal billetter, der allerede er solgt
        int ticketsSold = await db.Set<Ticket_sale>()
            .Where(t => t.Screening_id == dto.Screening_id)
            .SumAsync(t => t.Quantity);

        // Find biografsalens kapacitet
        int capacity = screening.Screen.Seating_capacity;

        // Kontrollér om der er plads til de nye billetter
        if ((long)ticketsSold + dto.Quantity > capacity)
            return Conflict(
                "Not enough available seats for this screening.");
        */

        var tickets = await _ticketsales.AddTicketAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetTicket), new {id = tickets.Sale_id}, tickets); 
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(int id, UpdateTicketSaleDto dto, CancellationToken cancellationToken)
    {
        var tickets = await _ticketsales.UpdateTicketAsync(id, dto, cancellationToken);

        if (tickets == null)
        {
            return NotFound("Ticket sale not found.");
        }

        // Kontrollér antal billetter
        if (dto.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than 0.");
        }

        // Kontrollér salgsdato
        if (dto.Sale_date_time == default ||
            dto.Sale_date_time >= DateTime.Now)
        {
            return BadRequest("Sale date must be in the past.");
        }

        // Same issue as MovieController.cs
        /*
            // Find forestillingen og dens biografsal
            var screening = await db.Set<Screening>()
                .Include(s => s.Screen)
                .FirstOrDefaultAsync(s =>
                    s.Screening_id == dto.Screening_id);

            if (screening == null)
                return Results.BadRequest("Screening does not exist.");

            // Find antal billetter solgt til forestillingen,
            // men undlad det billetsalg, vi er ved at opdatere
            int ticketsSold = await db.Set<Ticket_sale>()
                .Where(t =>
                    t.Screening_id == dto.Screening_id &&
                    t.Sale_id != id)
                .SumAsync(t => t.Quantity);

            // Find biografsalens kapacitet
            int capacity = screening.Screen.Seating_capacity;

            // Kontrollér om der er plads til det opdaterede antal
            if ((long)ticketsSold + dto.Quantity > capacity)
                return Results.Conflict(
                    "Not enough available seats for this screening.");
        */
            
        return Ok(tickets);

    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveTicket(int id, CancellationToken cancellationToken)
    {
        var tickets = await _ticketsales.RemoveTicketAsync(id, cancellationToken);
        if(!tickets)
        {
            return NotFound("Ticket sale not found");
        }

        return NoContent();
    }

    [HttpGet("{id}/TicketSales")]
    public async Task<IActionResult> GetTicketSalesMovie(int id, CancellationToken cancellationToken)
    {
        var tickets = await _ticketsales.GetTicketSalesMovieAsync(id, cancellationToken);
        
        return Ok(tickets);
    }

}