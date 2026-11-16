
using assignment_3_movievault.DTO;

public interface ICinemaRevenue
{
    public Task<IEnumerable<CinemaRevenueResponseDto>> GetCinemaRevenueAsync(CancellationToken cancellationToken);
}