using assignment_3_movievault.DTO;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public interface ITicketSales
{
    public Task<IEnumerable<TicketSaleResponseDto>> GetAllTicketsAsync(CancellationToken cancellationToken);
    public Task<TicketSaleResponseDto?> GetTicketAsync(int id, CancellationToken cancellationToken);
    public Task<TicketSaleResponseDto> AddTicketAsync(CreateTicketSaleDto dto, CancellationToken cancellationToken);
    public Task<TicketSaleResponseDto?> UpdateTicketAsync(int id, UpdateTicketSaleDto dto, CancellationToken cancellationToken); 
    public Task<bool> RemoveTicketAsync(int id, CancellationToken cancellationToken);

    public Task<IEnumerable<MovieTicketSalesResponseDto>> GetTicketSalesMovieAsync(int id, CancellationToken cancellationToken);
}